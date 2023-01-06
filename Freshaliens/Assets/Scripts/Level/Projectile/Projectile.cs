using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    private ProjectilePool ownerPool;
    public ProjectilePool OwnerPool { set => ownerPool = value; }

    [Header("Projectile Stats")]
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


        /*
         *      lm: 0001100110
         *      gl: 0000100000 (5)
         *       &: 0000100000
         *       
         *      lm: 0001010100
         *      gl: 0000100000 (5)
         *       &: 0000000000
         */

        if (( (1 << layer) & hitLayers) != 0) {
            ownerPool.Reclaim(this);
            Debug.Log($"Hit! {collision.gameObject.name}");

            ProjectileTarget target = collision.GetComponent<ProjectileTarget>();
           //Debug.Log("vediamo se target è null : "+ (target == null));
           //((CLA)) there is no component ProjectileTarget in Ninja, neither in Fairy. target will be always null 
           if (target != null) {
                target.Hit();
                
            }
        }
    }
}
