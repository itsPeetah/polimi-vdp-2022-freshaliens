using UnityEngine;
namespace Freshaliens.Player.Components
{
    
    [RequireComponent(typeof(PlayerInputHandler)),RequireComponent(typeof(PlayerMovementController))]
    public class PlayerWeaponController : MonoBehaviour
    {
        [Header("Weapon")]
        [SerializeField, Tooltip("Projectile spawn point")] private Transform weaponMuzzle;
        
        [Header("Fire")]
        [SerializeField] private float fireInterval = 0.5f;
        [SerializeField] private string projectilePoolId = "Player";
        [SerializeField] private float firePower = 10f;

        // State
        float fireTimer = 0;

        private Vector3 muzzleOffset;
        // Components
        private PlayerInputHandler input;

        // References
        private ProjectilePool projectiles;
        private PlayerMovementController playerMovementController;

        private void Start()
        {
            input = GetComponent<PlayerInputHandler>();
            projectiles = ProjectilePool.GetByID(projectilePoolId);
            playerMovementController = GetComponent<PlayerMovementController>();
            muzzleOffset = weaponMuzzle.position - transform.position;

        }

        private void Update()
        {

            Vector3 actualOffset = new Vector3(muzzleOffset.x * playerMovementController.LastFacedDirection,
                muzzleOffset.y, muzzleOffset.z);
            weaponMuzzle.localPosition = actualOffset;
            // Shoot
            bool wantsToFire = input.GetActionInput();
            bool canFire = fireTimer <= 0;
            fireTimer -= Time.deltaTime;
            if (wantsToFire && canFire)
            {
                Shoot();
                fireTimer = fireInterval;
            }
        }

        private void Shoot()
        {
            
            Vector3 pos = weaponMuzzle.position;
            
            Vector3 vel = Vector3.right * (playerMovementController.LastFacedDirection * firePower);
            
            projectiles.Spawn(pos, vel);
        }
    }
}