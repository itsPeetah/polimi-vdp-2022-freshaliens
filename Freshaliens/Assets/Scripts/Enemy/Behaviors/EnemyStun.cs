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
        private AIAttack attacker;
        private float remainingTime;
        private bool isStunned;

        public bool IsStunned => isStunned;

        private void Start()
        {
            if (isShoot)
            {
                attacker = GetComponent<AIAttack>();
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
            if (isShoot)
            {
                attacker.SetStunned(true);
            }
            while (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                yield return null;
            }
            isStunned = false;
            if (isShoot)
            {
                attacker.SetStunned(false);
            }

            yield return null;
        }

        public override void OnFairyExit()
        {
            remainingTime = 0;
        }
    }
}