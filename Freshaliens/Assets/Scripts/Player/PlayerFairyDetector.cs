using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFairyDetector : Interactable
{
    private bool detected = false;
    private float stayTimer = 0;

    [Header("Air jump")]
    [SerializeField] private float maxFairyJumpTimeFrame = 0.5f;

    public bool Detected => detected;
    public bool CanFairyJump => detected && stayTimer <= maxFairyJumpTimeFrame;

    public override void OnFairyEnter()
    {
        detected = true;

        stayTimer = 0;
    }

    public override void OnFairyStay()
    {
        stayTimer += Time.deltaTime;
    }

    public override void OnFairyExit()
    {
        detected = false;
    }
}
