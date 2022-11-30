using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freshaliens.LevelSelection
{
    /// <summary>
    /// (Scripable object) Data class for level info
    /// </summary>
    [CreateAssetMenu(menuName = "Freshaliens/Level Info", fileName = "Level Information")]
    public class LevelInfo : ScriptableObject
    {
        // TODO Add level screenshot?

        [SerializeField] private string levelName;
        [SerializeField] private string sceneName;
        [SerializeField, TextArea] private string levelDescription;

        public string Name => levelName;
        public string SceneName => sceneName;
        public string Description => levelDescription;
    }
}