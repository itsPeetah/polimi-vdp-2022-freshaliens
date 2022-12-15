using UnityEngine;
using TMPro;

using Freshaliens.Management;

namespace Freshaliens.UI
{
    public class SpeedrunTimerHUD : UIScreen
    {
        [Header("Text Elements")]
        [SerializeField] private TextMeshProUGUI fullSessionTime = null;
        [SerializeField] private TextMeshProUGUI levelTime = null;


        private LevelManager level;

        protected override void Start()
        {
            base.Start();
            level = LevelManager.Instance;

            levelTime.gameObject.SetActive(false); // TODO Remove when implemented session time
        }

        private void Update()
        {
            // TODO Use this when implemented session time
            //fullSessionTime.SetText("not implemented yet");
            //levelTime.SetText(level.CurrentLevelTimerAsString);

            fullSessionTime.SetText(level.CurrentLevelTimerAsString);
        }
    }
}