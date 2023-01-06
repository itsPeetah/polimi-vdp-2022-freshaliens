using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.IO;
using System.Linq;

namespace Freshaliens.Social {

    public static class Leaderboard {
        public const string API_URL = "https://vdp22-freshaliens-leaderboard.vercel.app/api/leaderboards/official";

        public class LBRow : List<string> { }
        public class LBTable : Dictionary<string, LBRow>
        {
            public List<(string, string)> GetTimes(int level)
            {
                List<(string, string)> times = new List<(string, string)>();

                if (level < 1) return times;

                // Filter
                foreach (var kvp in this)
                {
                    string playerName = kvp.Key;
                    LBRow row = kvp.Value;
                    if (row.Count > level && row[level] != null)
                    {
                        string time = row[level];
                        times.Add((playerName, time));
                    }
                }

                // Sort
                times.Sort(((string name, string time) a, (string name, string time) b) => {
                    float va = FloatTimeToString.ParseString(a.time);
                    float vb = FloatTimeToString.ParseString(b.time);
                    return va.CompareTo(vb);
                });

                return times;
            }
        }

        public static LBTable ParseJSONData(string jsonText) {

            LBTable table = new LBTable();

            using var reader = new JsonTextReader(new StringReader(jsonText));
            string rowKey = "";
            LBRow row = null;

            while (reader.Read())
            {
                JsonToken tokenType = reader.TokenType;
                object value = reader.Value;

                switch (tokenType)
                {
                    case JsonToken.PropertyName:
                        rowKey = value as string;
                        break;
                    case JsonToken.StartArray:
                        row = new LBRow
                        {
                            null // Still using the "null" value for level 0
                        };
                        break;
                    case JsonToken.String:
                        row?.Add(value as string);
                        break;
                    case JsonToken.EndArray:
                        table.Add(rowKey, row);
                        break;
                }
            }

            return table;
        }
    }


    public class LeaderboardManager : MonoBehaviour
    {
        private const string API_URL = "https://vdp22-freshaliens-leaderboard.vercel.app/api/leaderboards/official";
        //private const string API_URL = "http://localhost:3000/api/leaderboards/official";

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