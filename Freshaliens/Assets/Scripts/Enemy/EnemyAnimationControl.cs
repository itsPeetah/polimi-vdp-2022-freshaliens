using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationControl : MonoBehaviour
{
    [SerializeField] private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
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
        float animationTime;
        animator.enabled = true;
        animator.SetBool(("IsDead"),true);
        yield return null;
        animationTime = animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationTime);
        gameObject.SetActive(false);
        yield return null;
    }
    // Update is called once per frame
 
}
