using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerWeaponController : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] private Transform weaponPivot;
    [SerializeField, Tooltip("Projectile spawn point")] private Transform weaponMuzzle;

    [Header("Fire")]
    [SerializeField] private float fireInterval = 0.5f;
    [SerializeField] private string projectilePoolId = "Player";
    [SerializeField] private float firePower = 10f;


    // State
    float weaponAngleRadians = 0;
    float fireTimer = 0;

    // Components
    private PlayerInputHandler input;

    // References
    private ProjectilePool projectiles;

    private void Start()
    {
        input = GetComponent<PlayerInputHandler>();
        projectiles = ProjectilePool.GetByID(projectilePoolId);
    }

    private void Update()
    {
        // Rotate
        Vector2 screenPos = Camera.main.WorldToScreenPoint(weaponPivot.position);
        Vector2 aimPos = input.GetAimPosition();
        float dx = aimPos.x - screenPos.x;
        float dy = aimPos.y - screenPos.y;
        weaponAngleRadians = Mathf.Atan2(dy, dx);
        weaponPivot.localRotation = Quaternion.Euler(0, 0, weaponAngleRadians * Mathf.Rad2Deg);

        // Shoot
        bool wantsToFire = input.GetActionInput();
        bool canFire = fireTimer <= 0;
        fireTimer -= Time.deltaTime;
        if (wantsToFire && canFire) {
            Shoot();
            fireTimer = fireInterval;
        }
    }

    private void Shoot() {
        Vector3 pos = weaponMuzzle.position;
        Vector3 vel = new Vector3(Mathf.Cos(weaponAngleRadians), Mathf.Sin(weaponAngleRadians)) * firePower;
        projectiles.Spawn(pos, vel);
    }
}
