using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    private bool _isActive;
    

    private void Activate()
    {
        _isActive = true;
    }
    
    private void Disactivate()
    {
        _isActive = false;
    }
    
    private void ChangeState()
    {
        _isActive = !_isActive;
    }
}
