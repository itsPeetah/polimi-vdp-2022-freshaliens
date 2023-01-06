using System;
using System.Collections;
using System.Collections.Generic;
using Freshaliens.Enemy.Components;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemyAnimationControl : MonoBehaviour
{
   // [SerializeField] private Animator animator; 
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] [ Range(0.20f, 0.90f)] private float fireInterval;
    private bool directionL = true;
    
    // Start is called before the first frame update
    void Start()
    {
      
        // gameObject.GetComponent<EnemyPatrol>().onFlipDirection += () =>
        // {
        //     Sprite.color = (DirectionL) ? Color.green : Color.yellow;
        //     DirectionL = !DirectionL;
        // };
        gameObject.GetComponent<EnemyDestroy>().OnDestroyEnemy += (dyingEnemy) =>
        {
            if (dyingEnemy == gameObject)
            {
                StartCoroutine(KillMyself());
            }
        };
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
        yield return new WaitForSeconds(0.25f);
        gameObject.SetActive(false);
        yield return null;
    }

    public void EnemyFlip()
    {
        FaceAnimation();
            
            directionL = !directionL;
        
    }

    private void FaceAnimation()
    {
        sprite.color = (directionL) ? Color.green : Color.yellow;
    }
    public void HasShoot( )
    {
        StartCoroutine(FireAnim());
    }

    IEnumerator FireAnim( )
    {
        var color = sprite.color;
        sprite.color = Color.black;
        yield return new WaitForSeconds(fireInterval);
       FaceAnimation();


    }
    // Update is called once per frame
 
}
