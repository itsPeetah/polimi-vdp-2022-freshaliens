using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freshaliens.LevelSelection.Components
{
    public class LevelSelectionCamera : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D boundCollider = null;
        [SerializeField] private Transform target = null;
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

            float x = Mathf.Clamp(target.position.x, minX, maxX);
            float y = Mathf.Clamp(target.position.y, minY, maxY);
            float z = ownTransform.position.z;

            transform.position = new Vector3(x, y, z);
        }


    }
}