using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Class that handles persistent save data.
/// Uses PlayerPrefs for scope reasons.
/// </summary>
public class PlayerData
{
    private const string PP_LEVEL_KEY = "LAST_LEVEL";
    private const int PP_LEVEL_DEFAULT = 1;

    private const string PP_MASTER_VOLUME_KEY = "SETTINGS:VOLUME";
    private const float PP_MASTER_VOLUME_DEFAULT = 1f;

    private const string PP_SFX_VOLUME_KEY = "SETTINGS:SFX";
    private const float PP_SFX_DEFAULT = 1.0f;

    private const string PP_MUSIC_VOLUME_KEY = "SETTINGS:MUSIC";
    private const float PP_MUSIC_DEFAULT = 1.0f;

    private const string PP_LEVEL_TIME_BASE_KEY = "LEADERBOARD:LEVEL_";
    private const float PP_LEVEL_TIME_DEFAULT = -1.0f;

    private const string PP_LEADERBOARD_NAME = "LEADERBOARD:NAME";
    private const string PP_LEADERBOARD_NAME_DEFAULT = "";

    private static PlayerData instance;
    /// <summary>
    /// Main instance of the class. Used to access saved game data.
    /// </summary>
    public static PlayerData Instance
    {
        get
        {
            if (instance == null)
            {
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
    private string leaderboardName;

    // Session data (no need to save this)
    private int lastLevelChosen;

    // Getters
    public int LastUnlockedLevel { get => lastUnlockedLevel; set => lastUnlockedLevel = value; }
    public float MasterVolume { get => masterVolume; set => masterVolume = value; }
    public float SFXVolume { get => sfxVolume; set => sfxVolume = value; }
    public float MusicVolume { get => musicVolume; set => musicVolume = value; }
    public string LeaderboardName { get => leaderboardName; private set => leaderboardName = value; }

    public int LastLevelSelected { get => lastLevelChosen; set => lastLevelChosen = value; }
    public bool HasPlayedBefore => lastUnlockedLevel > PP_LEVEL_DEFAULT;

    private static PlayerData Load()
    {
        PlayerData pd = new();
        // Save data
        pd.lastUnlockedLevel = PlayerPrefs.GetInt(PP_LEVEL_KEY, PP_LEVEL_DEFAULT);
        pd.masterVolume = PlayerPrefs.GetFloat(PP_MASTER_VOLUME_KEY, PP_MASTER_VOLUME_DEFAULT);
        pd.sfxVolume = PlayerPrefs.GetFloat(PP_SFX_VOLUME_KEY, PP_SFX_DEFAULT);
        pd.musicVolume = PlayerPrefs.GetFloat(PP_MUSIC_VOLUME_KEY, PP_MUSIC_DEFAULT);
        pd.leaderboardName = PlayerPrefs.GetString(PP_LEADERBOARD_NAME, PP_LEADERBOARD_NAME_DEFAULT);

        if (pd.leaderboardName.Length <= 1) {
            pd.leaderboardName = "Anonymous Player #" + UnityEngine.Random.Range(100, 1000).ToString();
        }

        // Session data
        pd.lastLevelChosen = pd.lastUnlockedLevel;
        return pd;
    }

    public void Save()
    {
        PlayerPrefs.SetInt(PP_LEVEL_KEY, lastUnlockedLevel);
        PlayerPrefs.SetFloat(PP_MASTER_VOLUME_KEY, masterVolume);
        PlayerPrefs.SetFloat(PP_SFX_VOLUME_KEY, sfxVolume);
        PlayerPrefs.SetFloat(PP_MUSIC_VOLUME_KEY, musicVolume);
        PlayerPrefs.SetString(PP_LEADERBOARD_NAME, leaderboardName);
    }

    public static void ForceSaveToDisk()
    {
        Instance.Save();
        PlayerPrefs.Save();
    }

    public void UnlockNextLevel(bool save = false)
    {
        lastUnlockedLevel = lastLevelChosen + 1;
        if (save) Save();
    }

    public void GetLevelTime(int level) {
        string k = PP_LEVEL_TIME_BASE_KEY + level.ToString();
        PlayerPrefs.GetFloat(k, PP_LEVEL_TIME_DEFAULT);
    }

    public void SaveLevelTime(int level, float time) {
        string k = PP_LEVEL_TIME_BASE_KEY + level.ToString();
        PlayerPrefs.SetFloat(k, time);
    }

    public void GenerateName(string name)
    {
        //int random = UnityEngine.Random.Range(1000, 10000);

        //int idx = name.IndexOf('#');
        //if (idx >= 0) name = name.Substring(0, name.Length - idx);

        //name = $"{name}#{random}";
        LeaderboardName = name;
        Save();
    }

#if UNITY_EDITOR
    [MenuItem("Freshaliens/Data/Erase Player Prefs")]
    public static void ErasePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
#endif
}
