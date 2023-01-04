using UnityEngine;
using UnityEngine.UI;

using Freshaliens.Management;
using Freshaliens.Player.Components;

namespace Freshaliens.UI
{
    public class GameplayHUD : UIScreen
    {
        private LevelManager level = null;

        [Header("HP Display")]
        [SerializeField] private Transform hpDisplayContainer = null;
        [SerializeField] private GameObject hpDisplayIconPrefab = null;
        private GameObject[] hpDisplayIcons = null;

        [Header("Fairy Status")]
        [SerializeField] private Image airJumpAvailableIcon = null;

        protected override void Start()
        {
            base.Start();

            level = LevelManager.Instance;

            InitHPDisplay();

            level.onPlayerHPChange += UpdateHPDisplay;
            PlayerMovementController.Instance.onJumpWhileAirborne += HideJumpAvailableIcon;
            PlayerMovementController.Instance.onLand += ShowJumpAvailableIcon;
        }

        private void InitHPDisplay() {
            int maxHP = level.MaxPlayerHP;
            hpDisplayIcons = new GameObject[maxHP];
            for (int i = 0; i < maxHP; i++) {
                GameObject go = Instantiate(hpDisplayIconPrefab, hpDisplayContainer);
                hpDisplayIcons[i] = go;
            }
        }

        private void UpdateHPDisplay( GameObject playerDamaged) {

            int currentHP = level.CurrentPlayerHP;
            for (int i = 0; i < hpDisplayIcons.Length; i++) {
                hpDisplayIcons[i].SetActive(i < currentHP);
            }
        }

        private void HideJumpAvailableIcon() => ToggleJumpAvailableIcon(false);
        private void ShowJumpAvailableIcon() => ToggleJumpAvailableIcon(true);
        private void ToggleJumpAvailableIcon(bool enabled) {
            airJumpAvailableIcon.enabled = enabled;
        }
    }
}