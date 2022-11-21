using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MenuManagement
{
    public class GameCompletedScreen : Menu<GameCompletedScreen>
    {
        public void OnEnable()
        {
            Time.timeScale = 0f;
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