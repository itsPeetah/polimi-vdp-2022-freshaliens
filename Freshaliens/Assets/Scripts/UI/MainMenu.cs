using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freshaliens.UI
{
    public class MainMenu : UIScreen
    {
        public void OnPlayPressed() {
            //SceneLoadingManager.LoadLevelSelection();
            SceneLoadingManager.LoadFirstScene();
            PlayButtonSound();
        }

        public void OnSettingsPressed() {
            TitleScreenManager.Instance.SwitchView(TitleScreenManager.MenuView.Settings);
            PlayButtonSound();
        }

        public void OnFeedbackPressed() {
            TitleScreenManager.Instance.SwitchView(TitleScreenManager.MenuView.Feedback);
            PlayButtonSound();
        }

        public void OnCreditsPressed() {
            TitleScreenManager.Instance.SwitchView(TitleScreenManager.MenuView.Credits);
            PlayButtonSound();
        }

        public void OnLeaderboardPressed() {
            TitleScreenManager.Instance.SwitchView(TitleScreenManager.MenuView.Leaderboard);
            PlayButtonSound();
        }

        public void OnQuitPressed() {
            TitleScreenManager.Instance.SwitchView(TitleScreenManager.MenuView.Quit);
            PlayButtonSound();
        }
    }
}