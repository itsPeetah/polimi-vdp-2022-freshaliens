using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

namespace MenuManagement
{
    public class MainMenu : Menu<MainMenu>
    {
        public bool fadeToPlay = true;                          // should it use the fading transition?
        [SerializeField] private float playDelay = 0.5f;        // # seconds before loading the gameplay screen
        [SerializeField] private TransitionFader transitionFaderPrefab;
        
        public void OnSettingsPressed()
        {
            //print("SETTINGS");
            SettingsMenu.Open();
        }

        public void OnCreditPressed()
        {
            //print("CREDITS");
            CreditsScreen.Open();
        } 
        public void OnFeedbackPressed()
        {
            //print("FEEDBACK");
            FeedBackScreen.Open();  
        }
        public void OnPlayPressed()
        {
            if (fadeToPlay)
            {
                StartCoroutine(OnPlayPressedRoutine());
            }
            else
            {
                SceneLoadingManager.LoadLevelSelection();
                GameMenu.Open();
            }
        }

        private IEnumerator OnPlayPressedRoutine()
        {
            TransitionFader.PlayTransition(transitionFaderPrefab);
            SceneLoadingManager.LoadLevelSelection();
            yield return new WaitForSeconds(playDelay);
            GameMenu.Open();
        }


        public override void OnBackPressed()
        {
            Application.Quit();
        }
    }
}
