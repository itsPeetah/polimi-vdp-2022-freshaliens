using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    //Parameters
    [SerializeField] private GameObject _linkedObject; //CREATE ABSTRACT CLASS "ACTIVABLE" WITH METHOD "ACTIVATE" OR SIMILAR
    
    //State
    private bool _isActive;
    
    
    private void Activate()
    {
        //_linkedObject.Activate();
        _isActive = true;
    }
    
    private void Deactivate()
    {
        //_linkedObject.Deactivate();
        _isActive = false;
    }
    
    private void ChangeState()
    {
        switch (_isActive)
        {
            case true:
                //_linkedObject.Activate();
                break;
            case false:
                //_linkedObject.Deactivate();
                break;
        }
        _isActive = !_isActive;
        
    }
}
