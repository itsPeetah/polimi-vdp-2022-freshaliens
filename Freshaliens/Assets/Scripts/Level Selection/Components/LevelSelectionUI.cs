using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace Freshaliens.LevelSelection.Components
{
    /// <summary>
    /// UI Manager for the level selection scene
    /// </summary>
    public class LevelSelectionUI : MonoBehaviour
    {
        [Header("Current Level Panel")]
        [SerializeField] private Transform currentLevelPanel = null;
        [SerializeField] private TextMeshProUGUI currentLevelLabel = null;
        [SerializeField] private TextMeshProUGUI currentLevelDescriptionLabel = null;
        [SerializeField] private float panelScalingDuration = 0.3f;

        [Header("Screen fader")]
        [SerializeField] private Image screenFader = null;
        [SerializeField] private float screenFadeDuration = 1f;

        private void Start()
        {
            FadeScreen(1, 0);
        }

        /// <summary>
        /// Update the information displayed for the currently selected level
        /// </summary>
        public void UpdateLevelInfoDisplay(LevelInfo info) {
            StopCoroutine(nameof(ScaleCurrentLevelPanel_Coroutine));
            StartCoroutine(nameof(ScaleCurrentLevelPanel_Coroutine));
            currentLevelLabel.SetText(info.Name);
            currentLevelDescriptionLabel.SetText(info.Description);
        }

        private IEnumerator ScaleCurrentLevelPanel_Coroutine() {

            float t = 0;
            while (t < panelScalingDuration) {
                currentLevelPanel.localScale = (t / panelScalingDuration) * Vector3.one;
                t += Time.deltaTime;
                yield return null;
            }
            currentLevelPanel.localScale = Vector3.one;

        }

        private void SetFaderAlpha(float a) {
            Color c = screenFader.color;
            c.a = a;
            screenFader.color = c;
        }

        /// <summary>
        /// Fade the screen
        /// </summary>
        /// <param name="from">Alpha value to start fading from</param>
        /// <param name="to">alpha value to fade to</param>
        /// <param name="onFinished">Callback to invoke when finished fading</param>
        public void FadeScreen(float from = 0, float to=1, Action onFinished=null) {
            StopAllCoroutines();
            StartCoroutine(FadeScreen_Coroutine( from, to, onFinished));
        }

        private IEnumerator FadeScreen_Coroutine(float from, float to, Action onFinished) {
            float t = 0;
            SetFaderAlpha(from);
            float a = from;
            while (t < screenFadeDuration) {

                a = Mathf.Lerp(from, to, t / screenFadeDuration);
                SetFaderAlpha(a);

                t += Time.deltaTime;
                yield return null;
            }
            SetFaderAlpha(to);
            onFinished?.Invoke();
        }
    }
}