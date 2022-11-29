using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freshaliens.LevelSelection.Components
{
    public class LevelSelectionCamera : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D boundCollider = null;
        [SerializeField] private Transform target = null;
        [SerializeField] private Vector2 offset = Vector2.zero;
        [SerializeField, Range(0, 0.1f)] private float smoothing = 1f;

        private Transform ownTransform = null;
        private Camera cam = null;
        private Bounds bounds;

        private void Start()
        {
            ownTransform = transform;
            cam = GetComponent<Camera>();
            bounds = boundCollider.bounds;
        }

        private void LateUpdate()
        {
            float halfViewportHeightWS = cam.orthographicSize;
            float halfViewportWidthWS = halfViewportHeightWS * cam.aspect;

            float minX = bounds.min.x + halfViewportWidthWS;
            float minY = bounds.min.y + halfViewportHeightWS;
            float maxX = bounds.max.x - halfViewportWidthWS;
            float maxY = bounds.max.x - halfViewportHeightWS;

            float x = Mathf.Clamp(target.position.x + offset.x, minX, maxX);
            float y = Mathf.Clamp(target.position.y + offset.y, minY, maxY);
            float z = ownTransform.position.z;

            Vector3 targetPosition = new Vector3(x, y, z);
            float distance = Vector3.Distance(ownTransform.position, targetPosition);
            ownTransform.position = distance < 0.01f ? targetPosition : Vector3.Lerp(ownTransform.position, targetPosition, smoothing);
        }


    }
}