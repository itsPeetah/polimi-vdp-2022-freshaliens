using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagement.Data
{
        
    public class DataManager : MonoBehaviour
    {
        private SaveData _saveData;
        private JsonSaver _jsonSaver; 

        public float MasterVolume
        {
            get { return _saveData.MasterVolume;  }
            set { _saveData.MasterVolume = value; }
        }
        
        public float SFXVolume
        {
            get { return _saveData.SfxVolume;  }
            set { _saveData.SfxVolume = value; }
        }
        public float MusicVolume
        {
            get { return _saveData.MusicVolume;  }
            set { _saveData.MusicVolume = value; }
        }

        private void Awake()
        {
            _saveData = new SaveData();
            _jsonSaver = new JsonSaver();
        }

        public void Save()
        {
            _jsonSaver.Save(_saveData);
        }

        public void Load()
        {
            _jsonSaver.Load(_saveData);
            Debug.Log(_saveData);
        }

        private void OnApplicationQuit()
        {
            _jsonSaver.Save(_saveData);
        }
    }

}
