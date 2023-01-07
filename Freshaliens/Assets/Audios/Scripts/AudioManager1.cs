using System;
using System.Collections.Generic;
using Freshaliens.Player.Components;
using Freshaliens.UI;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager1 : MonoBehaviour
{
    public static AudioManager1 instance;
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource, sfxSource;
    private Dictionary<string, AudioClip> sfxClips;
    private Dictionary<string, AudioClip> musicClips;
    
    private void Awake()
    {
        instance = this;
        InitializeClipDictionaries();
    }

    private void Start()
    {
        
        if (PlayerMovementController.Exists)
        {
            PlayerMovementController player = PlayerMovementController.Instance;
            player.onJumpWhileGrounded += () => PlaySFX("jump");
            player.onJumpWhileAirborne += () => PlaySFX("jump");
            player.onStep += () => PlaySFX("step");
        }

        UIScreen.onButtonClick += () => PlaySFX("button");

        //sfxSource.enabled = true;
        //musicSource.enabled = true;
        //sfxSource.gameObject.SetActive(true);
        //musicSource.gameObject.SetActive(true);

        PlayerData pd = PlayerData.Instance;
        SetMasterVolume(pd.MasterVolume);
        SetMusicVolume(pd.MusicVolume);
        SetSfxVolume(pd.SFXVolume);


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


    public void SetMasterVolume(float volume)
    {
        Debug.Log("Master");

        PlayerData.Instance.MasterVolume = volume;

        //if (useMixer)
        //{
        //    // volume for AudioSource is between 0 and 1, 
        //    // it is from -80 to -20 in the mixer and must be normalized

        //    float mixerVolume = SliderToDB(volume, MasterMaxDb);
        //    audioMixer.SetFloat("MasterVolume", mixerVolume);
        //}

        SetMusicVolume(PlayerData.Instance.MusicVolume);
        SetMusicVolume(PlayerData.Instance.SFXVolume);
    }

    public void SetMusicVolume(float volume)
    {
        PlayerData.Instance.MusicVolume = volume;
        volume = PlayerData.Instance.MuteMusic || PlayerData.Instance.MuteMaster ? 0 : volume * PlayerData.Instance.MasterVolume;
        volume = Mathf.Clamp(volume, 0f, 1f);
        musicSource.volume = volume;
    }

    public void SetSfxVolume(float volume)
    {
        PlayerData.Instance.SFXVolume = volume;
        volume = PlayerData.Instance.MuteSFX || PlayerData.Instance.MuteMaster ? 0 : volume * PlayerData.Instance.MasterVolume;

        //if (useMixer)
        //{
        //    float mixerVolume = SliderToDB(volume, SfxMaxDb);
        //    audioMixer.SetFloat("SFXVolume", mixerVolume);
        //}
        //else
        //{
        //}
            volume = Mathf.Clamp(volume, 0f, 1f);
            sfxSource.volume = volume;
    }

    public void ToggleMaster() {
        PlayerData.Instance.MuteMaster = !PlayerData.Instance.MuteMaster;
    }

    public void ToggleMusic()
    {
        PlayerData.Instance.MuteMusic = !PlayerData.Instance.MuteMusic;
        //musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        PlayerData.Instance.MuteSFX = !PlayerData.Instance.MuteSFX;
        //sfxSource.mute = !sfxSource.mute;
    }
}
