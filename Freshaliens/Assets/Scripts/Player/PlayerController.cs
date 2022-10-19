using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private const float minVerticalVelocityForJump = 0.001f;

    // TODO Move input handling to its own class
    [Header("Input")]
    [SerializeField] private string horizontalAxis = "Horizontal";
    [SerializeField] private string jumpAxis = "Jump";

    [Header("Stats")]
    [SerializeField] private float movementSpeedStart = 5.0f;
    [SerializeField] private float movementSpeedMax = 6.0f;
    [SerializeField] private float walkAcceleration = 1.0f;

    [Header("Jumping")]
    [SerializeField] private float jumpForceGrounded = 10.0f;
    [SerializeField] private int maxAirJumps = 1;
    [SerializeField] private float jumpQueueFrame = 0.15f;
    [SerializeField, Range(0, 80)] private float wallJumpAngle = 45f;
    [SerializeField] private bool canWallJump = true;
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
    [SerializeField] private float groundCheckRadius = 0.1f; // TODO Rename to something like collisionCheckRadius??
    [SerializeField] private LayerMask groundLayers = 0;

    [Header("Wall check")] // Uses groundlayers and groundcheckradius
    [SerializeField] private bool canClimb = true;
    [SerializeField] private Transform wallCheckRight = null;
    [SerializeField] private Transform wallCheckLeft = null;

    // State
    private bool isMoving = false;
    private bool isGrounded = false;
    private bool wasGrounded = false;
    private bool isWithinCoyoteTime = false;
    private bool isClimbing = false;
    private bool facingWallRight = false;
    private bool facingWallLeft = false;
    private bool jumpQueued = false;
    private int remainingAirJumps = 0;
    private float currentSpeed = 0f;
    private float jumpPressedTimestamp = 0f;
    private float lastGroundedTimestamp = 0f;
    private float wallJumpTimestamp = 0f;
    private Vector2 velocity = Vector2.zero;

    // Components
    private Rigidbody2D rbody = null;

    private void Start()
    {
        rbody = GetComponent<Rigidbody2D>();

        remainingAirJumps = maxAirJumps;
    }

    private void Update()
    {
        // Input

        float direction = Input.GetAxisRaw(horizontalAxis);
        if (Input.GetButtonDown(jumpAxis))
        {
            jumpPressedTimestamp = Time.time;
            jumpQueued = true;
        }
        // Handle jump queueing
        if (Time.time  - jumpPressedTimestamp > jumpQueueFrame)
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
        else if (wasGrounded) {
            lastGroundedTimestamp = Time.time;
        }

        // Climbing
        bool pushingWallRight = facingWallRight && direction > 0;
        bool pushingWallLeft = facingWallLeft && direction < 0;
        isClimbing = canClimb && (pushingWallRight || pushingWallLeft);


        // Jumping
        isWithinCoyoteTime = Time.time - lastGroundedTimestamp <= coyoteTimeFrame;
        if (jumpQueued && CanJump())
        {
            jumpQueued = false;

            if (isClimbing) {
                // Wall jump
                isClimbing = false;
                wallJumpTimestamp = Time.time;
                velocity.x = - direction * jumpForceGrounded * Mathf.Cos(wallJumpAngle);
                velocity.y = jumpForceGrounded * Mathf.Sin(wallJumpAngle);
            } else {
                // Grounded or airborne jump
                if (!isGrounded && !isWithinCoyoteTime) remainingAirJumps -= 1;
                velocity.y = jumpForceGrounded;
            }
        }
        else
        {
            velocity.y = Mathf.Clamp(rbody.velocity.y, -terminalVelocity, terminalVelocity);
        }

        // Gravity
        if (isClimbing && rbody.velocity.y <= minVerticalVelocityForJump) rbody.gravityScale = gravityScaleClimbing;
        else if (isGrounded || rbody.velocity.y <= minVerticalVelocityForJump) rbody.gravityScale = gravityScaleFalling;
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
        for (int i = 0; i < l; i++)
        {
            isGrounded |= Physics2D.OverlapCircle(groundChecks[i].position, groundCheckRadius, groundLayers) != null;
        }

        facingWallRight = Physics2D.OverlapCircle(wallCheckRight.position, groundCheckRadius, groundLayers) != null;
        facingWallLeft = Physics2D.OverlapCircle(wallCheckLeft.position, groundCheckRadius, groundLayers) != null;
    }

    private void OnDrawGizmos()
    {
        int l = groundChecks.Length;
            Gizmos.color = Color.yellow;
        for (int i = 0; i < l; i++)
        {
            Gizmos.DrawWireSphere(groundChecks[i].position, groundCheckRadius);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(wallCheckRight.position, groundCheckRadius);
        Gizmos.DrawWireSphere(wallCheckLeft.position, groundCheckRadius);

    }

    private bool CanJump() {
        if (isGrounded) return rbody.velocity.y <= minVerticalVelocityForJump;
        else return (isClimbing && canWallJump) || isWithinCoyoteTime || remainingAirJumps > 0;
    }
}
