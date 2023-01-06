using System;
using UnityEngine;

using Freshaliens.Management;

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
        private bool wasMoving = false;
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
        public event Action<float> onChangeDirection;
        public event Action<bool> onMovement;

        // Properties
        public Vector3 Position => ownTransform.position;
        public float LastFacedDirection => lastFacedDirection;
        public int RemainingAirJupms => remainingAirJumps;
        public Vector3 EnemyProjectileTarget => enemyProjectileTarget.position;

        [Header("Knockback")]
        [SerializeField] private float knockbackThrust = 1f; //how strong the knockback is at the beginning
        [SerializeField] private float knockbackDuration = 5f; //how long it lasts
        [SerializeField, Range(0f, 45f)] private float knockbackAngle = 30f;
        private bool storedKnockback = false;
        private float knockbackTimer = 0;
        private float knockbackDirection = 0; //direction of the knockback: in the ninja we want left (-1) or right (1)

        private bool IsBeingKnockedBack { get => knockbackTimer > 0; set => knockbackTimer = value ? knockbackDuration : 0; }


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
            float playingMultipler = 1;
            if (!LevelManager.Instance.IsPlaying)
            {
                playingMultipler = 0;
            }


            // Input

            float direction = input.GetHorizontal() * playingMultipler;
            
            if (direction != 0)
            {
                if (lastFacedDirection - direction < Mathf.Epsilon) onChangeDirection?.Invoke(direction);
                lastFacedDirection = direction;
            }
            //animation update
            // _animator.SetFloat("DirectionR", lastFacedDirection);

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
           // _animator.SetBool("IsMoving", isMoving);
            if (wasMoving != isMoving) onMovement?.Invoke(isMoving);
            if (isMoving)
            {
                // Accelerate until max speed
                currentSpeed = Mathf.Clamp(currentSpeed + walkAcceleration * Time.deltaTime, movementSpeedStart, movementSpeedMax);
                
            }
            else
            {
                currentSpeed = 0;
            }

            wasMoving = isMoving;
            // Ignore input direction if walljumping or if is being knocked back, apply it if not within the ignore input time frame
            if(!IsBeingKnockedBack)
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
                else
                {
                    onLand?.Invoke();
                }

                previousGroundPosition = groundTransform.position;
                lastGroundedTimestamp = Time.time;
            }
            else if (rbody.velocity.y < minVerticalVelocityForJump) hasJumpedSinceGrounded = false; // Not sure if this is the best fix for the double jump bug but here it is

            // Jumping
            isWithinCoyoteTime = Time.time - lastGroundedTimestamp <= coyoteTimeFrame && !hasJumpedSinceGrounded; // Remove AND to re-introduce jump bug
            if (jumpQueued && CanJump() && playingMultipler > 0)
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

            // Check for stored knockback
            if (storedKnockback)
            {
                IsBeingKnockedBack = true;
                storedKnockback = false;
                ApplyKnockbackToVelocity(ref velocity, knockbackDirection);
            }

            // Apply movement 
            rbody.position += groundVelocity;
            rbody.velocity = velocity;

            // Persist state
            wasGrounded = isGrounded;
            knockbackTimer -= Time.deltaTime;

            //animation update
           // _animator.SetBool("IsJumping", !isGrounded);

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
            if (IsBeingKnockedBack) return false;
            if (isGrounded) return (rbody.velocity.y <= minVerticalVelocityForJump);

            bool canAirJump = remainingAirJumps > 0 && fairyDetector.CanFairyJump;
            return isWithinCoyoteTime || canAirJump;
        }

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

        public float KnockbackTime()
        {
            return knockbackDuration;
        }


        /// <summary>
        //public function for a knockback when damage is taken
        /// </summary>
        public void Knockback(Vector3 obstaclePosition)
        {
            knockbackDirection = obstaclePosition.x >= rbody.position.x ? -1f : 1f;
            storedKnockback = true;
        }

        private void ApplyKnockbackToVelocity(ref Vector2 velocity, float direction) {

            // Calculate knockback
            float angle = knockbackAngle * Mathf.Deg2Rad;
            float power = knockbackThrust;

            Vector2 knockbackForce = new()
            {
                x = Mathf.Cos(angle) * power * direction,
                y = Mathf.Sin(angle) * power,
                //x = power * direction,
                //y = 0,
            };

            velocity.x = knockbackForce.x;
            velocity.y = knockbackForce.y;
        }
    }
}