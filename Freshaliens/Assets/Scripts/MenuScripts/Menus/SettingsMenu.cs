using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameManagement.Data;
using GameManagement.Audio;

namespace MenuManagement
{
    
    public class SettingsMenu : Menu<SettingsMenu>
    {
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;

        private DataManager _dataManager; 
        
        protected override void Awake()
        {
            base.Awake();
        //    _dataManager = GameObject.FindObjectOfType<DataManager>();
        }

        private void Start()
        {
            LoadData();
        }

        public void OnMasterVolumeChanged(float value)
        {
            if (_dataManager != null)
            {
                _dataManager.MasterVolume = value;
                UpdateAudioManager();
            }
        }

        public void OnSFXVolumeChanged(float value)
        {
            if (_dataManager != null)
            {
                _dataManager.SFXVolume = value;
                UpdateAudioManager();
            }
        }

        public void OnMusicVolumeChanged(float value)
        {
            if (_dataManager != null)
            {
                _dataManager.MusicVolume = value;
                UpdateAudioManager();
            }
        }

        public void LoadData()
        {
            if (_dataManager == null || masterVolumeSlider == null ||
                sfxVolumeSlider == null || musicVolumeSlider == null)
                return;
            
            _dataManager.Load();
            
            masterVolumeSlider.value = _dataManager.MasterVolume;
            sfxVolumeSlider.value = _dataManager.SFXVolume;
            musicVolumeSlider.value = _dataManager.MusicVolume;

            UpdateAudioManager();
        }
        
        public override void OnBackPressed()
        {
            base.OnBackPressed();
            
            // saves the values to disk
            // PlayerPrefs.Save();
            _dataManager.Save();
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
