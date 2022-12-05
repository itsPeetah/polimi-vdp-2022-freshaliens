using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Freshaliens.Social;
using TMPro;

namespace MenuManagement
{
        
    public class LevelCompletedScreen : Menu<LevelCompletedScreen>
    {
        [SerializeField] private TextMeshProUGUI timeLabel = null;
        [SerializeField] private TMP_InputField nameField = null;


        public void OnEnable()
        {
            Time.timeScale = 0f;
            LeaderboardManager.Stop();
            if (LeaderboardManager.Instance != null) {
                timeLabel.SetText($"Your time is: {LeaderboardManager.Instance.TimeAsString}");
            }
        }

        public void OnRestartPressed()
        {
            // delete the current screen
            base.OnBackPressed();
            SceneLoadingManager.ReloadLevel();
        }

        public void OnNextLevelPressed()
        {
            base.OnBackPressed();
            SceneLoadingManager.LoadFirstScene();
            PlayerData.Instance.Save();
        }

        public void OnMainMenuPressed()
        {   
            base.OnBackPressed();
            SceneLoadingManager.LoadMainMenuLevel();
            PlayerData.Instance.Save();
        }
        
        public void OnDisable()
        {
            Time.timeScale = 1f;
        }

        public void SubmitTime() {
            string name = nameField.text;
            if (name.Length < 1) name = "Anonymous";
            if (LeaderboardManager.Instance != null) {
                Debug.Log("Pressed POST and Instance != null");
                LeaderboardManager.Instance.PostTime(name);
                Debug.Log("Returned from Post");
            }
        }
    }
}
