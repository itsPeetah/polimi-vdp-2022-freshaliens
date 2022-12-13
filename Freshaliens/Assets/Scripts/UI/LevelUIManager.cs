using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freshaliens
{
    using Management;
    namespace UI
    {
        public class LevelUIManager : SingletonMonobehaviour<LevelUIManager>
        {

            [System.Serializable]
            public struct HUD {
                public LevelManager.LevelPhase displayWhen;
                public LevelUIScreen screenObject;

                public void Toggle(LevelManager.LevelPhase currentPhase) {
                    if(screenObject!=null) screenObject.SetActive((currentPhase & displayWhen) != 0);
                }
            }

            [Header("UI Screens")]
            public HUD[] screens = new HUD[] { };
            
            private void Start()
            {
                LevelManager.Instance.onLevelPhaseChange += ToggleScreens;

                ToggleScreens(LevelManager.Instance.CurrentPhase);
            }

            private void ToggleScreens(LevelManager.LevelPhase currentLevelPhase) {
                for (int i = 0; i < screens.Length; i++) {
                    screens[i].Toggle(currentLevelPhase);
                }
            }
        }
    }
}