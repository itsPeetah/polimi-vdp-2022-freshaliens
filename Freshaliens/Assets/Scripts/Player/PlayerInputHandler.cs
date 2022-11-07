using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private string horizontalAxis = "Horizontal";
    [SerializeField] private string jumpAxis = "Jump";
    [SerializeField] private string fireAxis = "Fire";

    public bool GetJumpInput() {
        return Input.GetButtonDown(jumpAxis);
    }

    public float GetWalkingDirection() {
        return Input.GetAxisRaw(horizontalAxis);
    }

    public Vector2 GetAimPosition() {
        return Input.mousePosition;
    }

    public bool GetFireInput() {
        return Input.GetButtonDown(fireAxis);
    }

}
