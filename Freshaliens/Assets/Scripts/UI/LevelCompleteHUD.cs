using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using Freshaliens.Management;
using Freshaliens.Social;

namespace Freshaliens.UI
{
    public class LevelCompleteHUD : UIScreen
    {
        [Header("Elements")]
        [SerializeField] private TextMeshProUGUI timeLabel = null;
        [SerializeField] private TMP_InputField nicknameField = null;
        [SerializeField] private Button submitButton = null;
        [SerializeField] private Button continueButton = null, restartButton = null;

        private bool isUploadingData = false;

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
            continueButton.interactable = !isUploadingData;
            restartButton.interactable = !isUploadingData;
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
            string playerTime = LevelManager.Instance.CurrentLevelTimerAsString;
            int currLevel = LevelManager.Instance.CurrentLevel;
            PostTime(playerName, playerTime, currLevel);
        }

        public void PostTime(string name, string time, int level)
        {
            //Debug.Log("Posting time as " + name);
            WWWForm form = new WWWForm();
            form.AddField("name", name);
            form.AddField("time", time);
            form.AddField("level", level);
            StartCoroutine(SendPost(form));
        }

        private IEnumerator SendPost(/*string payload*/WWWForm form)
        {
            isUploadingData = true;
            UnityWebRequest www = UnityWebRequest.Post(Leaderboard.API_URL, /*payload*/ form);
            yield return www.SendWebRequest();
            //Debug.Log(www.result);
            //Debug.Log("Posted: " + form);
            www.Dispose();
            //Debug.Log("Disposed of www");
            isUploadingData = false;
        }
    }
}