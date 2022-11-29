using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltWithMovement : MonoBehaviour
{
    [SerializeField, Min(0)] private float maxAngle = 30f;
    [SerializeField, Min(0)] private float maxPosDelta = 10f;

    private Transform ownTransform;
    private float maxRotationAngle = 0, minRotationAngle = 0;
    private Vector3 previousPosition = Vector3.zero;



    private void Start()
    {
        ownTransform = transform;
        previousPosition = ownTransform.position;

        maxRotationAngle = -maxAngle;
        minRotationAngle = maxAngle;
    }

    private void Update()
    {
        Vector3 pos = ownTransform.position;
        Vector3 delta = pos - previousPosition;
        bool toTheRight = pos.x >= previousPosition.x;
        float mag = delta.magnitude;
        float target = toTheRight ? maxRotationAngle : minRotationAngle;

        float angle = Mathf.Lerp(0, target, mag / (maxPosDelta* Time.deltaTime));
        transform.localRotation = Quaternion.Euler(0, 0, angle) ;

        previousPosition = pos;
    }
}
