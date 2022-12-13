using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Freshaliens.Management;

namespace Freshaliens.UI
{
    public class PauseHUD : LevelUIScreen
    {
        private enum PauseState {
            Pause,
            Settings,
        }

        [Header("Submenus")]
        [SerializeField] private GameObject defaultPausedScreen = null;
        [SerializeField] private GameObject settingsScreen = null;

        private PauseState currentPauseState = PauseState.Pause;

        protected override void Start()
        {
            base.Start();
            LevelManager.Instance.onPauseToggle += SetActive;
        }

        private void EnterPause() {
            currentPauseState = PauseState.Pause;
        }

        private void UpdateScreensVisibility() {
            defaultPausedScreen.SetActive(currentPauseState == PauseState.Pause);
            settingsScreen.SetActive(currentPauseState == PauseState.Settings);
        }
    }
}