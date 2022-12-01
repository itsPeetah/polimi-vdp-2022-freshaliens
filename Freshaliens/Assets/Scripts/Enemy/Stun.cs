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
        private AIPatrol enemyInt;
        private float remainingTime;

        private void Start()
        {
            enemyInt = GetComponent<AIPatrol>();
        }

        public override void OnInteract()
        {
            FairyMovementController.Instance.SetLockOnTarget(transform);

            //StopCoroutine(InteractCoroutine());
            //StartCoroutine(InteractCoroutine());
            if (remainingTime > 0)
            {
                remainingTime = stunTime;
                Debug.Log("remaining");
            }
            if (remainingTime <= 0)
            {
                remainingTime = stunTime;
                StartCoroutine(InteractCoroutine());
                Debug.Log("coroutine");
            }
        }

        IEnumerator InteractCoroutine()
        {
            enemyInt.setStun(true);
            while (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                yield return null;
            }
            // yield return new WaitForSeconds(stunTime);
            enemyInt.setStun(false);
            yield return null;
        }
        
        public override void OnFairyExit()
        {
            enemyInt.setStun(false);
        }
    }
}