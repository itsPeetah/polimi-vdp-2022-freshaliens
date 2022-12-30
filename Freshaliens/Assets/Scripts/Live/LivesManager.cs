using System.Collections;
using UnityEngine;
using Freshaliens.Level.Components;


using Freshaliens.Player.Components;
using Freshaliens.Management;


public class LivesManager : MonoBehaviour
{
    [SerializeField] private LayerMask hitLayers = -1;
    [SerializeField] private int deathLayer = 16;
    [SerializeField] private Animator _animator;
    //renderer of the hitten player
    SpriteRenderer _sprite ;
    private float _damageAnimationTime ;
    private bool invincible = false;

    private void Start()
    {   
        _sprite = gameObject.GetComponent<SpriteRenderer>();
        invincible = false;
        _damageAnimationTime = gameObject.GetComponent<IMovementController>().KnockbackTime();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;
        if (( (1 << layer) & hitLayers) != 0) {
            ///((CLA)) we get our position and the position of the player for the animation
            Vector3 obstaclePosition = collision.gameObject.transform.position;
            //--------------------------------
      
                HitPlayer(false,obstaclePosition);
            
        }
    }

    public void HitPlayer(bool triggerRespawn , Vector3 obstaclePosition){
        if (invincible)
        {
            return;
        }
        LevelManager.Instance.DamagePlayer(skipInvulnerableCheck: triggerRespawn);
        
        invincible = true;
       
        
        StartCoroutine(FlashSprite());
     if (triggerRespawn && !LevelManager.Instance.GameOver)
     {
         LevelManager.Instance.RespawnPlayer();
     }
     else
     {
         StartCoroutine(hittenAnimation()); 
         gameObject.GetComponent<IMovementController>().Knockback(obstaclePosition);
     }
    }
    /// <summary>(((CLA)))
    /// To make add a flash of the sprite, during invincibility
    /// </summary>
    /// <param name="extraTime">Extra time to be added on top of the default stun time</param>
    IEnumerator FlashSprite()
    {
        
       // SpriteRenderer _sprite = gameObject.GetComponent<SpriteRenderer>();
 
        //Debug.Log("sprite"+ _sprite.name);
        for (int i = 0; i < 5; i++)
        {
            
        
            _sprite.enabled = false;
            yield return new WaitForSeconds(.25f);
            _sprite.enabled = true;
            yield return new WaitForSeconds(.25f);
           
           
        }

        invincible = false;
        Debug.Log("3invincible è "+ invincible);
            yield return null;
    }
    IEnumerator hittenAnimation()
    {
        Debug.Log("tempo animazione danno dura " + _damageAnimationTime);
        _animator.SetBool("isHitten", true);
        yield return new WaitForSeconds(_damageAnimationTime);
        _animator.SetBool("isHitten", false);
       
        yield return null;
    }
    
}
        

    
   

