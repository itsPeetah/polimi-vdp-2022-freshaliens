#pragma warning disable IDE1006

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Freshaliens.UI;
using Freshaliens.Level.Components;
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

        [Header("Level Settings")]
        [SerializeField] private Checkpoint startingCheckpoint = null;
        [SerializeField] private Checkpoint finalCheckpoint = null;


        // Level state
        private readonly LevelPhase startingPhase = LevelPhase.Playing;
        private LevelPhase currentPhase = LevelPhase.Playing;
        private LevelPhase storedPhase = LevelPhase.Playing;
        private int currentPlayerHP = -1;
        private float currentLevelTimer = -1;
        private Checkpoint latestCheckpoint = null;

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
        public string CurrentLevelTimerAsString => FloatTimeToString.Convert(currentLevelTimer);
        public Vector3 PlayerRespawnPosition => latestCheckpoint.RespawnPosition;

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

            if (finalCheckpoint) finalCheckpoint.MarkAsFinal();
            latestCheckpoint = startingCheckpoint;

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

        public void UnlockCheckpoint(Checkpoint checkpoint) {
            latestCheckpoint = checkpoint;
        }

        public void DamagePlayer(int damageAmount = 1)
        {
            CurrentPlayerHP -= damageAmount;
            onPlayerDamageTaken?.Invoke();
        }

        public void RespawnPlayer()
        {

        }

        public void TriggerGameOver(bool hasWon)
        {
            if (hasWon)
            {
                PlayerData.Instance.SaveLevelTime(PlayerData.Instance.LastLevelSelected, currentLevelTimer);
                PlayerData.Instance.UnlockNextLevel(true);
                CurrentPhase = LevelPhase.GameWon;
                onGameWon?.Invoke();
            }
            else
            {
                CurrentPhase = LevelPhase.GameLost;
                onGameLost?.Invoke();
            }

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