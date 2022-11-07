using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    private ProjectilePool ownerPool;
    public ProjectilePool OwnerPool { set => ownerPool = value; }

    [Header("Projectile Stats")]
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private LayerMask hitLayers = -1;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rbody = null;


    public void Fire(Vector2 velocity) {
        StopAllCoroutines();
        StartCoroutine(Decay());
        rbody.velocity = velocity;
    }

    private IEnumerator Decay() {
        yield return new WaitForSeconds(lifeTime);
        ownerPool.Reclaim(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;
        // If object is on a hittable layer
        if ((layer & hitLayers) == layer) {
            ownerPool.Reclaim(this);
            Debug.Log("HIT!");
            // TODO ADD DAMAGING LOGIC
        }
    }
}
