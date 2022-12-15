using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Freshaliens.Social;
using UnityEngine.UI;

namespace Freshaliens.UI
{
    public class LeaderboardMenu : UIScreen
    {
        private static bool dataAlreadyDownloaded = false;
        private static string dataAsJSONString = "";
        private static Leaderboard.LBTable leaderboardData = null;
        private static bool isDownloading = false;

        [Header("UI Elements")]
        [SerializeField] private Button backButton;

        private void Update()
        {
            backButton.interactable = !isDownloading;
        }

        public void OnBackPressed()
        {    
            TitleScreenManager.Instance.ResetMenuView();
        }

        public override void SetActive(bool active)
        {
            base.SetActive(active);
            if (active) {
                // Download data
                if (!dataAlreadyDownloaded) {
                    StartCoroutine(nameof(DownloadData));
                }

                // Pick name if needed
            }
        }

        private IEnumerator DownloadData() {
            UnityWebRequest www = UnityWebRequest.Get(Leaderboard.API_URL);
            yield return www.SendWebRequest();
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


            foreach (var kvp in leaderboardData) {
                Debug.Log(kvp.Key);
                foreach (var e in kvp.Value) {
                    Debug.Log(e);
                }
            }
        }
    }
}