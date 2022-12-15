using System.Collections;
using UnityEngine;

using Freshaliens.Interaction;
using Freshaliens.Player.Components;

namespace Freshaliens.Enemy.Components
{

    public class EnemyStun : Interactable
    {
        [SerializeField] private float stunTime = 1;
        [SerializeField] private bool isShoot = false;
        private EnemyPatrol enemyPatrol;
        private float remainingTime;
        private bool isStunned;

        public bool IsStunned => isStunned;

        private void Start()
        {
            if (isShoot)
            {
                enemyPatrol = GetComponent<EnemyPatrol>();
            }
        }

        public override void OnInteract()
        {
            FairyMovementController.Instance.SetLockOnTarget(transform);

            if (remainingTime > 0)
            {
                remainingTime = stunTime;
            }
            if (remainingTime <= 0)
            {
                remainingTime = stunTime;
                StartCoroutine(InteractCoroutine());
            }
        }

        IEnumerator InteractCoroutine()
        {
            isStunned = true;
            enemyPatrol.SetStunned(true);
            while (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                yield return null;
            }
            isStunned = false;
            enemyPatrol.SetStunned(false);

            yield return null;
        }

        public override void OnFairyExit()
        {
            remainingTime = 0;
        }
    }
}