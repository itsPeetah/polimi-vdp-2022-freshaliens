using System.Collections;
using UnityEngine;
namespace Freshaliens.Player.Components
{
    
    [RequireComponent(typeof(PlayerInputHandler)),RequireComponent(typeof(PlayerMovementController))]
    public class PlayerWeaponController : MonoBehaviour
    {
        [Header("Weapon")]
        [SerializeField, Tooltip("Projectile spawn point")] private Transform weaponMuzzle;
        [SerializeField] private SpriteRenderer weaponMuzzleSprite;
        
        [Header("Fire")]
        [SerializeField] private float fireInterval = 0.5f;
        [SerializeField] private string projectilePoolId = "Player";
        [SerializeField] private float firePower = 10f;

        // State
        float fireTimer = 0;
        private Vector3 muzzleOffset;

        private bool _isJumping = false;
        
        
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
            
            PlayerMovementController.Instance.onJumpWhileGrounded += () => { _isJumping = true; };
            PlayerMovementController.Instance.onLand += () => { _isJumping = false; };

        }

        private void Update()
        {

            weaponMuzzleSprite.flipX = playerMovementController.LastFacedDirection < 0;

            // Vector3 actualOffset = new Vector3(muzzleOffset.x * playerMovementController.LastFacedDirection,
                // muzzleOffset.y, muzzleOffset.z);
            
            Vector3 actualOffset = _isJumping ? new Vector3((muzzleOffset.x + 0.07f)  * playerMovementController.LastFacedDirection,
                muzzleOffset.y + 0.25f, muzzleOffset.z) : new Vector3(muzzleOffset.x * playerMovementController.LastFacedDirection,
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
            weaponMuzzleSprite.enabled = true;

            Vector3 pos = weaponMuzzle.position + new Vector3(playerMovementController.LastFacedDirection*0.3f,0,0);
            
            Vector3 vel = Vector3.right * (playerMovementController.LastFacedDirection * firePower);
            
            projectiles.Spawn(pos, vel);
            AudioManager1.instance.PlaySFX("shot");
            
            StopAllCoroutines();
            StartCoroutine(OnShoot());

        }


        private IEnumerator OnShoot()
        {
            yield return new WaitForSeconds(2);
            weaponMuzzleSprite.enabled = false;
        }
    }
}