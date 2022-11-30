using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameManagement.Audio;

namespace MenuManagement
{

    public class SettingsMenu : Menu<SettingsMenu>
    {
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            LoadData();
        }

        public void OnMasterVolumeChanged(float value)
        {
            PlayerData.Instance.MasterVolume = value;
            UpdateAudioManager();

        }

        public void OnSFXVolumeChanged(float value)
        {

            PlayerData.Instance.SFXVolume = value;
            UpdateAudioManager();

        }

        public void OnMusicVolumeChanged(float value)
        {

            PlayerData.Instance.MusicVolume = value;
            UpdateAudioManager();

        }

        public void LoadData()
        {
            if (masterVolumeSlider == null ||
                sfxVolumeSlider == null || musicVolumeSlider == null)
                return;

            masterVolumeSlider.value = PlayerData.Instance.MasterVolume;
            sfxVolumeSlider.value = PlayerData.Instance.SFXVolume;
            musicVolumeSlider.value = PlayerData.Instance.MusicVolume;

            UpdateAudioManager();
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();

            // saves the values to disk

            PlayerData.Instance.Save();
        }

        public void UpdateAudioManager()
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.SetMasterVolume(masterVolumeSlider.value);
                AudioManager.Instance.SetMusicVolume(musicVolumeSlider.value);
                AudioManager.Instance.SetSfxVolume(sfxVolumeSlider.value);
            }
        }
    }

}
