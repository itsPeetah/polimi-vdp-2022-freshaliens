using UnityEngine;
using UnityEngine.Serialization;

public class TiltWithMovementRigidBody : MonoBehaviour
{
    [SerializeField, Min(0)] private float maxAngle = 30f;
    [SerializeField, Min(0)] private float maxPosDelta = 10f;

    [SerializeField] private Rigidbody2D rigidBody;
    private Transform ownTransform;
    private float maxRotationAngle = 0, minRotationAngle = 0;
    private Vector3 previousPosition = Vector3.zero;



    private void Start()
    {
        ownTransform = transform;
        previousPosition = rigidBody.position;

        maxRotationAngle = -maxAngle;
        minRotationAngle = maxAngle;
    }

    private void FixedUpdate()
    {
        Vector3 pos = rigidBody.position;
        Vector3 delta = pos - previousPosition;
        bool toTheRight = pos.x >= previousPosition.x;
        float mag = delta.magnitude;
        float target = toTheRight ? maxRotationAngle : minRotationAngle;

        float angle = Mathf.Lerp(0, target, (mag  + 0.0001f) / (maxPosDelta* Time.deltaTime));
        ownTransform.localRotation = Quaternion.Euler(0, 0, angle) ;

        previousPosition = pos;
    }
}
