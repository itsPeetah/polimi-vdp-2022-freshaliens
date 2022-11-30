using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles persistent save data.
/// Uses PlayerPrefs for scope reasons.
/// </summary>
public class PlayerData
{
    private const string PP_LEVEL_KEY = "LAST_LEVEL";
    private const int PP_LEVEL_DEFAULT = 0;

    private const string PP_MASTER_VOLUME_KEY = "SETTINGS:VOLUME";
    private const float PP_MASTER_VOLUME_DEFAULT = 1f;

    private const string PP_SFX_VOLUME_KEY = "SETTINGS:SFX";
    private const float PP_SFX_DEFAULT = 1.0f;

    private const string PP_MUSIC_VOLUME_KEY = "SETTINGS:MUSIC";
    private const float PP_MUSIC_DEFAULT = 1.0f;

    private static PlayerData instance;
    /// <summary>
    /// Main instance of the class. Used to access saved game data.
    /// </summary>
    public static PlayerData Instance
    {
        get {
            if (instance == null) {
                instance = Load();
            }
            return instance;
        }
    }

    // Data to be saved
    private int lastUnlockedLevel;
    private float masterVolume;
    private float sfxVolume;
    private float musicVolume;

    // Getters
    public int LastUnlockedLevel { get => lastUnlockedLevel; set => lastUnlockedLevel = value; }
    public float MasterVolume { get => masterVolume; set => masterVolume = value; }
    public float SFXVolume { get => sfxVolume; set => sfxVolume = value; }
    public float MusicVolume { get => musicVolume; set => musicVolume = value; }

    private static PlayerData Load() {
        PlayerData pd = new();
        pd.lastUnlockedLevel = PlayerPrefs.GetInt(PP_LEVEL_KEY, PP_LEVEL_DEFAULT);
        pd.masterVolume = PlayerPrefs.GetFloat(PP_MASTER_VOLUME_KEY, PP_MASTER_VOLUME_DEFAULT);
        pd.sfxVolume = PlayerPrefs.GetFloat(PP_SFX_VOLUME_KEY, PP_SFX_DEFAULT);
        pd.musicVolume = PlayerPrefs.GetFloat(PP_MUSIC_VOLUME_KEY, PP_MUSIC_DEFAULT);
        return pd;
    }

    public void Save() {
        PlayerPrefs.SetInt(PP_LEVEL_KEY, lastUnlockedLevel);
        PlayerPrefs.SetFloat(PP_MASTER_VOLUME_KEY, masterVolume);
        PlayerPrefs.SetFloat(PP_SFX_VOLUME_KEY, sfxVolume);
        PlayerPrefs.SetFloat(PP_MUSIC_VOLUME_KEY, musicVolume);
    }

    public void UnlockNextLevel(bool save=false) {
        lastUnlockedLevel += 1;
        if (save) Save();
    }
}
