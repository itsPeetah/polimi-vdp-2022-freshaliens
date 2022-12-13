using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Freshaliens.Management;

namespace Freshaliens.UI
{
    public class SpeedrunTimerHUD : LevelUIScreen
    {
        [Header("Text Elements")]
        [SerializeField] private TextMeshProUGUI fullSessionTime = null;
        [SerializeField] private TextMeshProUGUI levelTime = null;


        private LevelManager level;

        protected override void Start()
        {
            base.Start();
            level = LevelManager.Instance;
        }

        private void Update()
        {
            fullSessionTime.SetText("not implemented yet");
            levelTime.SetText(level.CurrentLevelTimerAsString);
        }
    }
}