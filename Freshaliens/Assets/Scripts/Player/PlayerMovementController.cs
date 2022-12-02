using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Freshaliens.Player.Components
{
    /// <summary>
    /// Component that handles player movement 
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(PlayerInputHandler)), RequireComponent(typeof(PlayerFairyDetector))]
    public class PlayerMovementController : MovementController
    {
        private static PlayerMovementController instance = null;
        public static PlayerMovementController Instance { get => instance; private set => instance = value; }

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

        // State
        private bool isMoving = false;
        private bool isGrounded = false;
        private bool wasGrounded = false;
        private bool isWithinCoyoteTime = false;
        private bool jumpQueued = false;
        private bool hasJumpedSinceGrounded = false;    // this might use a better name
        private bool hasChangedGroundTransform = true;
        private int remainingAirJumps = 0;
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

        // Properties
        public Vector3 Position => ownTransform.position;
        public float LastFacedDirection => lastFacedDirection;

        private void Awake()
        {
            Instance = this;
        }

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
            // Input

            float direction = input.GetHorizontal();
            if (direction != 0) lastFacedDirection = direction;
            //animation update
            _animator.SetFloat("DirectionR",lastFacedDirection);
            
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
            _animator.SetBool("IsMoving",isMoving);
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
                if (wasGrounded && !hasChangedGroundTransform /*Not the best fix for the trapdoor glitch but OK for now*/)
                {
                    groundVelocity = groundTransform.position - previousGroundPosition;
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
                if (!isGrounded && !isWithinCoyoteTime) remainingAirJumps -= 1;
                velocity.y = isGrounded ? jumpForceGrounded : jumpForceAirborne;

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
            rbody.position += groundVelocity;
            rbody.velocity = velocity;

            // Persist state
            wasGrounded = isGrounded;
            
            //animation update
            _animator.SetBool("IsJumping",!isGrounded);

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
            if (rightFoot) groundTransform = rightCollider.transform;   
            else if (leftFoot && lastFacedDirection < 0) groundTransform = leftCollider.transform;
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
    }
}