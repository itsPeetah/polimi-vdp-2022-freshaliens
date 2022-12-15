#pragma warning disable IDE1006

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Freshaliens.UI;
namespace Freshaliens.Management
{
    public class LevelManager : SingletonMonobehaviour<LevelManager>
    {
        [Flags]
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
        public bool GameOver => currentPhase == LevelPhase.GameWon || currentPhase == LevelPhase.GameLost;
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
        public int MaxPlayerHP => maxPlayerHP;
        public int CurrentPlayerHP
        {
            get => currentPlayerHP;
            private set => currentPlayerHP = Mathf.Clamp(value, 0, maxPlayerHP);
        }
        public float CurrentLevelTimer => currentLevelTimer;
        public string CurrentLevelTimerAsString
        {
            get
            {
                int secondsAsInt = (int)currentLevelTimer;
                int minutes = secondsAsInt / 60;
                int remainingSeconds = secondsAsInt % 60;
                int centiseconds = (int)((currentLevelTimer - secondsAsInt) * 100);
                string secondsAsString = (remainingSeconds < 10 ? "0" : "") + remainingSeconds.ToString();
                string centisecondsAsString = (centiseconds < 10 ? "0" : "") + centiseconds.ToString();
                return $"{minutes}:{secondsAsString}.{centisecondsAsString}";
            }
        }

        public event Action<LevelPhase> onLevelPhaseChange;
        public event Action<bool> onPauseToggle;
        public event Action onPlayerHPChange;
        public event Action onPlayerDamageTaken;
        public event Action onGameWon;
        public event Action onGameLost;

        private void Start()
        {
            currentPhase = startingPhase;
            currentPlayerHP = startingPlayerHP;
            currentLevelTimer = 0;
            onPlayerDamageTaken += () => onPlayerHPChange?.Invoke();
            onPauseToggle += (_) => onLevelPhaseChange?.Invoke(CurrentPhase);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                TogglePause();
            }

            if (!IsPaused && !GameOver)
            {
                currentLevelTimer += Time.deltaTime;
            }

            if (Input.GetKeyDown(KeyCode.Alpha0)) TriggerGameOver(false);
        }

        public void TogglePause()
        {
            IsPaused = !IsPaused;
            Time.timeScale = IsPaused ? 0 : 1;
            onPauseToggle?.Invoke(IsPaused);
        }

        public void ReloadLevel()
        {
            SceneLoadingManager.ReloadLevel();
        }

        public void QuitLevel() {
            SceneLoadingManager.LoadLevelSelection();
        }

        public void DamagePlayer(int damageAmount = 1)
        {
            CurrentPlayerHP -= damageAmount;
            onPlayerDamageTaken?.Invoke();
        }

        public void TriggerGameOver(bool hasWon)
        {
            LevelPhase outcome = hasWon ? LevelPhase.GameWon : LevelPhase.GameLost;
            (hasWon ? onGameWon : onGameLost)?.Invoke();
            CurrentPhase = outcome;
            onLevelPhaseChange?.Invoke(CurrentPhase);
        }

        public void StartDialogue(DialoguePromptData prompt) {
            CurrentPhase = LevelPhase.Dialogue;
            DialoguePromptDisplayer.Instance.DisplayDialoguePrompt(prompt, true);
        }

        public void EndDialogue() {
            CurrentPhase = LevelPhase.Playing;
        }
    }
}