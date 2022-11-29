using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Freshaliens.LevelSelection.Components
{
    public class LevelSelectionUI : MonoBehaviour
    {
        [Header("Current Level Panel")]
        [SerializeField] private Transform currentLevelPanel = null;
        [SerializeField] private TextMeshProUGUI currentLevelLabel = null;
        [SerializeField] private float panelScalingDuration = 0.3f;

        public void UpdateLevelInfoDisplay(LevelInfo info) {
            StartCoroutine(nameof(ScaleCurrentLevelPanel_Coroutine));
            currentLevelLabel.SetText(info.Name);
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
    }
}