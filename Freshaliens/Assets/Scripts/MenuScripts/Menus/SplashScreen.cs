using System;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Object = System.Object;

namespace MenuManagement
{
    [RequireComponent(typeof(ScreenFader))]
    public class SplashScreen : MonoBehaviour
    {
        private ScreenFader _screenFader;
        [SerializeField] private Button _startButton;
        [SerializeField] private TMP_Text _subtitleText;
        
        public bool automatic = false;
        public bool subtitle = false;
        public bool dontDestroyOnLoad = false;

        [SerializeField] private float _delay = 1f;

        private void Awake()
        {
            _screenFader = GetComponent<ScreenFader>();
            InitSplashScreenComponents();
        }

        // Start is called before the first frame update
        void Start()
        {
            if (automatic)
            {
                FadeAndLoad();
            }
        }

        public void FadeAndLoad()
        {
            Debug.Log("FADE AND LOAD STARTED");
            StartCoroutine(FadeAndLoadRoutine());
        }

        private IEnumerator FadeAndLoadRoutine()
        {
            yield return new WaitForSeconds(_delay);
            _screenFader.FadeOff();
            
            // LOAD MAIN MENU LEVEL
            LevelManager.LoadMainMenuLevel();
            
            yield return new WaitForSeconds(_screenFader.FadeOnDuration);

            // destroy the splash screen 
            Destroy(gameObject);
         }

        private void InitSplashScreenComponents()
        {
            if (_startButton != null)
            {
                if (automatic)
                {
                    // disable button?
                    _startButton.enabled = false;
                }
                else
                {
                    Debug.Log("Splash Screen Button Enabled");
                    _startButton.enabled = true;
                }
            }

            if (_subtitleText != null)
            {
                if (subtitle)
                {
                    _subtitleText.enabled = true;
                }
                else
                {
                    _subtitleText.enabled = false;
                }
            }

            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
