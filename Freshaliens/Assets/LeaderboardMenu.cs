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

        private int currentlySelectedLevel = 1;

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
                ReloadDisplayedScores();
            };
            
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

        private void SelectLevelToDisplay(int level) {
            currentlySelectedLevel = level;
            onDisplayedLevelChanged?.Invoke(level);
        }

        private void ReloadDisplayedScores() {

        }
    }
}