using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MenuManagement;

namespace Freshaliens.Level.Components
{
    /// <summary>
    /// Checkpoint component to handle respawning
    /// </summary>
    [RequireComponent(typeof(BoxCollider2D))]
    public class Checkpoint : MonoBehaviour
    {
        // Singleton-like reference
        private static Checkpoint lastActiveCheckpoint = null;
        public static Checkpoint LastActiveCheckpoint => lastActiveCheckpoint;

        private Transform respawnPoint = null;
        private BoxCollider2D boxCollider = null;

        [Header("Interaction")]
        [SerializeField] private bool isStartingCheckpoint = false;
        [SerializeField] private bool isFinalCheckpoint = false;
        [SerializeField,Tooltip("Should the checkpoint be activated every time the player triggers it?")] private bool allowMultipleActivations = false;

        private bool hasBeenActivated = false;

        private void Start()
        {
            Setup();
            if (isStartingCheckpoint)
            {
                lastActiveCheckpoint = this;
#if UNITY_EDITOR
                if (isFinalCheckpoint) Debug.LogWarning("The starting checkpoint is also the final checkpoint...WTF?");
#endif
            }

            if (isFinalCheckpoint) {
                allowMultipleActivations = false;
            }
            
        }

        private void Reset()
        {
            Setup();
        }

        private void OnValidate()
        {
            Setup();
        }

        private void Setup() {
            // Collider setup
            if (!boxCollider) boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.isTrigger = true;

            // Spawn point setup
            if (!respawnPoint)
            {
                respawnPoint = new GameObject("Respawn Point").transform;
                respawnPoint.parent = this.transform;
                respawnPoint.localPosition = Vector3.zero;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (hasBeenActivated && !allowMultipleActivations) return;
            // HACK For now I'll just make a new layer that only collides with player 1

            if (isFinalCheckpoint && !hasBeenActivated) {
                hasBeenActivated = true;
                //MenuManager.Instance.levelCompletedScreen.Open(); // TODO Move this to game manager script // TODO FIX MENU MANAGER OPEN
                return;
            }

            // Not final checkpoint -> update last available checkpoint
            lastActiveCheckpoint = this;
            hasBeenActivated = true;
        }
    }
}