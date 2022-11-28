using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private string horizontalAxis = "Horizontal";
    [SerializeField] private string verticalAxis = "Vertical";
    [SerializeField] private string jumpAxis = "Jump";
    [SerializeField] private string actionAxis = "Fire";

    public bool GetJumpInput() {
        return Input.GetButtonDown(jumpAxis);
    }

    public float GetHorizontal() {
        return Input.GetAxisRaw(horizontalAxis);
    }

    public float GetVertical() {
        return Input.GetAxisRaw(verticalAxis);
    }

    public Vector2 GetAimPosition() {
        return Input.mousePosition;
    }

    public bool GetActionInput() {
        return Input.GetButtonDown(actionAxis);
    }

}
