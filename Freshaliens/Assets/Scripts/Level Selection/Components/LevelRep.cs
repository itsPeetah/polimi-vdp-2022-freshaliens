using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freshaliens.LevelSelection.Components
{
    /// <summary>
    /// Physically represents levels in the level selection scene
    /// </summary>
    public class LevelRep : MonoBehaviour
    {
        [SerializeField] private LevelInfo info;
        public LevelInfo Info => info;
        [SerializeField] private bool allowLoading = true;
        [SerializeField] private bool alwaysUnlocked = false;

        public bool AllowLoading => allowLoading;
        public bool AlwaysUnlocked => alwaysUnlocked;
    }
}