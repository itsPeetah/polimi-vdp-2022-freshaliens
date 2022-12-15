using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freshaliens.UI
{
    public class TitleScreenManager : SingletonMonobehaviour<TitleScreenManager>
    {
        [System.Flags]
        public enum MenuView {
            Main = 1,
            Settings = 2,
            Feedback = 4,
            Leaderboard = 8,
            Credits = 16,
            Quit = 32
        }

        [System.Serializable]
        public struct Menu {
            public MenuView displayWhen;
            public UIScreen screenObject;
            public void Toggle(MenuView currentView)
            {
                if (screenObject != null) screenObject.SetActive((currentView & displayWhen) != 0);
            }
        }

        [Header("UI Screens")]
        public Menu[] screens = new Menu[] { };

        // State
        private MenuView currentView = MenuView.Main;

        private void Start()
        {
            ToggleScreens(currentView);
        }

        private void ToggleScreens(MenuView currentView)
        {
            for (int i = 0; i < screens.Length; i++)
            {
                screens[i].Toggle(currentView);
            }
        }

        public void ResetMenuView() => SwitchView(MenuView.Main);
        public void SwitchView(MenuView newMenuView) {
            currentView = newMenuView;
            ToggleScreens(currentView);
        }
    }
}