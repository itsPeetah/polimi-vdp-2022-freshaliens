using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Freshaliens.Social;
using UnityEngine.UI;
using TMPro;
namespace Freshaliens.UI
{
    public class LeaderboardMenu : UIScreen
    {
        private static bool dataAlreadyDownloaded = false;
        private static string dataAsJSONString = "";
        private static Leaderboard.LBTable leaderboardData = null;
        private static bool isDownloading = false;

        [Header("UI Elements")]
        [SerializeField] private GameObject namePickerWindow = null;
        [SerializeField] private GameObject scoreboardWindow = null;
        [SerializeField] private TMP_InputField nameInputField = null;
        [SerializeField] private Button namePickButton = null;
        [SerializeField] private Button backButton = null;

        private void Update()
        {
            backButton.interactable = !isDownloading;
            namePickButton.interactable = nameInputField.text.Length > 0;
        }

        public void SubmitName() {
            string name = nameInputField.text;
            if (name.Length < 1) return;
            PlayerData.Instance.GenerateName(name);
            namePickerWindow.SetActive(false);
            scoreboardWindow.SetActive(true);
        }

        public void OnBackPressed()
        {    
            TitleScreenManager.Instance.ResetMenuView();
        }

        public override void SetActive(bool active)
        {
            base.SetActive(active);
            if (active)
            {
                // Download data
                if (!dataAlreadyDownloaded)
                {
                    StartCoroutine(nameof(DownloadData));
                }

                // Pick name if needed
                bool nameNotPickedYet = PlayerData.Instance.LeaderboardName.Equals(string.Empty);
                namePickerWindow.SetActive(nameNotPickedYet);
                scoreboardWindow.SetActive(!nameNotPickedYet);
            }
        }

        private IEnumerator DownloadData() {
            UnityWebRequest www = UnityWebRequest.Get(Leaderboard.API_URL);
            isDownloading = true;
            yield return www.SendWebRequest();
            isDownloading = false;
            if (www.responseCode == 200)
            {
                string jsonText = www.downloadHandler.text;
                SaveData(jsonText);
            }
            www.Dispose();
        }

        private void SaveData(string jsonText) {
            Debug.Log(jsonText);
            dataAsJSONString = jsonText;
            leaderboardData = Leaderboard.ParseJSONData(jsonText);
            dataAlreadyDownloaded = true;
        }
    }
}