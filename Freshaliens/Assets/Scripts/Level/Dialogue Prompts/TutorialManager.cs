using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Freshaliens.Player.Components;
using Freshaliens.Management;
using Object = UnityEngine.Object;

namespace Freshaliens.UI
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private DialoguePromptData dialogueStartTutorial;
        [SerializeField] private DialoguePromptData dialogueCompletedTutorial;
        [SerializeField] private NeededEvent _neededEvent;
        private enum NeededEvent
        {
            Nothing,
            onJumpWhileGrounded,
            onJumpWhileAirborne,
        }

        private void StartTutorial()
        {
            Destroy(GetComponent<BoxCollider2D>());
            LevelManager.Instance.StartDialogue(dialogueStartTutorial);
            AddEventListener();
        }
        
        private void CompletedTutorial()
        {
            if(dialogueCompletedTutorial)
                LevelManager.Instance.StartDialogue(dialogueCompletedTutorial);
            RemoveEventListener();
            // Destroy(gameObject);
        }
        
        private void AddEventListener()
        {
            switch (_neededEvent)
            {
                case NeededEvent.onJumpWhileGrounded:
                    PlayerMovementController.Instance.onJumpWhileGrounded += CompletedTutorial;
                    break;
                case NeededEvent.onJumpWhileAirborne:
                    PlayerMovementController.Instance.onJumpWhileAirborne += CompletedTutorial;
                    break;
            }
        }
        
        private void RemoveEventListener()
        {
            switch (_neededEvent)
            {
                case NeededEvent.onJumpWhileGrounded:
                    PlayerMovementController.Instance.onJumpWhileGrounded -= CompletedTutorial;
                    break;
                case NeededEvent.onJumpWhileAirborne:
                    PlayerMovementController.Instance.onJumpWhileAirborne -= CompletedTutorial;
                    break;
            }
        }
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            StartTutorial();
        }
    }
}
