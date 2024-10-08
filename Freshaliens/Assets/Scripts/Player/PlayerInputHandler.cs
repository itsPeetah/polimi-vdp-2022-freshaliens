using UnityEngine;
using Freshaliens.Management;
namespace Freshaliens.Player.Components
{
    /// <summary>
    /// Centralized input handler for a gameobject
    /// </summary>
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private string horizontalAxis = "Horizontal";
        [SerializeField] private string verticalAxis = "Vertical";
        [SerializeField] private string jumpAxis = "Jump";
        [SerializeField] private string actionAxis = "Fire";

        private bool IsPaused => LevelManager.Instance.IsPaused;
        private bool SkipInput => LevelManager.Instance.CurrentPhase != LevelManager.LevelPhase.Playing;

        public bool GetJumpInput()
        {
            if (IsPaused || SkipInput) return false;
            return Input.GetButtonDown(jumpAxis);
        }

        public float GetHorizontal()
        {
            if (IsPaused || SkipInput) return 0;
            return Input.GetAxisRaw(horizontalAxis);
        }

        public float GetVertical()
        {
            if (IsPaused || SkipInput) return 0;
            return Input.GetAxisRaw(verticalAxis);
        }

        public bool GetActionInput()
        {
            if (IsPaused || SkipInput) return false;
            return Input.GetButtonDown(actionAxis);
        }

    }
}