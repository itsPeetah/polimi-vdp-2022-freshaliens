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
            LevelManager.ReloadLevel();
        }

        public void OnNextLevelPressed()
        {
            base.OnBackPressed();
            LevelManager.LoadNextLevel();
        }

        public void OnMainMenuPressed()
        {   
            base.OnBackPressed();
            LevelManager.LoadMainMenuLevel();
        }
        
        public void OnDisable()
        {
            Time.timeScale = 1f;
        }

    }
}
