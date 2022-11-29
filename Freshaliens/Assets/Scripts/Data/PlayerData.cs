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

    // Getters
    public int LastUnlockedLevel { get => lastUnlockedLevel; set => lastUnlockedLevel = value; } 

    private static PlayerData Load() {
        PlayerData pd = new();
        pd.lastUnlockedLevel = PlayerPrefs.GetInt(PP_LEVEL_KEY, PP_LEVEL_DEFAULT);
        return pd;
    }

    public void Save() {
        PlayerPrefs.SetInt(PP_LEVEL_KEY, lastUnlockedLevel);
    }
}
