using System;
using UnityEngine;
using Freshaliens.Management;

namespace Freshaliens.Player.Components
{
    /// <summary>
    /// Movment controller component for the fairy character
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(PlayerInputHandler))]
    public class FairyMovementController : SingletonMonobehaviour<FairyMovementController>, IMovementController
    {
        [Header("Player Controlled Movement")] [SerializeField]
        private float maxSpeed = 6f;

        [SerializeField] private float movementAcceleration = 5f;
        [SerializeField] private float movementDeceleration = 20f;

        [Header("External Control")]
        [SerializeField] private float maxOffScreenDistanceBeforeReturn = 1f;
        [SerializeField] private float returnOnScreenDistance = 2f;

        [SerializeField] private float returnAcceleration = 40f;
        [SerializeField] private float maxReturnSpeed = 10f;
        [SerializeField] private float verticalDistanceAtRespawn = 3f;
        [SerializeField] private float horizontalDistanceAtRespawn = 8f;

        [Header("World Interaction")] [SerializeField]
        private float stunDuration = 3f;

        [Header("Knockback")] [SerializeField]
        private float knockbackThrust = 1f; //how strong the knockback is at the beginning

        [SerializeField] private float knockbackDuration = 5f; //how long it lasts

        //[SerializeField, Range(0f, 45f)] private float knockbackAngle = 30f;
        private bool storedKnockback = false;
        private float knockbackSpeed; //how fast the knockback g
        private float knockSmoothness = 0.125f; //the "smoothness" of the knockback
        private float knockbackTimer = 0;
        private Vector2 knockbackDirection = Vector2.zero; //direction of the knockback

        private bool IsBeingKnockedBack
        {
            get => knockbackTimer > 0;
            set => knockbackTimer = value ? knockbackDuration : 0;
        }


        // State
        private Vector2 movementDirection = Vector2.zero;
        private Vector2 currentVelocity = Vector2.zero;
        private Transform lockOnTarget = null;
        private float stunTimer = 0;
        private bool isMoving = false;
        private bool isReturningToPlayer = false;
        private bool isLockingOnTarget = true;
        private bool lockOnTargetAssigned = false;
        
        private float _maxCameraSize;
        private float _minCameraSize;
        private float _cameraVerticalOffset;
        private float _cameraHorizontalOffset;

       

        // Components
        private PlayerInputHandler input = null;
        private Rigidbody2D rbody = null;
        private Transform ownTransform = null;

        // Respawn utility
        private CameraManager _camInstance;
        private Vector3 _fairyRespawnOffset;

        // Properties
        public bool IsMoving => isMoving;
        public bool IsReturning => isReturningToPlayer;
        public bool IsStunned => stunTimer > 0;
        public bool IsLockingOnTarget => isLockingOnTarget;

        //private void Awake()
        //{
        //    Instance = this;
        //}

        private void Start()
        {
            input = GetComponent<PlayerInputHandler>();
            rbody = GetComponent<Rigidbody2D>();
            ownTransform = transform;

            _camInstance = CameraManager.Instance;
            
            _maxCameraSize = _camInstance.getMaxCameraSize();
            _minCameraSize = _camInstance.getMinCameraSize();
            _cameraVerticalOffset = _camInstance.getVerticalOffset();
            _cameraHorizontalOffset = _camInstance.getHorizontalOffset();
            
            float respawnHorizontalOffset = _maxCameraSize * 16 / 9 + horizontalDistanceAtRespawn;
            float respawnVerticalOffset = _minCameraSize + verticalDistanceAtRespawn;
            _fairyRespawnOffset = new Vector3(respawnHorizontalOffset, respawnVerticalOffset, 0);
            
            

        }

        private void Update()
        {
            // TODO Ugly, fix later
            if (!LevelManager.Instance.IsPlaying)
            {
                rbody.velocity = Vector2.zero;
                return;
            }

            // Input
            movementDirection = new Vector2(input.GetHorizontal(), input.GetVertical());
            isMoving = movementDirection != Vector2.zero;
            if (isMoving)
            {
                PlaySound();
            }

            // Calculate acceleration based on whether the player wants to move or stop
            float acceleration = !isMoving ? movementDeceleration : movementAcceleration;
            float currentMaxSpeed = maxSpeed;

            // Should the fairy return towards the player?
            
            // float distanceFromPlayer = Vector2.Distance(ownTransform.position, PlayerMovementController.Instance.Position);
            // isReturningToPlayer = distanceFromPlayer >= maxDistanceBeforeReturn || (isReturningToPlayer && distanceFromPlayer > returnDistance);

            
            Vector3 cameraPosition = _camInstance.getCameraPosition();
            float distanceFromCameraX = Mathf.Abs(ownTransform.position.x - cameraPosition.x);
            float distanceFromCameraY = Mathf.Abs(ownTransform.position.y - cameraPosition.y);
            bool tooDistantX = distanceFromCameraX >= _maxCameraSize * 16 / 9 + maxOffScreenDistanceBeforeReturn;
            bool tooDistantY = distanceFromCameraY >= _maxCameraSize + maxOffScreenDistanceBeforeReturn;
            bool tooDistant = tooDistantX || tooDistantY;
            isReturningToPlayer = tooDistant || (isReturningToPlayer && 
                                  (distanceFromCameraX > _maxCameraSize*16/9-returnOnScreenDistance || distanceFromCameraY > _maxCameraSize-returnOnScreenDistance));

            // Is the fairy locking onto a target?
            bool wasLockingOnTarget = isLockingOnTarget;
            isLockingOnTarget = lockOnTargetAssigned &&
                                Vector2.Distance(ownTransform.position, lockOnTarget.position) > 0.05f;
            if (!isLockingOnTarget)
            {
                if (wasLockingOnTarget && lockOnTargetAssigned)
                {
                    rbody.position = lockOnTarget.position;
                    rbody.velocity = Vector2.zero;
                }

                lockOnTarget = null;
                lockOnTargetAssigned = false;
            }

            // "Free movement" exceptions
            if (isLockingOnTarget)
            {
                movementDirection = (lockOnTarget.position - ownTransform.position);
            }
            else if (isReturningToPlayer || isLockingOnTarget)
            {
                movementDirection = PlayerMovementController.Instance.Position - ownTransform.position;
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
            if (storedKnockback)
            {
                Debug.Log("knockback!!");
                IsBeingKnockedBack = true;
                storedKnockback = false;
                ApplyKnockbackToVelocity(ref currentVelocity, knockbackDirection);
                Debug.Log("movementDirection was "+ movementDirection);
                movementDirection = knockbackDirection;
                Debug.Log("movementDirection NOW IS "+ movementDirection);
            }


            Vector2 targetVelocity = movementDirection.normalized * maxSpeed;
            Vector2 deltaVelocity = targetVelocity - currentVelocity;
            float dampen = Mathf.Clamp01(acceleration * Time.deltaTime / currentMaxSpeed);

            // TODO Apply stored knockback
            rbody.velocity = currentVelocity + dampen * deltaVelocity;
        }

        /// <summary>
        /// Stun the fairy, preventing it from moving
        /// </summary>
        /// <param name="extraTime">Extra time to be added on top of the default stun time</param>
        public void Stun(float extraTime = 0)
        {
            // TODO Move the fairy away to avoid stun locking?
            stunTimer = stunDuration + extraTime;
        }

        public void PlaySound()
        {
            AudioManager1.instance.PlaySFX("fata1");
        }

        /// <summary>
        /// Lock the fairy onto a target
        /// </summary>
        public void SetLockOnTarget(Transform target)
        {
            lockOnTarget = target;
            lockOnTargetAssigned = true;
        }

        public void RespawnWithNinja(Vector3 ninjaPosition)
        {
            Vector3 newFairyPosition = RespawnPosition(ninjaPosition);
            rbody.position = newFairyPosition;
        }

        private Vector3 RespawnPosition(Vector3 ninjaPosition)
        {
            Vector3 currentFairyPosition = ownTransform.position;

            float xNinja = ninjaPosition.x;
            float yNinja = ninjaPosition.y;
            float xFairy = currentFairyPosition.x;
            float yFairy = currentFairyPosition.y;
            float newXfairy, newYfairy;

            // If the fairy is too right or too left reset horizontal position, else leave it in the same position 
            if ((xFairy - xNinja) > _fairyRespawnOffset.x)
            {
                // The fairy is too right
                newXfairy = xNinja + _fairyRespawnOffset.x + _cameraHorizontalOffset;
            }
            else if ((xNinja - xFairy) > _fairyRespawnOffset.x)
            {
                // The fairy is too left
                newXfairy = xNinja - _fairyRespawnOffset.x;
            }
            else
            {
                newXfairy = currentFairyPosition.x;
            }

            // If the fairy is too high or too low reset vertical position, else leave it in the same position 
            if ((yFairy - yNinja) > _fairyRespawnOffset.y)
            {
                // The fairy is too high
                newYfairy = yNinja + _fairyRespawnOffset.y + _cameraVerticalOffset;
            }
            else if ((yNinja - yFairy) > _fairyRespawnOffset.y)
            {
                // The fairy is too low
                newYfairy = yNinja - _fairyRespawnOffset.y;
            }
            else
            {
                newYfairy = currentFairyPosition.y;
            }

            return new Vector3(newXfairy, newYfairy, currentFairyPosition.z);
        }
        //((CLA)) fairy knockback

        /// <summary>
        /// ((CLA))
        /// knockback logic. 
        /// </summary>
        //public function for a knockback when damage is taken
   
        public float KnockbackTime()
        {
            return knockbackDuration;
        }

        public void Knockback(Vector3 obstaclePosition)
        {
            Debug.Log("obstacleposition"+ (Vector2)obstaclePosition);
            Debug.Log("rbody position"+ rbody.position);
            
            knockbackDirection = (Vector2)obstaclePosition - rbody.position;
            
            storedKnockback = true;
        }

        private void ApplyKnockbackToVelocity(ref Vector2 velocity, Vector2 direction)
        {
            float power = knockbackThrust;
            
            float angle = Vector2.Angle(Vector2.right, direction) * Mathf.Deg2Rad;
            
            // Calculate knockback
            // float angle = knockbackAngle * Mathf.Deg2Rad;
            

            Vector2 knockbackForce = new()
            {
                 x = Mathf.Cos(angle) * power,
                 y = Mathf.Sin(angle) * power,
                //x = power * direction,
                //y = 0,
            };
            velocity = knockbackForce;
        }
    }
}