using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Freshaliens.Interaction;
using Freshaliens.Player.Components;

namespace Freshaliens.Enemy.Components
{

    public class Stun : Interactable
    {
        [SerializeField] private float stunTime = 1;
        [SerializeField] private bool isShoot= false;
        private AIAttack attacker;
        private AIPatrol enemyInt;
        private float remainingTime;
        private bool isStunned;

        public bool IsStunned => isStunned;

        private void Start()
        {
            enemyInt = GetComponent<AIPatrol>();
            if (isShoot)
            {
                attacker = GetComponent<AIAttack>();
            }
        }

        public override void OnInteract()
        {
            FairyMovementController.Instance.SetLockOnTarget(transform);

            //StopCoroutine(InteractCoroutine());
            //StartCoroutine(InteractCoroutine());
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
            // enemyInt.setStun(true);
            isStunned = true;
            if (isShoot)
            {
                attacker.setStun(true);
            }
            while (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                yield return null;
            }
            // yield return new WaitForSeconds(stunTime);
            // enemyInt.setStun(false);
            isStunned = false;
            if (isShoot)
            {
                attacker.setStun(false);
            }
            
            yield return null;
        }
        
        public override void OnFairyExit()
        {
            remainingTime = 0;
        }
    }
}