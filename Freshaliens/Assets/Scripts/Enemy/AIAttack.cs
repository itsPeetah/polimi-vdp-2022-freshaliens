using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class AIAttack : MonoBehaviour
{
    
    [SerializeField] public float _attackRange;
    [SerializeField] public int _damage;
    [SerializeField] public Transform _target;
    [SerializeField] private float firePower = 10f;
    private Quaternion _rotation;
    [SerializeField] public Rigidbody2D _rb;
    
    [SerializeField, Tooltip("Projectile spawn point")] private Transform weaponMuzzle;
    
    private ProjectilePool projectiles;
    [SerializeField] private string projectilePoolId;
    [SerializeField] private float fireInterval;
    
    float weaponAngleRadians = 0;
    float fireTimer = 0;

    private void Start()
    {
      
        projectiles = ProjectilePool.GetByID(projectilePoolId);
    }

    // Update is called once per frame
    void Update()
    {
        float distToPlayer = Vector3.Distance(transform.position, _target.position);
        float dx = transform.position.x - _target.position.x;
        float dy = transform.position.y - _target.position.y;
        weaponAngleRadians = Mathf.Atan2(dy, dx);
        if (distToPlayer < _attackRange)
        {
            bool canFire = fireTimer <= 0;
            fireTimer -= Time.deltaTime;
            if (canFire)
            {
                
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
        Vector3 vel = new Vector3(-Mathf.Cos(weaponAngleRadians), -Mathf.Sin(weaponAngleRadians))* firePower;
        projectiles.Spawn(pos, _rotation, vel);
       
    }

}
