using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Class that handles persistent save data.
/// Uses PlayerPrefs for scope reasons.
/// </summary>
public class PlayerData
{
    public const int MAX_LEVEL = 6;

    private const string PP_LEVEL_KEY = "NEW:LAST_LEVEL";
    private const int PP_LEVEL_DEFAULT = 0;

    private const string PP_MASTER_VOLUME_KEY = "SETTINGS:VOLUME";
    private const float PP_MASTER_VOLUME_DEFAULT = 0.5f;

    private const string PP_SFX_VOLUME_KEY = "SETTINGS:SFX";
    private const float PP_SFX_DEFAULT = 0.5f;

    private const string PP_MUSIC_VOLUME_KEY = "SETTINGS:MUSIC";
    private const float PP_MUSIC_DEFAULT = 0.5f;

    private const string PP_MASTER_MUTE_KEY = "SETTINGS:VOLUME_MUTE";
    private const int PP_MASTER_MUTE_DEFAULT = 0;

    private const string PP_MUSIC_MUTE_KEY = "SETTINGS:MUSIC_MUTE";
    private const int PP_MUSIC_MUTE_DEFAULT = 0;

    private const string PP_SFX_MUTE_KEY = "SETTINGS:SFX_MUTE";
    private const int PP_SFX_MUTE_DEFAULT = 0;

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
    private bool masterVolumeMuted;
    private bool musicVolumeMuted;
    private bool sfxVolumeMuted;

    // Session data (no need to save this)
    private int lastLevelChosen;

    // Getters
    public int LastUnlockedLevel { get => lastUnlockedLevel; set => lastUnlockedLevel = value; }
    public float MasterVolume { get => masterVolume; set => masterVolume = value; }
    public float SFXVolume { get => sfxVolume; set => sfxVolume = value; }
    public float MusicVolume { get => musicVolume; set => musicVolume = value; }
    public string LeaderboardName { get => leaderboardName; set => leaderboardName = value; }
    public bool MuteMaster { get => masterVolumeMuted ; set => masterVolumeMuted = value; }
    public bool MuteMusic { get => musicVolumeMuted; set => musicVolumeMuted = value; }
    public bool MuteSFX { get => sfxVolumeMuted; set => sfxVolumeMuted = value; }

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
        pd.masterVolumeMuted = PlayerPrefs.GetInt(PP_MASTER_MUTE_KEY, PP_MASTER_MUTE_DEFAULT) > 0;
        pd.musicVolumeMuted = PlayerPrefs.GetInt(PP_MUSIC_MUTE_KEY, PP_MUSIC_MUTE_DEFAULT) > 0; ;
        pd.sfxVolumeMuted = PlayerPrefs.GetInt(PP_SFX_MUTE_KEY, PP_SFX_MUTE_DEFAULT) > 0; ;

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
        PlayerPrefs.SetInt(PP_MASTER_MUTE_KEY, masterVolumeMuted ? 1 : 0);
        PlayerPrefs.SetInt(PP_MUSIC_MUTE_KEY, musicVolumeMuted ? 1 : 0);
        PlayerPrefs.SetInt(PP_SFX_MUTE_KEY, sfxVolumeMuted ? 1 : 0);

    }

    public static void ForceSaveToDisk()
    {
        Instance.Save();
        PlayerPrefs.Save();
    }

    public void UnlockNextLevel(bool save = false)
    {
        lastUnlockedLevel = Mathf.Clamp(lastLevelChosen + 1, PP_LEVEL_DEFAULT, MAX_LEVEL);
        
        /*if (save)*/Save();
    }

    public void GetLevelTime(int level) {
        string k = PP_LEVEL_TIME_BASE_KEY + level.ToString();
        PlayerPrefs.GetFloat(k, PP_LEVEL_TIME_DEFAULT);
    }

    public void SaveLevelTime(int level, float time) {
        string k = PP_LEVEL_TIME_BASE_KEY + level.ToString();
        PlayerPrefs.SetFloat(k, time);
    }


#if UNITY_EDITOR
    [MenuItem("Freshaliens/Data/Erase Player Prefs")]
    public static void ErasePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
#endif
}
