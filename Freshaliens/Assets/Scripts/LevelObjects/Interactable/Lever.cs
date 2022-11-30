using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Freshaliens.Interaction.Components
{

    // [RequireComponent(typeof(Activable))]
    public class Lever : Interactable
    {
        //Parameters
        [SerializeField] private Actionable _linkedObject;

        // //State                                                          ALL 
        // private bool _isActive;                                          THIS 
        //                                                                  STUFF 
        //                                                                  IS 
        // private void Activate()                                          ACTUALLY 
        // {                                                                USELESS
        //     //_linkedObject.Activate();
        //     _isActive = true;
        // }
        //
        // private void Deactivate()
        // {
        //     //_linkedObject.Deactivate();
        //     _isActive = false;
        // }
        //
        // private void ChangeState()
        // {
        //     switch (_isActive)
        //     {
        //         case true:
        //             //_linkedObject.Activate();
        //             break;
        //         case false:
        //             //_linkedObject.Deactivate();
        //             break;
        //     }
        //     _isActive = !_isActive;
        //     
        // }

        public override void OnInteract()
        {
            _linkedObject.OnAction();
        }
    }
}