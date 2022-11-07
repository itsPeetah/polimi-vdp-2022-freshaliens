using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MenuManagement
{
    public class GameMenu : Menu<GameMenu>
    {
 
        public void OnPausePressed()
        {
            PauseMenu.GameIsPaused = !PauseMenu.GameIsPaused;
            Time.timeScale = 0f;
            PauseMenu.Open();
        }
        //when you are playing, the update check if you want to pause
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Pause();
            }
                
        }
        private void Pause()
        {

            Time.timeScale = 0f;
            PauseMenu.GameIsPaused = true;
            PauseMenu.Open();
            
        }
    }
    
    
}
