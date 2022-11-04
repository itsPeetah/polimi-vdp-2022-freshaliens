using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerWeaponController : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private Transform weaponMuzzle;

    [Header("Fire")]
    [SerializeField] private float fireInterval = 0.5f;
    

    // State
    float weaponAngle = 0;
    float fireTimer = 0;

    // Components
    private PlayerInputHandler input;


    private void Start()
    {
        input = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        // Rotate
        Vector2 screenPos = Camera.main.WorldToScreenPoint(weaponPivot.position);
        Vector2 aimPos = input.GetAimPosition();
        float dx = aimPos.x - screenPos.x;
        float dy = aimPos.y - screenPos.y;
        weaponAngle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        weaponPivot.localRotation = Quaternion.Euler(0, 0, weaponAngle);

        // Shoot
        bool wantsToFire = input.GetFireInput();
        bool canFire = fireTimer <= 0;
        fireTimer -= Time.deltaTime;
        if (wantsToFire && canFire) {
            Shoot();
            fireTimer = fireInterval;
        }
    }

    private void Shoot() {
        Debug.Log("Shooting");
    }
}
