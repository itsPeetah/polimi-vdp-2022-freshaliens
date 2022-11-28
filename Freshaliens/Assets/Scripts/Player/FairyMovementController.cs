using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Freshaliens.Player.Components
{

    [RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(PlayerInputHandler))]
    public class FairyMovementController : MonoBehaviour
    {
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

        // State
        private Vector2 movementDirection = Vector2.zero;
        private Vector2 currentVelocity = Vector2.zero;
        private bool isMoving = false;
        private bool isReturningToPlayer = false;

        // Components
        private PlayerInputHandler input = null;
        private Rigidbody2D rbody = null;
        private Transform ownTransform = null;

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
            if (isReturningToPlayer)
            {
                movementDirection = (PlayerMovementController.Instance.Position - ownTransform.position);
                isMoving = true;
                currentMaxSpeed = maxReturnSpeed;
                acceleration = returnAcceleration;
            }

            // Calculate change in velocity
            currentVelocity = rbody.velocity;
            Vector2 targetVelocity = movementDirection.normalized * maxSpeed;
            Vector2 deltaVelocity = targetVelocity - currentVelocity;
            float dampen = Mathf.Clamp01(acceleration * Time.deltaTime / currentMaxSpeed);
            rbody.velocity = currentVelocity + dampen * deltaVelocity;
        }
    }
}