using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freshaliens.Utility.Component
{
    public class Rotator : MonoBehaviour
    {
        public enum Space { Local, Global }
        [SerializeField] private Space rotationSpace = Space.Local;
        [SerializeField] private Vector3 rotationAxis = Vector3.up;
        [SerializeField] private float rotationSpeed = 10f;

        private Transform ownTransform;

        private void Start()
        {
            ownTransform = transform;
        }

        private void Update()
        {

            ownTransform.Rotate(rotationSpeed * Time.deltaTime * rotationAxis, rotationSpace == Space.Local ? UnityEngine.Space.Self : UnityEngine.Space.World);
            
        }
    }
}