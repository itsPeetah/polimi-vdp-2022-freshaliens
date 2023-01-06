using System.Collections;
using UnityEngine;

namespace Freshaliens.Interaction.Components
{

    public class Door : Actionable
    {

        //State
        private GameObject _gameObject;
        private bool _isDoorClosed = false;
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
                    _isDoorClosed = true;
                    break;
                case DoorMode.TrapDoor:
                    _isDoorClosed = false;
                    //we prefer to use the animation to make disappear the doors
                    //gameObject.SetActive(false);
                    _animator.SetBool("isDoorOpen",!_isDoorClosed);
                    break;
                default:
                    _isDoorClosed = true;
                    break;
            }
            //to get the duration of the animation (door get deactivated after the animation)
            /*m_CurrentClipInfo = _animator.GetCurrentAnimatorClipInfo(0);
      
            Debug.Log(m_CurrentClipInfo[0].clip.name + "a");
            Debug.Log(m_CurrentClipInfo[1].clip.name + "b");
            m_CurrentClipLength = m_CurrentClipInfo[1].clip.length;*/
        }
        private void ToggleState()
        {
            _isDoorClosed = !_isDoorClosed;
           // gameObject.SetActive(_isDoorClosed);
        }

        public override void OnAction()
        {
            if (!_isDoorClosed)
            {
                //animation door closes
               

                _animator.SetBool("isDoorOpen",_isDoorClosed);
            }
            else
            {
            
                //animation door opens
                _animator.SetBool("isDoorOpen", _isDoorClosed);
               // StartCoroutine(Wait1Second());
               
                
                
            }
            ToggleState();
            
        }

        /*
        IEnumerator  Wait1Second()
        {  //Print the time of when the function is first called.
            while (true)
            {

                Debug.Log(m_CurrentClipLength);
                yield return new WaitForSeconds(m_CurrentClipLength );
                ToggleState();
                yield return null;
            }
        }
        */

    }
}