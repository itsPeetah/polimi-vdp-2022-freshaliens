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
            AtStartPosition,
            AtEndPosition,
            Moving,
            Chasing,
        }

        private Rigidbody2D rbody = null;
        
        private PlayerMovementController playerMovementController;

        [Header("Path")] 
        [SerializeField] private Vector3 startPositionOS = Vector3.left, endPositionOS = Vector3.right;
        private Vector3 startPositionWS, endPositionWS;
        [SerializeField] private bool allowFloating = false;
        
        [Header("Movement")] 
        [SerializeField] private float movementSpeed = 3f;
        [SerializeField] private AnimationCurve movementCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        private State currentState = State.AtStartPosition;
        
        [Header("Behaviors")]
        [SerializeField] private bool canChasePlayer = false;
        [SerializeField] private bool canBeStunned = false;
        private Stun stunComponent = null;
        
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
            
            stunComponent = GetComponent<Stun>();
            canBeStunned = stunComponent != null;

            currentState = State.AtStartPosition;
            startPositionWS = transform.TransformPoint(startPositionOS);
            endPositionWS = transform.TransformPoint(endPositionOS);
            SetPosition(startPositionWS);
        }

        private void Update()
        {
            // if (currentState != State.Moving) StartCoroutine(nameof(Move));
            if (canBeStunned && stunComponent.IsStunned)
            {
                rbody.velocity = Vector3.zero;
                return;
            }
            
            Vector3 position = rbody.position;
            if (canChasePlayer && PlayerIsNear(position))
            {
                currentState = State.Chasing;
            }
            
            // Se mi sto muovendo...
            // se stavo andando verso fine e sono abbastanza vicino (~0.01f) metti che sto andando a inzio
            // se stavo andando verso inizio e sono abbastanza vicino metti verso fine
            if (currentState == State.Moving)
            {
                if (Vector3.Distance(StartPosition, position) < 0.01f)
                {
                    currentState = State.AtStartPosition;
                }
                else if (Vector3.Distance(EndPosition, position) < 0.01f)
                {
                    currentState = State.AtEndPosition;
                }
            }
            else if (currentState == State.AtStartPosition) {
                currentTargetPosition = EndPosition;
                currentState = State.Moving;
                // towardsEndPosition = true
            }
            else if (currentState == State.AtEndPosition) { 
                currentTargetPosition = StartPosition;
                currentState = State.Moving;
                // towardsEndPosition = false
            }
            else if (currentState == State.Chasing)
            {
                currentTargetPosition = playerMovementController.Position;
                currentState = State.Chasing;
            }
            
            float xDirection = currentTargetPosition.x - position.x; 
            float yDirection = currentTargetPosition.y - position.y;
            
            // if (PlayerIsNear(position))
            // {
            //     currentState = State.Chasing;
            // }
            // else
            // {
            //     currentState = State.Moving;
            // }
            
            if (!allowFloating) yDirection = 0;
            
            Vector2 velocity = new Vector2(xDirection, yDirection).normalized * movementSpeed;
            rbody.velocity = velocity;
        }
        
        
        
        
        
        private IEnumerator Move()
        {
            State startingState = currentState;
            currentState = State.Moving;
            float distanceBetweenPoints = Vector3.Distance(StartPosition, EndPosition);
            float timeToTravel = distanceBetweenPoints / movementSpeed;
            float t = 0;
            float progress;
            SetPosition(GetPositionInPath(startingState == State.AtStartPosition ? 0 : 1));
            while (t <= timeToTravel)
            {
                if (canBeStunned && stunComponent.IsStunned)
                {
                    yield return null;
                    continue;
                }
                progress = startingState == State.AtStartPosition ? t / timeToTravel : 1 - t / timeToTravel;
                progress = movementCurve.Evaluate(progress);
                SetPosition(GetPositionInPath(progress));
                t += Time.deltaTime;
                yield return null;
            }

            SetPosition(GetPositionInPath(startingState == State.AtStartPosition ? 1 : 0));
            currentState = startingState == State.AtStartPosition ? State.AtEndPosition : State.AtStartPosition;
        }

        private Vector3 GetPositionInPath(float t)
        {
            if (t <= 0) return startPositionWS;
            if (t >= 1) return endPositionWS;

            return Vector3.Lerp(startPositionWS, endPositionWS, t);
        }

        private void SetPosition(Vector3 position)
        {
            rbody.position = position;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }

        private bool PlayerIsNear(Vector2 position)
        {
            Vector3 leftmost = StartPosition, rightmost = EndPosition;
            if (startPositionWS.x > endPositionWS.x)
            {
                (leftmost, rightmost) = (rightmost, leftmost);
            }

            if (!allowFloating)
            {
                return playerMovementController.Position.x > rightmost.x &&
                       playerMovementController.Position.x < leftmost.x;
            }

            Vector3 middlePoint = (startPositionWS + endPositionWS) * 0.5f;

           return Vector3.Distance(playerMovementController.Position, middlePoint) < (rightmost - leftmost).magnitude;
        }
    }
}