using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    [SerializeField] protected bool storable = true;
    public bool ShouldBeStored => storable;
    
    public virtual void OnInteract() { }
    
    public virtual void OnFairyEnter() { }
    
    public virtual void OnFairyExit() { }
    
    public virtual void OnFairyStay() { }
    
    
}
