using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freshaliens.Level.Components
{
    /// <summary>
    /// Simple 2-point path moving platform
    /// </summary>
    public class MovingPlatform : MonoBehaviour
    {
        // TODO Add player check to add movement

        private enum State {AtStart, AtEnd, Moving}

        [Header("Path")]
        [SerializeField] private Vector3 startPositionOS = Vector3.left, endPositionOS = Vector3.right;
        private Vector3 startPositionWS, endPositionWS;

        [Header("Movement")]
        [SerializeField] private float movementSpeed = 3f;
        [SerializeField] private AnimationCurve movementCurve = AnimationCurve.EaseInOut(0f,0f,1f,1f);
        [SerializeField] private float waitTimeAtEndPoint = 0f;
        private State currentState = State.AtStart;
        

        public Vector3 StartPosition
        {
            get => transform.TransformPoint(startPositionOS);
            set { startPositionOS = transform.InverseTransformPoint(value); startPositionWS = value;}
        }

        public Vector3 EndPosition
        {
            get => transform.TransformPoint(endPositionOS);
            set { endPositionOS = transform.InverseTransformPoint(value); startPositionWS = value; }
        }

        private void Start()
        {
            currentState = State.AtStart;
            startPositionWS = transform.TransformPoint(startPositionOS);
            endPositionWS = transform.TransformPoint(endPositionOS);
            SetPosition(startPositionWS);
        }

        private void Update()
        {
            if (currentState != State.Moving) StartCoroutine(nameof(Move));
        }

        private IEnumerator Move() {

            State startingState = currentState;
            currentState = State.Moving;
            yield return new WaitForSeconds(waitTimeAtEndPoint);

            float distanceBetweenPoints = Vector3.Distance(StartPosition, EndPosition);
            float timeToTravel = distanceBetweenPoints / movementSpeed;

            float t = 0;
            float progress;
            SetPosition(GetPositionInPath(startingState == State.AtStart ? 0 : 1));
            while (t <= timeToTravel) {
                progress = startingState == State.AtStart ? t / timeToTravel : 1 - t/timeToTravel;
                progress = movementCurve.Evaluate(progress);
                SetPosition(GetPositionInPath(progress));
                t += Time.deltaTime;
                yield return null;
            }
            SetPosition(GetPositionInPath(startingState == State.AtStart ? 1 : 0));

            currentState = startingState == State.AtStart ? State.AtEnd : State.AtStart;
        }

        private Vector3 GetPositionInPath(float t) {
            if (t <= 0) return startPositionWS;
            if (t >= 1) return endPositionWS;

            return Vector3.Lerp(startPositionWS, endPositionWS, t);
        }

        private void SetPosition(Vector3 position) {
            transform.position = position;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
        }
    }
}