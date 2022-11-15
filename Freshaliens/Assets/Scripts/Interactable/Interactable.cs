using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    //State
    protected bool _isActive;
    protected GameObject _gameObject;
    

    protected void Activate()
    {
        _isActive = true;
    }
    protected void Disactivate()
    {
        _isActive = false;
    }
    
    protected void ChangeState()
    {
        _isActive = !_isActive;
    }
    
    protected void ChangeLayer(string newLayerName)
    {
        int newLayer = LayerMask.NameToLayer(newLayerName);
        _gameObject.layer = newLayer;
    }

    public abstract void OnInteract();

    public abstract void OnFatinaEnter();
    
    public abstract void OnFatinaExit();
    

}
