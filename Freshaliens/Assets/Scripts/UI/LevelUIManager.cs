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
            public struct LevelUIScreen {
                public LevelManager.LevelPhase displayWhen;
                public GameObject root;

                public void Toggle(LevelManager.LevelPhase currentPhase) {
                    root.SetActive((currentPhase | displayWhen) != 0);
                }
            }

            [Header("UI Screens")]
            public LevelUIScreen[] screens = new LevelUIScreen[] { };
            
            private void Start()
            {
                LevelManager.Instance.onLevelPhaseChange += ToggleScreens;
            }

            private void ToggleScreens(LevelManager.LevelPhase currentLevelPhase) {
                for (int i = 0; i < screens.Length; i++) {
                    screens[i].Toggle(currentLevelPhase);
                }
            }
        }
    }
}