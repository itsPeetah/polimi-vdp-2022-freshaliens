using System;
using System.Collections;
using System.Collections.Generic;
using Freshaliens;
using Freshaliens.Player.Components;
using Freshaliens.UI;
using GameManagement.Audio;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager1 : MonoBehaviour
{   
    [SerializeField] private AudioMixer audioMixer; 
    public static AudioManager1 instance;
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;
    private bool useMixer = true;
    private Dictionary<String, AudioClip> sfxClips;
    private Dictionary<String, AudioClip> musicClips;


    [Range(-80f,20f)]
    public float SfxMaxDb = 0f;

    [Range(-80f,20f)]
    public float MusicMaxDb = 0f;

    [Range(-80f,20f)]
    public float MasterMaxDb = 0f;

    private void Awake()
    {
        
        instance = this;
        InitializeClipDictionaries();
    }

    private void Start()
    {
        
        PlayerMovementController player = PlayerMovementController.Instance;
        if (player != null)
        {
            player.onJumpWhileGrounded += () => PlaySFX("jump");
            player.onStep += () => PlaySFX("step");
            player.onJumpWhileAirborne += () => PlaySFX("jump");
        }

        UIScreen.onButtonClick += () => PlaySFX("button");
        
        //sfxSource.enabled = true;
        //musicSource.enabled = true;
        //sfxSource.gameObject.SetActive(true);
        //musicSource.gameObject.SetActive(true);

    }

    void InitializeClipDictionaries()
    {
        sfxClips = new Dictionary<string, AudioClip>();
        musicClips = new Dictionary<string, AudioClip>();

        foreach (Sound sound in musicSounds)
        {
            musicClips.Add(sound.name, sound.clip);
        }
        
        foreach (Sound sound in sfxSounds)
        {
            sfxClips.Add(sound.name, sound.clip);
        }
        
            
    }

    public void PlayMusic(string name)
    {
        if (musicClips.TryGetValue(name, out AudioClip clip))
        {
            musicSource.clip = clip;
            musicSource.Play();
        }

    }

    public void PlaySFX(string name)
    {
        
        if (sfxClips.TryGetValue(name, out AudioClip clip))
        {
            sfxSource.PlayOneShot(clip);
        }
        
    }
    
    public static float SliderToDB(float volume, float maxDB=0, float minDB=-80)
    {
        float dbRange = maxDB - minDB;
        return minDB + volume * dbRange;
    }

    
    
    public void SetSfxVolume(float volume)
    {
        if (useMixer)
        {
            float mixerVolume = AudioManager.SliderToDB(volume, SfxMaxDb);
            audioMixer.SetFloat ("SFXVolume", mixerVolume);
            PlayerData.Instance.SFXVolume = mixerVolume;
        }
        else
        {
            volume = Mathf.Clamp(volume, 0f, 1f);
            sfxSource.volume = volume;
        }
    }
    
    public void SetMusicVolume(float volume)
    {
        if (useMixer)
        {
            
            float mixerVolume = AudioManager.SliderToDB(volume, MusicMaxDb);
            audioMixer.SetFloat ("MusicVolume", mixerVolume);
            PlayerData.Instance.MusicVolume = mixerVolume;
        }
        else
        {
            
            volume = Mathf.Clamp(volume, 0f, 1f);
            musicSource.volume = volume;
        }
    }
    
    public void SetMasterVolume(float volume)
    {
        if (useMixer)
        {
            // volume for AudioSource is between 0 and 1, 
            // it is from -80 to -20 in the mixer and must be normalized
            
            float mixerVolume = AudioManager.SliderToDB(volume, MasterMaxDb);
            audioMixer.SetFloat ("MasterVolume", mixerVolume);
            PlayerData.Instance.MasterVolume = mixerVolume;

        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

}
