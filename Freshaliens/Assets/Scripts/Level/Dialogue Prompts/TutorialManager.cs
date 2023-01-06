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
        [SerializeField] private DialoguePromptData[] dialoguePrompts;
        private Object box;
        private enum State
        {
            START,
            MUSTJUMP,
            MUSTJUMPONFAIRY,
            FINISHED
        }
        private State _currentState;

        private void Start()
        {
            _currentState = State.START;
            box = GetComponent<BoxCollider2D>();
        }
        
        private void NextTutorial()
        {
            StartDialog();
            ChangeEventListener();
            _currentState += 1;
        
            if (_currentState == State.FINISHED)
            {
                Destroy(gameObject);
            }
        }
        // private void NextTutorial()
        // {
        //     StartCoroutine(OnNextTutorial());
        // }
        //
        // private IEnumerator OnNextTutorial()
        // {
        //     // yield return new WaitForSeconds(1);
        //     StartDialog();
        //     ChangeEventListener();
        //     _currentState += 1;
        //
        //     if (_currentState == State.FINISHED)
        //     {
        //         Destroy(gameObject);
        //     }
        //
        //     yield return null;
        // }
        
        private void StartDialog()
        {
            LevelManager.Instance.StartDialogue(dialoguePrompts[(int)_currentState]);
        }
        
        private void ChangeEventListener()
        {
            switch (_currentState)
            {
                case State.START:
                    PlayerMovementController.Instance.onJumpWhileGrounded += NextTutorial;
                    break;
                case State.MUSTJUMP:
                    PlayerMovementController.Instance.onJumpWhileGrounded -= NextTutorial;
                    PlayerMovementController.Instance.onJumpWhileAirborne += NextTutorial;
                    break;
                case State.MUSTJUMPONFAIRY:
                    PlayerMovementController.Instance.onJumpWhileAirborne -= NextTutorial;
                    break;
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            NextTutorial();
            Destroy(box);
        }
    }
}
