using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Slider musicSlider, sfxSlider, masterSlider;
    
    
    private void Awake()
    {
        masterSlider.value = 1;
        musicSlider.value = 1;
        sfxSlider.value = 1;
    }

    public void ToggleMusic()
    {
     AudioManager1.instance.ToggleMusic();   
    }
    public void ToggleSfx()
    {
        AudioManager1.instance.ToggleSFX();
    }

    public void ToggleMaster()
    {
        AudioManager1.instance.ToggleMusic(); 
        AudioManager1.instance.ToggleSFX();
    }
    public void MusicVolume()
    {
        AudioManager1.instance.SetMusicVolume(musicSlider.value);
    }
    public void SfxVolume()
    {
        AudioManager1.instance.SetSfxVolume(sfxSlider.value);
    }

    public void MasterVolume()
    {
        AudioManager1.instance.SetMasterVolume(masterSlider.value);
    }

    

}
