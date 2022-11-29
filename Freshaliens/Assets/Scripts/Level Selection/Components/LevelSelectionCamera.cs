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

        private void Update()
        {
            float halfViewportHeightWS = cam.orthographicSize;
            float halfViewportWidthWS = cam.orthographicSize * cam.aspect;


            
        }
    }
}