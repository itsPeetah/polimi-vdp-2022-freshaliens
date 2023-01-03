using System.Collections;
using System.Collections.Generic;
using Freshaliens.Enemy.Components;
using UnityEngine;

public class EnemyAnimationControl : MonoBehaviour
{
   // [SerializeField] private Animator animator; 
    [SerializeField] private SpriteRenderer Sprite;

    private bool DirectionL = true;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<EnemyPatrol>().onFlipDirection += () =>
        {
            Sprite.color = (DirectionL) ? Color.green : Color.yellow;
            DirectionL = !DirectionL;
        };
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
        Sprite.color = Color.red;
        yield return new WaitForSeconds(0.25f);
        gameObject.SetActive(false);
        yield return null;
    }
    // Update is called once per frame
 
}
