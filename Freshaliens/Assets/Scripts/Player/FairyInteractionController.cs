using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
public class FairyInteractionController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask interactableLayers = -1;

    // State
    private Interactable storedInteractable = null;

    // Components
    private PlayerInputHandler input;

    private void Start()
    {
        input = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        if (input.GetFireInput() && storedInteractable != null) {
            storedInteractable.OnInteract();
        }
    }

    /**
     * 
     * go.layer = 7 (int)
     * lm = uint 101011 (bitmask)
     * 5 -> 000101
     * 
     * 000101 (layer = 5)
     * 100000 (layermask = 5)
     * 
     * 1 -> 000001
     * 1 << layer -> 1 << 5 -> 100000
     * 
     */

    // private bool CheckLayer(int layer)
    // {
    //     return ((1 << layer) & interactableLayers) != 0;
    // }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (CheckLayer(collision.gameObject.layer) && collision.gameObject.TryGetComponent(out Interactable interactable))
    //     {
    //         storedInteractable = interactable;
    //         interactable.OnFairyEnter();
    //     }
    // }
    //
    // private void OnTriggerStay2D(Collider2D collision)
    // {
    //     if (CheckLayer(collision.gameObject.layer) && collision.gameObject.TryGetComponent(out Interactable interactable))
    //     {
    //         interactable.OnFairyStay();
    //     }
    // }
    //
    // private void OnTriggerExit2D(Collider2D collision)
    // {
    //     if (CheckLayer(collision.gameObject.layer) && collision.gameObject.TryGetComponent(out Interactable interactable))
    //     {
    //         storedInteractable = null;
    //         interactable.OnFairyExit();
    //     }
    // }
    
    public void SetStoredInteractable(Interactable interactable, bool triggerEvents = false) {
        if (triggerEvents && interactable != storedInteractable && storedInteractable != null) storedInteractable.OnFairyExit();
        storedInteractable = interactable;
        if (triggerEvents && storedInteractable != null) interactable.OnFairyEnter();
    }
}

