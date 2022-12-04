using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freshaliens.Interaction.Components
{

    public class Door : Actionable
    {

        //State
        private GameObject _gameObject;
        private bool _isActive = false;
        [SerializeField] private Animator _animator;

        [SerializeField] private DoorMode _currentDoorMode;
        private enum DoorMode
        {
            NormalDoor,
            TrapDoor
        }

        void Start()
        {
            _gameObject = gameObject;
            switch (_currentDoorMode)
            {
                case DoorMode.NormalDoor:
                    _isActive = true;
                    break;
                case DoorMode.TrapDoor:
                    _isActive = false;
                    gameObject.SetActive(false);
                    break;
                default:
                    _isActive = true;
                    break;
            }

        }
        private void ToggleState()
        {
            _isActive = !_isActive;
            //gameObject.SetActive(_isActive);
        }

        public override void OnAction()
        {
           
            if (!_isActive)
            {
                //animation door closes
                Debug.Log("apro portone");
                _animator.SetBool("isDoorOpen",false);
            }
            else
            {
                Debug.Log("chiud porta");
                //animation door opens
                _animator.SetBool("isDoorOpen", true);
               // StartCoroutine(DelayAction(1));
            }
            ToggleState();
        }

    }
}