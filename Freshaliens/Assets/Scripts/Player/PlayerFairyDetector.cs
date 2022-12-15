using UnityEngine;

using Freshaliens.Interaction;

namespace Freshaliens
{
    /// <summary>
    /// Interactable component to be put on the player to allow interaction with the fairy.
    /// Used for fairy jumping
    /// </summary>
    public class PlayerFairyDetector : Interactable
    {
        private bool detected = false;
        private float stayTimer = 0;

        [Header("Air jump")]
        [SerializeField] private float maxFairyJumpTimeFrame = 0.5f;

        public bool Detected => detected;
        public bool CanFairyJump => detected /* && stayTimer <= maxFairyJumpTimeFrame */;
        public bool CanFairyJumpWithTimeFrame => detected && stayTimer <= maxFairyJumpTimeFrame;

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
}