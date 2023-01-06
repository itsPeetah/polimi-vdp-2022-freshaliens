using UnityEngine;
using Freshaliens.Player.Components;
namespace Freshaliens.Enemy.Components
{

    public class AIAttack : MonoBehaviour
    {

        [SerializeField] public float _attackRange;
        [SerializeField] public int _damage;
        [SerializeField] private float firePower = 10f;
        private bool stunned = false;
        private Quaternion _rotation;
        private Rigidbody2D _rb;

        [SerializeField, Tooltip("Projectile spawn point")] private Transform weaponMuzzle;

        private ProjectilePool projectiles;
        private AIPatrol enemyInt;
        [SerializeField] private string projectilePoolId;
        [SerializeField] private float fireInterval;
        
        float weaponAngleRadians = 0;
        float fireTimer = 0;

        private void Start()
        {   
            _rb = GetComponent<Rigidbody2D>();
            projectiles = ProjectilePool.GetByID(projectilePoolId);
            
        }

        // Update is called once per frame
        void Update()
        {
            if (stunned)
            {
                return;
            }
            Vector3 target = PlayerMovementController.Instance.EnemyProjectileTarget;
            float distToPlayer = Vector3.Distance(transform.position, target);
            float dx = transform.position.x - target.x;
            float dy = transform.position.y - target.y;
            weaponAngleRadians = Mathf.Atan2(dy, dx);
            if (distToPlayer < _attackRange)
            {
                //Debug.Log("in range");
                bool canFire = fireTimer <= 0;
                fireTimer -= Time.deltaTime;
                if (canFire)
                {
                   // Debug.Log("shoot");
                    fireTimer = fireInterval;
                    Shoot();
                }

            }

        }

        private void Shoot()
        {
            _rotation = Quaternion.Euler(0, 0, weaponAngleRadians * Mathf.Rad2Deg);
            _rb.velocity = new Vector2(0, 0);
            // float angle = Vector3.Angle(transform.position, _target.position);
            Vector3 pos = weaponMuzzle.position;
            Vector3 vel = new Vector3(-Mathf.Cos(weaponAngleRadians), -Mathf.Sin(weaponAngleRadians)) * firePower;
            projectiles.Spawn(pos, _rotation, vel);
            AudioManager1.instance.PlaySFX("laser");
        }
        
        public void SetStunned(bool stun)
        {
            stunned = stun;
        }
    }
}