#pragma warning disable IDE1006

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freshaliens
{
    using Utility;

    namespace Management
    {
        public class LevelManager : SingletonMonobehaviour<LevelManager>
        {
            [System.Flags]
            public enum LevelPhase
            {
                Playing = 1,
                GameWon = 2,
                GameLost = 4,
                Paused = 8,
                Dialogue = 16,
            }

            [Header("Player Settings")]
            [SerializeField] private int maxPlayerHP = 3;
            [SerializeField] private int startingPlayerHP = 3;

            // Level state
            private readonly LevelPhase startingPhase = LevelPhase.Playing;
            private LevelPhase currentPhase = LevelPhase.Playing;
            private LevelPhase storedPhase = LevelPhase.Playing;
            private int currentPlayerHP = -1;
            private float currentLevelTimer = -1;

            // Properties
            public LevelPhase CurrentPhase
            {
                get => currentPhase;
                private set
                {
                    storedPhase = currentPhase;
                    currentPhase = value;
                }
            }
            public bool IsPlaying => currentPhase == LevelPhase.Playing;
            public bool IsPaused
            {
                get => currentPhase == LevelPhase.Paused;
                private set
                {
                    if (value)
                    {
                        storedPhase = currentPhase;
                        currentPhase = LevelPhase.Paused;
                    }
                    else
                    {
                        currentPhase = storedPhase;
                    }
                }
            }
            public bool IsPlayingDialogue => currentPhase == LevelPhase.Dialogue;
            public int CurrentHP { get => currentPlayerHP; private set { currentPlayerHP = Mathf.Clamp(value, 0, maxPlayerHP); } }
            public float CurrentLevelTimer => currentLevelTimer;

            public event Action<LevelPhase> onLevelPhaseChange;
            public event Action<bool> onPauseToggle;
            public event Action onPlayerDamageTaken;
            public event Action onGameWon;
            public event Action onGameLost;

            private void Start()
            {
                currentPhase = startingPhase;
                currentPlayerHP = startingPlayerHP;
                currentLevelTimer = 0;
            }

            private void Update()
            {
                if (!IsPaused)
                {
                    currentLevelTimer += Time.deltaTime;
                }
            }

            public void TogglePause()
            {
                IsPaused = !IsPaused;
                onPauseToggle?.Invoke(IsPaused);
            }

            public void DamagePlayer(int damageAmount = 1)
            {
                CurrentHP -= damageAmount;
                onPlayerDamageTaken?.Invoke();
            }

            public void TriggerGameOver(bool hasWon)
            {
                LevelPhase outcome = hasWon ? LevelPhase.GameWon : LevelPhase.GameLost;
                CurrentPhase = outcome;
                onLevelPhaseChange?.Invoke(CurrentPhase);
                (hasWon ? onGameWon : onGameLost)?.Invoke();
            }
        }
    }
}