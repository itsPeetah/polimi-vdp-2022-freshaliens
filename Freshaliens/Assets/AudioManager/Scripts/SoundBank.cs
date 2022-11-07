using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GameManagement
{
    
    [CreateAssetMenu(menuName = "AudioManager/SoundBank")]
    public class SoundBank : ScriptableObject
    {
        public enum SoundEffects
        {
            Explosion,
            Shot
        }
        public AudioClip[] MenuMusicLoop;

        public AudioClip[] GameMusicLoop;

        public Dictionary<SoundEffects,AudioClip> SFX = new Dictionary<SoundEffects, AudioClip>();

        public AudioClip GetMenuMusicLoop()
        {
            return MenuMusicLoop[0];
        }
        
        public AudioClip GetGameMusicLoop()
        {
            return GameMusicLoop[0];
        }

        
    }
}

