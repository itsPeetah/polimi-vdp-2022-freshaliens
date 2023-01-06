using System.Collections;
using System.Collections.Generic;
using Freshaliens.Management;
using Freshaliens.Player.Components;
using UnityEngine;

public class FairyAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private float damageAnimationTime = 0;
    [SerializeField, Range(0.1f, 1f)] private float flashingInterval;
    private float invulnerabilityTime;
    private SpriteRenderer playerSprite;
    private bool animationHit = false;
    private void Start()
    {
        playerSprite = gameObject.GetComponent<SpriteRenderer>();
        invulnerabilityTime = LevelManager.Instance.InvulnerabilityDuration;
        //damageAnimationTime = gameObject.GetComponent<IMovementController>().KnockbackTime();
        gameObject.GetComponent<FairyInteractionController>().onInteract += (lightsOn) => { ChangeLights(lightsOn); };
        //animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        // LevelManager.Instance.onPlayerDamageTaken += (playerDamaged) =>
        // {
        //     if (playerDamaged == gameObject)
        //       
        //     //Debug.Log("animation corrente = "+ animator.GetCurrentAnimatorClipInfo(0)[0]);
        //    
        // };
        LevelManager.Instance.onPlayerDamageTaken += (playerDamaged) =>
        {
            if (playerDamaged == gameObject){
                animator.SetBool("IsHit", true);
                //if(animationHit)
               // damageAnimationTime = animator.GetCurrentAnimatorStateInfo(0).length;
                animationHit = true;
                
                StartCoroutine(StopHitAnimation());
                }
            };
        LevelManager.Instance.onPlayerDamageTaken += (playerDamaged) =>
        {
            if (playerDamaged == gameObject)
                StartCoroutine(FlashColorSprite());
        };
    }

    private void ChangeLights(bool lightsOn)
    {
        animator.SetBool("canLight", lightsOn);
    }


    IEnumerator StopHitAnimation()
    {
        yield return null;
        damageAnimationTime = animator.GetCurrentAnimatorStateInfo(0).length;
        Debug.Log("animation time is "+ damageAnimationTime);
        yield return new WaitForSeconds(damageAnimationTime);
        animator.SetBool("IsHit", false);
        animationHit = false;
       
        yield return null;
    }
    IEnumerator FlashColorSprite()
    {
        float numberOfIntervals = (invulnerabilityTime / flashingInterval) / 2;
        // SpriteRenderer _sprite = gameObject.GetComponent<SpriteRenderer>();



        for (int i = 0; i < numberOfIntervals; i++)
        {
            playerSprite.color = Color.red;


            yield return new WaitForSeconds(flashingInterval);
            playerSprite.color = Color.white;


            yield return new WaitForSeconds(flashingInterval);


        }
    }

}