using System;
using System.Collections;
using System.Collections.Generic;
using Freshaliens.Enemy.Components;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemyAnimationControl : MonoBehaviour
{
    [SerializeField] private Animator animator; 
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] [ Range(0.20f, 0.90f)] private float fireInterval;
    [SerializeField] private bool firstDirection = false;
    private Rigidbody2D rbody;
    private bool directionR;
    // Start is called before the first frame update
    void Start()
    {
        rbody = gameObject.GetComponent<Rigidbody2D>();
        directionR = firstDirection;
        // gameObject.GetComponent<EnemyPatrol>().onFlipDirection += () =>
        // {
        //     Sprite.color = (DirectionL) ? Color.green : Color.yellow;
        //     DirectionL = !DirectionL;
        // };
        gameObject.GetComponent<EnemyDestroy>().OnDamageEnemy += () => { StartCoroutine(AnimateDamage()); };
        gameObject.GetComponent<EnemyDestroy>().OnDestroyEnemy += (dyingEnemy) =>
        {
            if (dyingEnemy == gameObject)
            {
                StartCoroutine(KillMyself());
            }
        };
    }

    IEnumerator AnimateDamage()
    {
        animator.SetBool("isHit",true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isHit",false);
        yield return null;
    }
    IEnumerator KillMyself()
    {
         
        /// CODE FOR ACTUAL KILLING
        // float animationTime;
        // animator.SetBool(("IsDead"),true);
        // yield return null;
        // animationTime = animator.GetCurrentAnimatorStateInfo(0).length;
        // yield return new WaitForSeconds(animationTime);
        // gameObject.SetActive(false);
        ///test animation death
        sprite.color = Color.red;
        rbody.constraints = RigidbodyConstraints2D.FreezeAll;
        animator.SetBool("isDying",true);
       yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
        yield return null;
    }

    public void EnemyFlip()
    {
        FaceAnimation();
            
            directionR = !directionR;
        
    }

    private void FaceAnimation()
    {
        sprite.flipX = directionR;
       //sprite.color = (directionR) ? Color.green : Color.yellow;
    }
    public void HasShoot(float playerX, float myPositionX )
    {
       // StartCoroutine(FireAnim(playerX, myPositionX));
    }

    IEnumerator FireAnim(float playerX, float myPositionX )
    {
        //if enemy is on the left of the player, and directionR is true, flips Sprite.
        // if ((myPositionX - playerX) > 0)
        // {
        //     if (directionR) sprite.flipX = !directionR;
        // }
        // else
        // {
        //     //enemy is not on the left of the player, so if directionR is false , flips Sprite
        //     if (!directionR) sprite.flipX = !directionR;
        // }
        // yield return new WaitForSeconds(fireInterval);
       // FaceAnimation();

       yield return null;   
    }
    // Update is called once per frame
 
}
