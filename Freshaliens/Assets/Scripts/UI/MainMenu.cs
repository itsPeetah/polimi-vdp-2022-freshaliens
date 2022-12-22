using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freshaliens.UI
{
    public class MainMenu : UIScreen
    {
        public void OnPlayPressed() {
            SceneLoadingManager.LoadLevelSelection();
        }

        public void OnSettingsPressed() {
            TitleScreenManager.Instance.SwitchView(TitleScreenManager.MenuView.Settings);
        }

        public void OnFeedbackPressed() {
            TitleScreenManager.Instance.SwitchView(TitleScreenManager.MenuView.Feedback);
        }

        public void OnCreditsPressed() {
            TitleScreenManager.Instance.SwitchView(TitleScreenManager.MenuView.Credits);
        }

        public void OnLeaderboardPressed() {
            TitleScreenManager.Instance.SwitchView(TitleScreenManager.MenuView.Leaderboard);
        }

        public void OnQuitPressed() {
            TitleScreenManager.Instance.SwitchView(TitleScreenManager.MenuView.Quit);
        }
    }
}