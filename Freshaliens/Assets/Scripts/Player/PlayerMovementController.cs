using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Freshaliens.Player.Components
{

    [RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(PlayerInputHandler)), RequireComponent(typeof(PlayerFairyDetector))]
    public class PlayerMovementController : MovementController
    {
        private static PlayerMovementController instance = null;
        public static PlayerMovementController Instance { get => instance; private set => instance = value; }

        private const float minVerticalVelocityForJump = 0.001f;

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
        [SerializeField] private float groundCheckRadius = 0.1f;
        [SerializeField] private LayerMask groundLayers = 0;

        // State
        private bool isMoving = false;
        private bool isGrounded = false;
        private bool wasGrounded = false;
        private bool isWithinCoyoteTime = false;
        private bool jumpQueued = false;
        private int remainingAirJumps = 0;
        private float currentSpeed = 0f;
        private float jumpPressedTimestamp = 0f;    // time of last jump press (for buffering)
        private float lastGroundedTimestamp = 0f;   // time player was last grounded (for coyote time)
        private float wallJumpTimestamp = 0f;   // time of last wall jump (to disable airborne control)
        private Vector2 velocity = Vector2.zero;

        // Components
        private PlayerInputHandler input = null;
        private Rigidbody2D rbody = null;
        private Transform ownTransform = null;
        private PlayerFairyDetector fairyDetector = null;

        // Properties
        public Vector3 Position => ownTransform.position;

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
        }

        private void Update()
        {
            // Input

            float direction = input.GetHorizontal();
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
            if (Time.time - wallJumpTimestamp > wallJumpIgnoreInputFrame)
                velocity.x = direction * currentSpeed;

            if (isGrounded) remainingAirJumps = maxAirJumps;
            else if (wasGrounded)
            {
                lastGroundedTimestamp = Time.time;
            }

            // Jumping
            isWithinCoyoteTime = Time.time - lastGroundedTimestamp <= coyoteTimeFrame;
            if (jumpQueued && CanJump())
            {
                jumpQueued = false;

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
            if (isGrounded || rbody.velocity.y <= minVerticalVelocityForJump) rbody.gravityScale = gravityScaleFalling;
            else rbody.gravityScale = gravityScaleDefault;

            // Apply movement
            rbody.velocity = velocity;

            // Persist state
            wasGrounded = isGrounded;
        }

        private void FixedUpdate()
        {
            int l = groundChecks.Length;
            isGrounded = false;
            // TODO Change to two separate objects and unroll loop
            for (int i = 0; i < l; i++)
            {
                isGrounded |= Physics2D.OverlapCircle(groundChecks[i].position, groundCheckRadius, groundLayers) != null;
            }
        }

        private bool CanJump()
        {
            if (isGrounded) return rbody.velocity.y <= minVerticalVelocityForJump;

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