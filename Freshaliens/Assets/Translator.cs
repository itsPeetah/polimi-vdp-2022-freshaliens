using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translator : MonoBehaviour
{
    public enum Space { Local, Global }
    [SerializeField] private Space translationSpace = Space.Local;
    [SerializeField] private Vector3 translationAxis = Vector3.up;
    [SerializeField] private float movementSpeed = 10f;

    private Transform ownTransform;

    private void Start()
    {
        ownTransform = transform;
    }

    private void Update()
    {

        ownTransform.Translate(movementSpeed * Time.deltaTime * translationAxis, translationSpace == Space.Local ? UnityEngine.Space.Self : UnityEngine.Space.World);

    }
}
