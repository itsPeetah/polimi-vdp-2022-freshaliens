using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected const int FAIRYLAYER = 8;
    
    protected bool CheckLayer(int layer)
    {
        return layer == FAIRYLAYER;
    }
    
    public virtual void OnInteract() { }
    
    public virtual void OnFairyEnter() { }
    
    public virtual void OnFairyExit() { }
    
    public virtual void OnFairyStay() { }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CheckLayer(collision.gameObject.layer) && collision.gameObject.TryGetComponent(out FairyInteractionController fairy))
        {
            fairy.SetStoredInteractable(this);
            OnFairyEnter();
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (CheckLayer(collision.gameObject.layer) && collision.gameObject.TryGetComponent(out FairyInteractionController fairy))
        {
            OnFairyStay();
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (CheckLayer(collision.gameObject.layer) && collision.gameObject.TryGetComponent(out FairyInteractionController fairy))
        {
            fairy.SetStoredInteractable(null);
            OnFairyExit();
        }
    }
}
