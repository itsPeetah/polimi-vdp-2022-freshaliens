using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freshaliens.LevelSelection
{
    public class LevelInfo : ScriptableObject
    {
        [SerializeField] private string levelName;
        [SerializeField, TextArea] private string levelDescription;

        public string Name => levelName;
        public string Description => levelDescription;
    }
}