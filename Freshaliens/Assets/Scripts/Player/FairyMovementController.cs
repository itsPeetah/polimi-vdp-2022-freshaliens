using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Freshaliens.Player.Components
{
    /// <summary>
    /// Movment controller component for the fairy character
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(PlayerInputHandler))]
    public class FairyMovementController : MovementController
    {
        // Singleton instance
        private static FairyMovementController instance = null;
        public static FairyMovementController Instance { get => instance; private set => instance = value; }

        [Header("Player Controlled Movement")]
        [SerializeField] private float maxSpeed = 6f;
        [SerializeField] private float movementAcceleration = 5f;
        [SerializeField] private float movementDeceleration = 20f;

        [Header("External Control")]
        [SerializeField] private float maxDistanceBeforeReturn = 30f;
        [SerializeField] private float returnDistance = 15f;
        [SerializeField] private float returnAcceleration = 40f;
        [SerializeField] private float maxReturnSpeed = 10f;

        [Header("World Interaction")]
        [SerializeField] private float stunDuration = 3f;

        // State
        private Vector2 movementDirection = Vector2.zero;
        private Vector2 currentVelocity = Vector2.zero;
        private Transform lockOnTarget = null;
        private float stunTimer = 0;
        private bool isMoving = false;
        private bool isReturningToPlayer = false;
        private bool isLockingOnTarget = true;
        private bool lockOnTargetAssigned = false;

        // Components
        private PlayerInputHandler input = null;
        private Rigidbody2D rbody = null;
        private Transform ownTransform = null;

        // Properties
        public bool IsMoving => isMoving;
        public bool IsReturning => isReturningToPlayer;
        public bool IsStunned => stunTimer > 0;
        public bool IsLockingOnTarget => isLockingOnTarget;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            input = GetComponent<PlayerInputHandler>();
            rbody = GetComponent<Rigidbody2D>();
            ownTransform = transform;
        }

        private void Update()
        {
            // Input
            movementDirection = new Vector2(input.GetHorizontal(), input.GetVertical());
            isMoving = movementDirection != Vector2.zero;

            // Calculate acceleration based on whether the player wants to move or stop
            float acceleration = !isMoving ? movementDeceleration : movementAcceleration;
            float currentMaxSpeed = maxSpeed;

            // Should the fairy return towards the player?
            float distanceFromPlayer = Vector2.Distance(ownTransform.position, PlayerMovementController.Instance.Position);
            isReturningToPlayer = distanceFromPlayer >= maxDistanceBeforeReturn || (isReturningToPlayer && distanceFromPlayer > returnDistance);

            // Is the fairy locking onto a target?
            isLockingOnTarget = lockOnTargetAssigned && Vector2.Distance(ownTransform.position, lockOnTarget.position) > 0.01f;
            if (!isLockingOnTarget) {
                lockOnTarget = null;
                lockOnTargetAssigned = false;
            }

            // "Free movement" exceptions
            if (isReturningToPlayer || isLockingOnTarget)
            {
                movementDirection = isLockingOnTarget ? (lockOnTarget.position - ownTransform.position) : (PlayerMovementController.Instance.Position - ownTransform.position);
                isMoving = true;
                currentMaxSpeed = maxReturnSpeed;
                acceleration = returnAcceleration;
            }
            else if (stunTimer > 0)
            {
                movementDirection = Vector2.zero;
                isMoving = false;
                currentMaxSpeed = 0;
                acceleration = 0;
                stunTimer -= Time.deltaTime;
            }

            // Calculate change in velocity
            currentVelocity = rbody.velocity;
            Vector2 targetVelocity = movementDirection.normalized * maxSpeed;
            Vector2 deltaVelocity = targetVelocity - currentVelocity;
            float dampen = Mathf.Clamp01(acceleration * Time.deltaTime / currentMaxSpeed);
            rbody.velocity = currentVelocity + dampen * deltaVelocity;
        }

        /// <summary>
        /// Stun the fairy, preventing it from moving
        /// </summary>
        /// <param name="extraTime">Extra time to be added on top of the default stun time</param>
        public void Stun(float extraTime = 0) {
            // TODO Move the fairy away to avoid stun locking?
            stunTimer = stunDuration + extraTime;
        }

        /// <summary>
        /// Lock the fairy onto a target
        /// </summary>
        public void SetLockOnTarget(Transform target) {
            lockOnTarget = target;
            lockOnTargetAssigned = true;
        }
    }
}