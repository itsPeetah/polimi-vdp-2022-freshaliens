using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(PlayerInputHandler))]
public class FairyMovementController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float maxSpeed = 6f;
    [SerializeField] private float movementAcceleration = 5f;
    [SerializeField] private float movementDeceleration = 20f;
    // State
    private Vector2 movementDirection = Vector2.zero;
    private Vector2 currentVelocity = Vector2.zero;
    private bool isMoving = false;

    // Components
    private PlayerInputHandler input;
    private Rigidbody2D rbody;

    private void Start()
    {
        input = GetComponent<PlayerInputHandler>();
        rbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Debug.Log("Update");

        // Input
        movementDirection = new Vector2(input.GetHorizontal(), input.GetVertical());
        isMoving = movementDirection != Vector2.zero;

        // Calculate acceleration based on wether the player wants to move or stop
        float acceleration = movementAcceleration;
        if (!isMoving) acceleration = movementDeceleration;

        // Calculate change in velocity
        currentVelocity = rbody.velocity;
        Vector2 targetVelocity = movementDirection.normalized * maxSpeed;
        Vector2 deltaVelocity = targetVelocity - currentVelocity;
        float dampen = Mathf.Clamp01(acceleration * Time.deltaTime / maxSpeed);
        rbody.velocity = currentVelocity + dampen * deltaVelocity;
        


    }


}
