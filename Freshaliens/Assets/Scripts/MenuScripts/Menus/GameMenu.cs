using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Freshaliens.Player.Components;

namespace MenuManagement
{
    public class GameMenu : Menu<GameMenu>
    {

        [SerializeField] private GameObject jumpSprite;
        [SerializeField] private GameObject[] heartSprites;
 
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
            

            // HACK This is **UGLY** but it's late and I can't be bothered
            heartSprites[0].SetActive(LivesManager.numberOfLives > 0);
            heartSprites[1].SetActive(LivesManager.numberOfLives > 1);
            heartSprites[2].SetActive(LivesManager.numberOfLives > 2);

            if(PlayerMovementController.Instance != null)
                jumpSprite.SetActive(PlayerMovementController.Instance.RemainingAirJupms > 0);

        }
        private void Pause()
        {

            Time.timeScale = 0f;
            PauseMenu.GameIsPaused = true;
            PauseMenu.Open();
            
        }
    }
    
    
}
