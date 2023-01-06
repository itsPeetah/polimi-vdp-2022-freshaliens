using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Freshaliens.UI
{
    public class SettingsMenu : UIScreen
    {
        public enum SettingsMenuContext {
            TitleScreen,
            GameplayHUD,
        }
        [SerializeField] private SettingsMenuContext context = SettingsMenuContext.GameplayHUD;

        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider sfxSlider;

        [SerializeField] private Button masterButton;
        [SerializeField] private Button musicButton;
        [SerializeField] private Button sfxButton;

        protected override void Start()
        {
            PlayerData pd = PlayerData.Instance;

            Debug.Log("Hey");

            masterSlider.onValueChanged.AddListener(AudioManager1.instance.SetMasterVolume);
            musicSlider.onValueChanged.AddListener(AudioManager1.instance.SetMusicVolume);
            sfxSlider.onValueChanged.AddListener(AudioManager1.instance.SetSfxVolume);

            masterButton.onClick.AddListener(() =>
            {
                PlayerData.Instance.MuteMaster = !PlayerData.Instance.MuteMaster;
                AudioManager1.instance.SetMasterVolume(PlayerData.Instance.MasterVolume);
            });
            musicButton.onClick.AddListener(() =>
            {
                PlayerData.Instance.MuteMusic = !PlayerData.Instance.MuteMusic;
                AudioManager1.instance.SetMasterVolume(PlayerData.Instance.MusicVolume);
            });
            sfxButton.onClick.AddListener(() =>
            {
                PlayerData.Instance.MuteSFX = !PlayerData.Instance.MuteSFX;
                AudioManager1.instance.SetMasterVolume(PlayerData.Instance.SFXVolume);
            });
        }

        private void OnEnable()
        {
            PlayerData pd = PlayerData.Instance;
            masterSlider.value = pd.MasterVolume;
            musicSlider.value = pd.MusicVolume;
            sfxSlider.value = pd.SFXVolume;
        }

        private void OnDisable()
        {
            PlayerData.Instance.Save();
        }


        public void OnBackPressed() {

            if (context == SettingsMenuContext.GameplayHUD)
            {
                FindObjectOfType<PauseHUD>().CloseSettings();
            }
            else
            {
                TitleScreenManager.Instance.ResetMenuView();
            }
        }
    }
}