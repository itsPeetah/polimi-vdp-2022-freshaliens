using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
public class FairyInteractionController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private LayerMask interactableLayers = -1;


    // Components
    private PlayerInputHandler input;

    private void Start()
    {
        input = GetComponent<PlayerInputHandler>();
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
    private bool CheckLayer(int layer) {
        return ((1 << layer) & interactableLayers) != 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (CheckLayer(collision.gameObject.layer)) {
            /*
             * Comp c = collision.GetComponent<Comp>();
             * if(c != null) c.OnEnter();
            */
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (CheckLayer(collision.gameObject.layer))
        {
            
            /*
             * Comp c = collision.GetComponent<Comp>();
             * if(c != null) c.OnExit();
            */
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
            if (CheckLayer(collision.gameObject.layer))
            {
                
            /*
             * Comp c = collision.GetComponent<Comp>();
             * if(c != null) c.OnStay();
            */
        }
    }
}

