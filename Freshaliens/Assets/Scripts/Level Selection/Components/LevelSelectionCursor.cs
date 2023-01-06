using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freshaliens.LevelSelection.Components
{

    public class LevelSelectionCursor : MonoBehaviour
    {
        private struct Target
        {
            public int index;
            public Vector3 position;
        }

        [Header("Settings")]
        [SerializeField] private float movementSpeed = 15f;
        [SerializeField] private float waitBetweenTargets = 0.1f;

        private List<Target> queuedMovements = new List<Target>();
        private bool isMoving = false;
        private Transform ownTransform;

        public bool IsMoving => isMoving;

        private void Start()
        {
            ownTransform = transform;
        }

        public void SetTarget(int index, Vector3 position)
        {
            // Add the target to the queue. If it is already in the queue, the player is trying to go back while the movement is still happening
            // If this happens, cancel all movements after the target one
            int existingIndex = queuedMovements.FindIndex((Target t) => t.index == index);
            if (existingIndex == -1) queuedMovements.Add(new Target { index = index, position = position });
            else queuedMovements.RemoveRange(existingIndex, queuedMovements.Count - existingIndex);

            if (!isMoving) StartCoroutine(nameof(MoveCursor));
            //AudioManager1.instance.PlayMusic("levelselection");
        }

        private IEnumerator MoveCursor()
        {
            isMoving = true;
            Vector3 startingPosition, targetPosition, currentPosition;
            float distance, timeToTravel, timeElapsed;
            while (queuedMovements.Count > 0)
            {

                startingPosition = ownTransform.position;
                targetPosition = queuedMovements[0].position;
                currentPosition = targetPosition;

                distance = Vector3.Distance(startingPosition, targetPosition);
                timeToTravel = distance / movementSpeed;
                timeElapsed = 0;

                while (timeElapsed <= timeToTravel)
                {

                    currentPosition = Vector3.Lerp(startingPosition, targetPosition, timeElapsed / timeToTravel);
                    ownTransform.position = currentPosition;
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }

                ownTransform.position = currentPosition;
                if(queuedMovements.Count > 0) queuedMovements.RemoveAt(0);

                yield return new WaitForSeconds(waitBetweenTargets);
            }

            isMoving = false;
            //Debug.Log("stopped");
        }

        public void SetPosition(Vector3 position) => ownTransform.position = position;
    }
}