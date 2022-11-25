using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;
namespace MenuManagement
{
    public class PauseMenu : Menu<PauseMenu>
    {
        private static bool _gameIsPaused = false;
        public static bool GameIsPaused { get => _gameIsPaused; set => _gameIsPaused = value; }

        public void OnResumePressed()
        {
            Time.timeScale = 1f;
            base.OnBackPressed();
        }

        public void OnRestartPressed()
        {
            Time.timeScale = 1f;
            base.OnBackPressed();
            
            // LOAD THE NEXT LEVEL
        }
        
        public void OnMainMenuPressed()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
            MainMenu.Open();
        }
        
        public void OnQuitPressed()
        {
            Application.Quit();
        }
        //checks if you want to quit the menu with the ESC button
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("premuto ESC, _gameIsPaused è "+ _gameIsPaused);
                if (_gameIsPaused)
                {
                   
                    Resume();
                }

                _gameIsPaused = !_gameIsPaused;
            }
        }

        private void Resume()
        {
            OnResumePressed();
            //Debug.Log("OnResumePressed, _gameIsPaused è "+ _gameIsPaused);
        }


    }
}
