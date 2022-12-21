using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private Button backButton = null;
        [Header("Scoreboard")]
        [SerializeField] private GameObject scoreboardWindow = null;
        [SerializeField] private Transform recordContainer = null;
        [Header("Record Pooling")]
        [SerializeField] private int recordPoolSize = 50;
        [SerializeField] private GameObject recordObjectPrefab = null;

        private int currentlySelectedLevel = 1;
        private LeaderboardEntry[] recordObjects = null;

        private event System.Action<int> onDisplayedLevelChanged;
        private event System.Action onDataAvailable;

        protected override void Start()
        {
            base.Start();

            onDataAvailable += () => {
                onDisplayedLevelChanged?.Invoke(currentlySelectedLevel);
            };
            onDisplayedLevelChanged += (l) =>
            {
                ReloadLeaderboardEntries();
            };

            InitializeEntryPool();
        }

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
            if (active)
            {
                // Download data
                if (!dataAlreadyDownloaded)
                {
                    StartCoroutine(nameof(DownloadData));
                }

                scoreboardWindow.SetActive(active);                
            }
        }

        private IEnumerator DownloadData() {
            UnityWebRequest www = UnityWebRequest.Get(Leaderboard.API_URL);
            isDownloading = true;
            yield return www.SendWebRequest();
            if (www.responseCode == 200)
            {
                string jsonText = www.downloadHandler.text;
                SaveData(jsonText);
            }
            www.Dispose();
            isDownloading = false;
            onDataAvailable?.Invoke();
        }

        private void SaveData(string jsonText) {
            Debug.Log(jsonText);
            dataAsJSONString = jsonText;
            leaderboardData = Leaderboard.ParseJSONData(jsonText);
            dataAlreadyDownloaded = true;
        }

        public void SelectLevelToDisplay(int level) {
            currentlySelectedLevel = level;
            onDisplayedLevelChanged?.Invoke(level);
        }

        private void InitializeEntryPool() {
            recordObjects = new LeaderboardEntry[recordPoolSize];

            for (int i = 0; i < recordPoolSize; i++) {
                recordObjects[i] = Instantiate(recordObjectPrefab, recordContainer).GetComponentInChildren<LeaderboardEntry>();
                recordObjects[i].SetActive(false);
            }
        }

        private void ReloadLeaderboardEntries() {

            if (!dataAlreadyDownloaded) return;

            List<(string, string)> levelTimes = leaderboardData.GetTimes(currentlySelectedLevel);
            int maxEntriesDisplayed = Mathf.Min(recordPoolSize, levelTimes.Count);

            for (int i = 0; i < recordPoolSize; i++) {
                // Hide all entries that do not have data associated with them
                if (i >= maxEntriesDisplayed) {
                    recordObjects[i].SetActive(false);
                    continue;
                }
                recordObjects[i].SetText(levelTimes[i]);
                recordObjects[i].SetActive(true);
            }
        }
    }
}