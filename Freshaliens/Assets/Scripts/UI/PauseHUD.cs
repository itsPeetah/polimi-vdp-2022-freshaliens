using UnityEngine;
using Freshaliens.Management;

namespace Freshaliens.UI
{
    public class PauseHUD : UIScreen
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

        
        private void OnEnable()
        {
            currentPauseState = PauseState.Pause;
            UpdateScreensVisibility();
        }

        private void UpdateScreensVisibility() {
            defaultPausedScreen.SetActive(currentPauseState == PauseState.Pause);
            settingsScreen.SetActive(currentPauseState == PauseState.Settings);
        }

        public void OnResumePressed() {
            LevelManager.Instance.TogglePause();
        }

        public void OnRestartPressed() {
            LevelManager.Instance.ReloadLevel();
        }

        public void OnSettingsPressed() {
            currentPauseState = PauseState.Settings;
            UpdateScreensVisibility();

        }

        public void CloseSettings() {
            currentPauseState = PauseState.Pause;
            UpdateScreensVisibility();
        }

        public void OnBackPressed() {
            LevelManager.Instance.QuitLevel();
        }
    }
}