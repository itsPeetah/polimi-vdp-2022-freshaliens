using System.Collections;
using UnityEngine;

using Freshaliens.Interaction;
using Freshaliens.Player.Components;

namespace Freshaliens.Enemy.Components
{

    public class EnemyStun : Interactable
    {
        [SerializeField] private float stunTime = 1;
        private EnemyPatrol enemyPatrol;
        private float remainingTime;
        public bool IsStunned => remainingTime > 0;

        private void Start()
        {
            enemyPatrol = GetComponent<EnemyPatrol>();
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
            //enemyPatrol.SetStunned(true);
            while (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                yield return null;
            }
            //enemyPatrol.SetStunned(false);

            yield return null;
        }

        public override void OnFairyExit()
        {
            remainingTime = 0;
        }
    }
}