using System;
using UnityEngine;

using Freshaliens.Management;
using Unity.Mathematics;

namespace Freshaliens.Player.Components
{
    /// <summary>
    /// Component that handles player movement 
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(PlayerInputHandler)), RequireComponent(typeof(PlayerFairyDetector))]
    public class PlayerMovementController : SingletonMonobehaviour<PlayerMovementController>, IMovementController
    {
        private const float minVerticalVelocityForJump = 0.001f;
        [SerializeField] private Animator _animator;
        [Header("Walking")]
        [SerializeField] private float movementSpeedStart = 5.0f;
        [SerializeField] private float movementSpeedMax = 6.0f;
        [SerializeField] private float walkAcceleration = 1.0f;

        [Header("Jumping")]
        [SerializeField, Tooltip("Jump force when jumping from the ground")] private float jumpForceGrounded = 10.0f;
        [SerializeField, Tooltip("Jump force when fairy-jumping")] private float jumpForceAirborne = 10.0f;
        [SerializeField] private int maxAirJumps = 1;
        [SerializeField] private float jumpQueueFrame = 0.15f;
        [SerializeField] private float wallJumpIgnoreInputFrame = 0.1f;

        [Header("Gravity control")]
        [SerializeField] private float gravityScaleDefault = 1f;
        [SerializeField] private float gravityScaleFalling = 1.3f;
        [SerializeField] private float gravityScaleClimbing = 0.2f;
        [Space(5)]
        [SerializeField] private float terminalVelocity = 20;
        [SerializeField] private float coyoteTimeFrame = 0.1f;

        [Header("Ground check")]
        [SerializeField] private Transform[] groundChecks = new Transform[0];
        [SerializeField] private Transform leftGroundCheck = null;
        [SerializeField] private Transform rightGroundCheck = null;
        [SerializeField] private float groundCheckRadius = 0.1f;
        [SerializeField] private LayerMask groundLayers = 0;

        [Header("Misc")]
        [SerializeField] private Transform enemyProjectileTarget = null;
        [Space(5)]
        [SerializeField] private AudioSource movementAudioSource = null;
        [SerializeField] private AudioClip stepAudioClip = null;
        [SerializeField] private AudioClip jumpAudioClip = null;

        // State
        private bool isMoving = false;
        private bool isGrounded = false;
        private bool wasGrounded = false;
        private bool isWithinCoyoteTime = false;
        private bool jumpQueued = false;
        private bool hasJumpedSinceGrounded = false;    // this might use a better name
        private bool hasChangedGroundTransform = true;
        private int remainingAirJumps = 0;
        //to look into reaminingAirJumps
        private float currentSpeed = 0f;
        private float jumpPressedTimestamp = 0f;    // time of last jump press (for buffering)
        private float lastGroundedTimestamp = 0f;   // time player was last grounded (for coyote time)
        private float lastFacedDirection = 1f;
        private Vector2 velocity = Vector2.zero;
        private Vector3 previousGroundPosition = Vector3.zero;
        private Transform groundTransform = null;

        // Components
        private PlayerInputHandler input = null;
        private Rigidbody2D rbody = null;
        private Transform ownTransform = null;
        private PlayerFairyDetector fairyDetector = null;

        // Events
        public event Action onJumpWhileGrounded;
        public event Action onJumpWhileAirborne;
        public event Action onLand;

        // Properties
        public Vector3 Position => ownTransform.position;
        public float LastFacedDirection => lastFacedDirection;
        public int RemainingAirJupms => remainingAirJumps;
        public Vector3 EnemyProjectileTarget => enemyProjectileTarget.position;

        private void Start()
        {
            input = GetComponent<PlayerInputHandler>();
            rbody = GetComponent<Rigidbody2D>();
            ownTransform = transform;
            fairyDetector = GetComponent<PlayerFairyDetector>();

            remainingAirJumps = maxAirJumps;

            if (!leftGroundCheck) leftGroundCheck = transform.Find("Ground Check Left");
            if (!rightGroundCheck) rightGroundCheck = transform.Find("Ground Check Right");
#if UNITY_EDITOR
            if (!leftGroundCheck || !rightGroundCheck) Debug.LogWarning("Missing ground checks in player prefab!");
#endif
        }

        private void Update()
        {
            // TODO Ugly, fix state
            if (!LevelManager.Instance.IsPlaying)
            {
                rbody.velocity = Vector2.zero;
                return;
            }

            // Input

            float direction = input.GetHorizontal();
            if (direction != 0) lastFacedDirection = direction;
            //animation update
            _animator.SetFloat("DirectionR", lastFacedDirection);

            if (input.GetJumpInput())
            {
                jumpPressedTimestamp = Time.time;
                jumpQueued = true;
            }
            // Handle jump queueing
            if (Time.time - jumpPressedTimestamp > jumpQueueFrame)
                jumpQueued = false;

            // Walking
            isMoving = Mathf.Abs(direction) > 0f;
            //changing animation state
            _animator.SetBool("IsMoving", isMoving);
            if (isMoving)
            {
                // Accelerate until max speed
                currentSpeed = Mathf.Clamp(currentSpeed + walkAcceleration * Time.deltaTime, movementSpeedStart, movementSpeedMax);
            }
            else
            {
                currentSpeed = 0;
            }

            // Ignore input direction if walljumping, apply it if not within the ignore input time frame
            velocity.x = direction * currentSpeed;

            Vector2 groundVelocity = Vector2.zero;
            if (isGrounded)
            {
                // Reset air jumps
                remainingAirJumps = maxAirJumps;

                // Add ground movement
                if (wasGrounded)
                {
                    if (!hasChangedGroundTransform) // Not the best fix for the trapdoor glitch but OK for now
                        groundVelocity = groundTransform.position - previousGroundPosition;
                }
                else {
                    onLand?.Invoke();
                }

                previousGroundPosition = groundTransform.position;
                lastGroundedTimestamp = Time.time;
            }
            else if (rbody.velocity.y < minVerticalVelocityForJump) hasJumpedSinceGrounded = false; // Not sure if this is the best fix for the double jump bug but here it is

            // Jumping
            isWithinCoyoteTime = Time.time - lastGroundedTimestamp <= coyoteTimeFrame && !hasJumpedSinceGrounded; // Remove AND to re-introduce jump bug
            if (jumpQueued && CanJump())
            {
                jumpQueued = false;

                hasJumpedSinceGrounded = true;

                // Grounded or airborne jump
                if (!isGrounded && !isWithinCoyoteTime)
                {
                    remainingAirJumps -= 1;
                    onJumpWhileAirborne?.Invoke();
                }
                else onJumpWhileGrounded?.Invoke();
                velocity.y = isGrounded ? jumpForceGrounded : jumpForceAirborne;

                PlayJumpSound();
            }
            else
            {
                // Clamp terminal velocity
                velocity.y = Mathf.Clamp(rbody.velocity.y, -terminalVelocity, terminalVelocity);
            }

            // Gravity
            if (isGrounded || rbody.velocity.y <= minVerticalVelocityForJump)
            {
                rbody.gravityScale = gravityScaleFalling;
            }
            else rbody.gravityScale = gravityScaleDefault;

            // Apply movement 
            //((Cla))just if it isn't knocked back
            if (_knockbackCount <= 0)
            {
                rbody.position += groundVelocity;
                rbody.velocity = velocity;
                
            }
            else
            {
                //((Cla))
                //the knockback need another calculation
                _knockbackCount -= Time.deltaTime;
                velocity.x = _knockbackDirection * _knockbackSpeed;
                velocity.y = _knockbackSpeed;
                rbody.velocity = velocity;
                rbody.position += (velocity*Time.deltaTime);
               // _knockbackSpeed -= _knockSmoothness;

            }

            // Persist state
            wasGrounded = isGrounded;

            //animation update
            _animator.SetBool("IsJumping", !isGrounded);

        }

        private void FixedUpdate()
        {
            int l = groundChecks.Length;
            isGrounded = false;
            Collider2D leftCollider = Physics2D.OverlapCircle(leftGroundCheck.position, groundCheckRadius, groundLayers);
            Collider2D rightCollider = Physics2D.OverlapCircle(rightGroundCheck.position, groundCheckRadius, groundLayers);
            bool rightFoot = rightCollider != null;
            bool leftFoot = leftCollider != null;
            isGrounded = (leftFoot || rightFoot);
            Transform oldGT = groundTransform;
            // Store ground transform
            if (!isGrounded) groundTransform = null;
            else if (rightFoot) groundTransform = rightCollider.transform;
            else if (leftFoot) groundTransform = leftCollider.transform;
            hasChangedGroundTransform = oldGT != groundTransform;
        }

        private bool CanJump()
        {
            if (isGrounded) return (rbody.velocity.y <= minVerticalVelocityForJump);

            bool canAirJump = remainingAirJumps > 0 && fairyDetector.CanFairyJump;
            return isWithinCoyoteTime || canAirJump;
        }

        //private void OnDrawGizmos()
        //{
        //    int l = groundChecks.Length;
        //        Gizmos.color = Color.yellow;
        //    for (int i = 0; i < l; i++)
        //    {
        //        Gizmos.DrawWireSphere(groundChecks[i].position, groundCheckRadius);
        //    }
        //}

        public void PlayStepSound()
        {
            movementAudioSource.pitch = UnityEngine.Random.Range(0.85f, 1.15f);
            movementAudioSource.PlayOneShot(stepAudioClip);
        }

        public void PlayJumpSound()
        {
            movementAudioSource.pitch = 1;
            movementAudioSource.PlayOneShot(jumpAudioClip);
        }
        /// <summary>
        /// ((CLA))
        /// knockback logic. 
        /// </summary>
        //public function for a knockback when damage is taken
        [Header("Knockback")]
        //how strong the knockback is at the beginning
        [SerializeField] private float _knockbackThrust = 1f;
        //how fast the knockback g
        private float _knockbackSpeed ;
        //the "smoothness" of the knockback
        private float _knockSmoothness = 0.125f;
        //how long it lasts
        [SerializeField] private float _knockbackTime = 5f ;

        public float KnockbackTime()
        {
            return _knockbackTime; 
        }
        private float _knockbackCount;
        //direction of the knockback: in the ninja we want left (-1) or right (1)
        private int _knockbackDirection = 1;

        public void Knockback(Vector3 obstaclePosition)
        {
            _knockbackDirection =   (obstaclePosition.x-rbody.position.x >= 0)? -1:1;
            _knockbackSpeed = _knockbackThrust;
            _knockbackCount = _knockbackTime;
            _knockSmoothness = _knockbackTime / _knockbackSpeed;
        }
    }
}