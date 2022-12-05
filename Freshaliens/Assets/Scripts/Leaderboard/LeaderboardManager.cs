using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MenuManagement;
using UnityEngine.Networking;

namespace Freshaliens.Social {
    public class LeaderboardManager : MonoBehaviour
    {
        private const string API_URL = "https://vdp22-freshaliens-leaderboard.vercel.app/api/leaderboard";

        private static LeaderboardManager instance = null;
        public static LeaderboardManager Instance => instance;

        [SerializeField] private int currentLevel = 1;
        [SerializeField] private float time = 0;
        [SerializeField] private bool stop = false;

        public string TimeAsString => TimeToString(time);

        private void Awake()
        {
            instance = this;
        }

        private void OnDestroy()
        {
            instance = null;
        }

        private void Update()
        {
            if (!PauseMenu.GameIsPaused && !stop) {
                time += Time.deltaTime;
            }

            //Debug.Log(TimeToString(time));

            if (Input.GetKeyDown(KeyCode.Alpha0)) {
                PostTime("Debug");
            }
        }

        public void PostTime(string name) {
            WWWForm form = new WWWForm();
            form.AddField("name", name);
            form.AddField("level", Instance.currentLevel);
            form.AddField("time", TimeToString(Instance.time));

            StartCoroutine(PostTimeCoroutine(form));
        }

        private static IEnumerator PostTimeCoroutine(WWWForm form) {
            Debug.Log("Posting time");
            using (UnityWebRequest www = UnityWebRequest.Post(API_URL, form)) {
                yield return www.SendWebRequest();
            }
            Debug.Log("Time posted");
        }

        public static string TimeToString(float timeInSeconds) {
            int minutes = (int)(timeInSeconds / 60);
            int seconds = (int)(timeInSeconds) - (minutes * 60);
            int milliseconds = (int)((timeInSeconds - (int)timeInSeconds) * 1000);

            return $"{minutes}:{seconds}.{milliseconds}";
        }

        public static void Stop() {
            if (Instance != null) Instance.stop = true;
        }
    }
}