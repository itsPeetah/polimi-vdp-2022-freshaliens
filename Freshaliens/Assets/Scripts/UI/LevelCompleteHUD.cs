using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Freshaliens.Management;
namespace Freshaliens.UI
{
    public class LevelCompleteHUD : UIScreen
    {
        [Header("Elements")]
        [SerializeField] private TextMeshProUGUI timeLabel = null;
        [SerializeField] private TMP_InputField nicknameField = null;
        [SerializeField] private Button submitButton = null;

        protected override void Start()
        {
            base.Start();
            LevelManager.Instance.onGameWon += () =>
            {
                timeLabel.SetText(LevelManager.Instance.CurrentLevelTimerAsString);
                nicknameField.SetTextWithoutNotify(PlayerData.Instance.LeaderboardName);
            };
        }

        private void Update()
        {
            submitButton.interactable = nicknameField.text.Length >= 1;
        }

        public void OnContinuePress() {
            LevelManager.Instance.QuitLevel();
        }

        public void OnRestartPress() {
            LevelManager.Instance.ReloadLevel();
        }

        public void SubmitTimeToLeaderboard() {
            if (nicknameField.text.Length < 1) return;
            string playerName = nicknameField.text;
            PlayerData.Instance.LeaderboardName = playerName;
            // TODO Submit time
        }
    }
}