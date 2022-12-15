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
        [Header("Initial Name Picker")]
        [SerializeField] private GameObject namePickerWindow = null;
        [SerializeField] private TMP_InputField namePickInputField = null;
        [SerializeField] private Button namePickButton = null;
        [Header("Scoreboard")]
        [SerializeField] private GameObject scoreboardWindow = null;
        [SerializeField] private TMP_InputField nameChangeInputField = null;
        [SerializeField] private Button nameChangeButton = null;
        [SerializeField] private TextMeshProUGUI nameLabel = null;
        [SerializeField] private Transform entryNameColumn = null;
        [SerializeField] private Transform entryTimeColumn = null;
        [Header("Pooling")]
        [SerializeField] private int entryPoolSize = 50;
        [SerializeField] private GameObject entryNamePrefab = null;
        [SerializeField] private GameObject entryTimePrefab = null;

        private int currentlySelectedLevel = 1;
        private TextMeshProUGUI[] entryNameLabels = null;
        private TextMeshProUGUI[] entryTimeLabels = null;

        private event System.Action<string> onNameChanged;
        private event System.Action<int> onDisplayedLevelChanged;
        private event System.Action onDataAvailable;

        protected override void Start()
        {
            base.Start();

            onDataAvailable += () => {
                onDisplayedLevelChanged?.Invoke(currentlySelectedLevel);
            };
            onNameChanged += (n) =>
            {
                namePickerWindow.SetActive(false);
                scoreboardWindow.SetActive(true);
                nameLabel.SetText($"Hello, {n}!");
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
            namePickButton.interactable = namePickInputField.text.Length > 0;
            nameChangeButton.interactable = nameChangeInputField.text.Length > 0;
        }

        public void SubmitName()
        {
            string name = namePickInputField.text;
            if (name.Length < 1) return;
            PlayerData.Instance.GenerateName(name);
            // TODO Upload existing times
            onNameChanged?.Invoke(PlayerData.Instance.LeaderboardName);
        }

        // This has a little duplication but I'm too tired rn
        public void ChangeName()
        {
            string name = nameChangeInputField.text;
            if (name.Length < 1) return;
            PlayerData.Instance.GenerateName(name);
            onNameChanged?.Invoke(PlayerData.Instance.LeaderboardName);
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
                if (PlayerData.Instance.LeaderboardName.Equals(string.Empty))
                {
                    namePickerWindow.SetActive(true);
                    scoreboardWindow.SetActive(false);
                }
                else {
                    onNameChanged?.Invoke(PlayerData.Instance.LeaderboardName);
                }
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

            entryNameLabels = new TextMeshProUGUI[entryPoolSize];
            entryTimeLabels = new TextMeshProUGUI[entryPoolSize];

            for (int i = 0; i < entryPoolSize; i++) {
                entryNameLabels[i] = Instantiate(entryNamePrefab, entryNameColumn).GetComponentInChildren<TextMeshProUGUI>();
                entryTimeLabels[i] = Instantiate(entryTimePrefab, entryTimeColumn).GetComponentInChildren<TextMeshProUGUI>();

                // This code sucks and is repeated a lot, but again I am too tired for this shit that is already overscoped enough
                // I can't even blame it on anyone, really, it's my idea...
                entryNameLabels[i].transform.parent.gameObject.SetActive(false);
                entryTimeLabels[i].transform.parent.gameObject.SetActive(false);
            }
        }

        private void ReloadLeaderboardEntries() {

            if (!dataAlreadyDownloaded) return;

            List<(string, string)> levelTimes = leaderboardData.GetTimes(currentlySelectedLevel);
            int maxEntriesDisplayed = Mathf.Min(entryPoolSize, levelTimes.Count);

            for (int i = 0; i < entryPoolSize; i++) {
                // Hide all entries that do not have data associated with them
                if (i >= maxEntriesDisplayed) {
                    entryNameLabels[i].transform.parent.gameObject.SetActive(false);
                    entryTimeLabels[i].transform.parent.gameObject.SetActive(false);
                    continue;
                }
                (string pName, string lTime) = levelTimes[i];
                entryNameLabels[i].SetText(pName);
                entryTimeLabels[i].SetText(lTime);

                entryNameLabels[i].transform.parent.gameObject.SetActive(true);
                entryTimeLabels[i].transform.parent.gameObject.SetActive(true);
            }
        }
    }
}