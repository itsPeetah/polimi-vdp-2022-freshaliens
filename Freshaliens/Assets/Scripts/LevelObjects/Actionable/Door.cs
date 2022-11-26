using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Activable
{
    
    //State
    private GameObject _gameObject;
    private bool _isActive = false;
    
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
        gameObject.SetActive(_isActive);
    }
    
    public override void OnAction()
    {
        ToggleState();
        if (_isActive)
        {
            //animation door closes
        }
        else
        {
            //animation door opens
        }
        
    }
}