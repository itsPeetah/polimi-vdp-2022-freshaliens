using System;
using System.Collections;
using System.Collections.Generic;
using Freshaliens.Management;
using Freshaliens.Player.Components;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField, Range(0f, 1f)] private float flashingInterval;
    private float damageAnimationTime = 0;

    private SpriteRenderer playerSprite;
    private float invulnerabilityTime;
    //     public event Action<LevelPhase> onLevelPhaseChange;
//     public event Action<bool> onPauseToggle;
//     public event Action onGameLost;
 //    public event Action<bool> onJump;
//     public event Action onPlayerDamageTaken;
//     public event Action onGameWon;
    
    private void Start()
    {
      
        playerSprite = gameObject.GetComponent<SpriteRenderer>();
        damageAnimationTime = gameObject.GetComponent<IMovementController>().KnockbackTime();
        invulnerabilityTime = LevelManager.Instance.InvulnerabilityDuration;
        PlayerMovementController.Instance.onJumpWhileGrounded += () => { animator.SetBool("IsJumping", true); };
        PlayerMovementController.Instance.onLand += () => { animator.SetBool("IsJumping", false); };
        PlayerMovementController.Instance.onChangeDirection += (direction) => { animator.SetFloat("DirectionR", direction); };
        PlayerMovementController.Instance.onMovement += (isMoving) => { animator.SetBool("IsMoving", isMoving); };
        LevelManager.Instance.onPlayerDamageTaken += (playerDamaged) =>
        {
            if (playerDamaged == gameObject)
                StartCoroutine(HitAnimation());
        };
        LevelManager.Instance.onPlayerDamageTaken += (playerDamaged) =>
        {
            if (playerDamaged == gameObject)
            StartCoroutine(FlashColorSprite());
        };
    }  
    
    IEnumerator HitAnimation()
    {
       // Debug.Log("tempo animazione danno dura " + _damageAnimationTime);
        animator.SetBool("isHit", true);
        yield return new WaitForSeconds(damageAnimationTime);
        animator.SetBool("isHit", false);
       
        yield return null;
    }
    IEnumerator FlashColorSprite()
    {
        float numberOfIntervals = (invulnerabilityTime / flashingInterval) / 2;
        // SpriteRenderer _sprite = gameObject.GetComponent<SpriteRenderer>();
 
        Debug.Log("sprite"+ playerSprite);
        
        for (int i = 0; i < numberOfIntervals; i++)
        {
            playerSprite.color = Color.red;
      
         
            yield return new WaitForSeconds(flashingInterval);
            playerSprite.color = Color.white;

        
            yield return new WaitForSeconds(flashingInterval);
           
           
        }

     
      
        yield return null;
    }
}

