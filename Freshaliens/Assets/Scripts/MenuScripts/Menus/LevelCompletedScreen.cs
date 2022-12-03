using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MenuManagement
{
        
    public class LevelCompletedScreen : Menu<LevelCompletedScreen>
    {
        public void OnEnable()
        {
            Time.timeScale = 0f;
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
            SceneLoadingManager.LoadLevelSelection();
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

    }
}
