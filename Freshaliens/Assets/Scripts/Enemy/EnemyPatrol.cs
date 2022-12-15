using UnityEngine;
using System.Collections;
using Freshaliens.Player.Components;
using Unity.VisualScripting;


namespace Freshaliens.Enemy.Components
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyPatrol : MonoBehaviour
    {
        private enum State
        {
            MovingTowardsStartPosition,
            MovingTowardsEndPosition,
            Chasing,
        }

        private Rigidbody2D rbody = null;
        
        private PlayerMovementController playerMovementController;

        [Header("Path")] 
        [SerializeField] private Vector3 startPositionOS = Vector3.left, endPositionOS = Vector3.right;
        [SerializeField] private bool allowFloating = false;
        private Vector3 startPositionWS, endPositionWS;
        private Vector3 midPointInPath = Vector3.zero;
        private float pathChaseRadius = -1f;

        [Header("Movement")] 
        [SerializeField] private float movementSpeed = 3f;
        private State currentState = State.MovingTowardsEndPosition;
        
        [Header("Behaviors")]
        [SerializeField] private bool canChasePlayer = false;
        private bool canBeStunned = false;
        private EnemyStun stunComponent = null;
        
        private Vector3 currentTargetPosition = Vector3.zero;

        public Vector3 StartPosition
        {
            get => transform.TransformPoint(startPositionOS);
            set
            {
                Vector3 pos = value;
                if (!allowFloating) pos.y = transform.position.y;
                
                startPositionOS = transform.InverseTransformPoint(pos);
                startPositionWS = pos;
            }
        }

        public Vector3 EndPosition
        {
            get => transform.TransformPoint(endPositionOS);
            set
            {
                Vector3 pos = value;
                if (!allowFloating) pos.y = transform.position.y;
                
                endPositionOS = transform.InverseTransformPoint(pos);
                startPositionWS = pos;
            }
        }

        private void Start()
        {
            playerMovementController = PlayerMovementController.Instance;
            rbody = GetComponent<Rigidbody2D>();
            
            stunComponent = GetComponent<EnemyStun>();
            canBeStunned = stunComponent != null;

            currentState = State.MovingTowardsEndPosition;

            startPositionWS = transform.TransformPoint(startPositionOS);
            endPositionWS = transform.TransformPoint(endPositionOS);
            midPointInPath = (startPositionWS + endPositionWS) * 0.5f;
            pathChaseRadius = Vector3.Distance(startPositionWS, endPositionWS) * 0.5f;

            currentTargetPosition = endPositionWS;
            SetPosition(startPositionWS);
        }

        private void Update()
        {
            // Prevent movement when stunned
            if (canBeStunned && stunComponent.IsStunned)
            {
                rbody.velocity = Vector3.zero;
                return;
            }

            // Movement assessment
            Vector3 position = rbody.position;
            Vector3 directionFromMidPoint = (movementSpeed * Time.deltaTime) * (midPointInPath - position).normalized;
            float distanceFromMidPoint = Vector3.Distance(position - directionFromMidPoint, midPointInPath);
            bool playerInChaseRange = PlayerInChaseRange();


            if (canChasePlayer && playerInChaseRange)
            {
                currentState = State.Chasing;
            }

            // Calculate movement
            if (currentState == State.MovingTowardsStartPosition && distanceFromMidPoint >= pathChaseRadius)
            {
                currentTargetPosition = endPositionWS;
                currentState = State.MovingTowardsEndPosition;

            }
            else if (currentState == State.MovingTowardsEndPosition && distanceFromMidPoint >= pathChaseRadius)
            {
                currentTargetPosition = startPositionWS;
                currentState = State.MovingTowardsStartPosition;
            }
            if (currentState == State.Chasing)
            {
                Vector3 closest = playerMovementController.Position;

                // Should the enemy stop chasing?
                if (!playerInChaseRange)
                {
                    State newState = State.MovingTowardsStartPosition;
                    closest = startPositionWS;
                    if (Vector3.Distance(position, endPositionWS) <= pathChaseRadius)
                    {
                        closest = endPositionWS;
                        newState = State.MovingTowardsEndPosition;
                    }
                    currentState = newState;
                }

                currentTargetPosition = closest;
            }

            Vector2 direction = currentTargetPosition - position;

            Debug.DrawLine(position, currentTargetPosition);

            if (!allowFloating) direction.y = 0;

            Debug.DrawLine(position, position + new Vector3(direction.x, direction.y, 0));

            Vector2 velocity = direction.normalized * movementSpeed;
            rbody.velocity = velocity;
        }

        private void SetPosition(Vector3 position)
        {
            rbody.position = position;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private bool PlayerInChaseRange()
        {
            return Vector3.Distance(playerMovementController.Position, midPointInPath) <= pathChaseRadius;
        }
    }
}