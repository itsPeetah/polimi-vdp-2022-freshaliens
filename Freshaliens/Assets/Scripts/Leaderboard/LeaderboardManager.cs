using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Freshaliens.Social {
    public class LeaderboardManager : MonoBehaviour
    {
        private const string API_URL = "https://vdp22-freshaliens-leaderboard.vercel.app/api/leaderboard";
        //private const string API_URL = "http://localhost:3000/api/leaderboard";

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
            //if (!PauseMenu.GameIsPaused && !stop) {
            //    time += Time.deltaTime;
            //}

            //Debug.Log(TimeToString(time));

            //if (Input.GetKeyDown(KeyCode.Alpha0)) {
            //    PostTime("Debug");
            //}
        }

        public void PostTime(string name) {
            Debug.Log("Posting time as " + name);
            string fields = $"name={name}&level={Instance.currentLevel}&time={Instance.TimeAsString}";
            WWWForm form = new WWWForm();
            form.AddField("name", name);
            form.AddField("time", TimeAsString);
            form.AddField("level", currentLevel);
            StartCoroutine(SendPost(form));
        }

        private IEnumerator SendPost(/*string payload*/WWWForm form) {

            

            UnityWebRequest www = UnityWebRequest.Post(API_URL, /*payload*/ form);
            

            yield return www.SendWebRequest();
            Debug.Log(www.result);
            
            Debug.Log("Posted: " + form);
            www.Dispose();
            Debug.Log("Disposed of www");
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