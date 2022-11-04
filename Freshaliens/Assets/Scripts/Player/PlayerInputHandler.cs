using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private string horizontalAxis = "Horizontal";
    [SerializeField] private string jumpAxis = "Jump";

    public bool GetJumpInput() {
        return Input.GetButtonDown(jumpAxis);
    }

    public float GetWalkingDirection() {
        return Input.GetAxisRaw(horizontalAxis);
    }

}
