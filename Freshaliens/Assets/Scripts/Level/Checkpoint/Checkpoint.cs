using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        [SerializeField,Tooltip("Should the checkpoint be activated every time the player triggers it?")] private bool allowMultipleActivations = false;

        private bool hasBeenActivated = false;

        private void Start()
        {
            Setup();
            if (isStartingCheckpoint) {
                lastActiveCheckpoint = this;
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

        private void OnTriggerEnter(Collider other)
        {
            if (hasBeenActivated && !allowMultipleActivations) return;
            // HACK For now I'll just make a new layer that only collides with player 1
            lastActiveCheckpoint = this;
            hasBeenActivated = true;
        }
    }
}