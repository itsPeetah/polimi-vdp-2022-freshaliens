using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Freshaliens.Interaction;

namespace Freshaliens.Player.Components
{
    /// <summary>
    /// Component that handles the interaction between the fairy character and the world.
    /// </summary>
    [RequireComponent(typeof(PlayerInputHandler))]
    public class FairyInteractionController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private LayerMask interactableLayers = -1;
        //Sprite for the shining
        private SpriteRenderer shining;
        // State
        private Interactable storedInteractable = null;

        // Components
        private PlayerInputHandler input;

        private void Start()
        {
            input = GetComponent<PlayerInputHandler>();
            shining = this.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
            shining.enabled = false;

        }

        private void Update()
        {
            if (input.GetActionInput() && storedInteractable != null)
            {
                storedInteractable.OnInteract(); // TODO Change to event + listener in Interactable?
            }
        }

        /// <summary>
        /// Check if the collision was with an interactable object
        /// </summary>
        /// <param name="collision">Collider detected by the trigger</param>
        /// <param name="interactable">Interactable component on the colliding object</param>
        /// <returns>Whether the collision is to be considered by the interaction system</returns>
        private bool CheckInteractable(Collider2D collision, out Interactable interactable)
        {
            collision.gameObject.TryGetComponent(out interactable);
            if (CheckLayer(collision.gameObject.layer))
                return true;
            return false;
        }


        /// <summary>
        /// Check whether the layer is contained within the interactable layers mask.
        /// </summary>
        private bool CheckLayer(int layer)
        {
            /**
             * Flag explanation
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
            return ((1 << layer) & interactableLayers) != 0;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (CheckInteractable(collision, out Interactable interactable))
            {
   
                //visual effect
                shining.enabled = true;
                if (interactable.ShouldBeStored)
                {
                    storedInteractable = interactable;
                 
                }
                interactable.OnFairyEnter();
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            
            if (CheckInteractable(collision, out Interactable interactable))
            {
                interactable.OnFairyStay();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (CheckInteractable(collision, out Interactable interactable))
            {
                //visual effect
                shining.enabled = false;
                Debug.Log("lascio fatina");
                if (interactable.ShouldBeStored)
                {
                    storedInteractable = null;
                    
                }
                interactable.OnFairyExit();
            }
        }
    }

}