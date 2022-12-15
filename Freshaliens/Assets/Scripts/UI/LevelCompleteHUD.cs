using UnityEngine;
using TMPro;
using Freshaliens.Management;

namespace Freshaliens.UI
{
    public class LevelCompleteHUD : UIScreen
    {
        [Header("Elements")]
        [SerializeField] private TextMeshProUGUI timeLabel = null;

        protected override void Start()
        {
            base.Start();
            LevelManager.Instance.onGameWon += () => timeLabel.SetText(LevelManager.Instance.CurrentLevelTimerAsString);
        }

        public void OnContinuePress() {
            LevelManager.Instance.QuitLevel();
        }

        public void OnRestartPress() {
            LevelManager.Instance.ReloadLevel();
        }
    }
}