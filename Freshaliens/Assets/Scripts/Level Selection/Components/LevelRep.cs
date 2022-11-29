using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freshaliens.LevelSelection.Components
{
    public class LevelRep : MonoBehaviour
    {
        [SerializeField] private LevelInfo info;
        public LevelInfo Info => info;
    }
}