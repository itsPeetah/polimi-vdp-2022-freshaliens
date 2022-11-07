using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float MasterVolume = 0f;
    public float SfxVolume = 0f;
    public float MusicVolume = 0f;
    public string hashValue;

    public SaveData()
    {
        MasterVolume = 0f;
        SfxVolume = 0f;
        MusicVolume = 0f;
    }
}
