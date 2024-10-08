using System;
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
        
        public event Action<bool> onInteract;
        //animation for the shining
        //private Animator _animator;
        // State
        private Interactable storedInteractable = null;

        // Components
        private PlayerInputHandler input;

        private void Start()
        {
            input = GetComponent<PlayerInputHandler>();
         //   _animator = GetComponent<Animator>();
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
            if (!collision.gameObject.TryGetComponent(out interactable))
            {
                //Quedta roba fa schifo ma risolve l'edge case che abbiamo
                interactable = collision.GetComponentInParent<Interactable>();
            }
            if (CheckLayer(collision.gameObject.layer))
                return true;
            return false;
        }


        /// <summary>
        /// Check whether the layer is contained within the interactable layers mask.
        /// </summary>
        private bool CheckLayer(int layer)
        {
            return ((1 << layer) & interactableLayers) != 0;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (CheckInteractable(collision, out Interactable interactable))
            {
                if (interactable.ShouldBeStored)
                {   
                    storedInteractable = interactable;
                }
                //check if can change animation
                if (!collision.CompareTag( "Player" ) || (collision.CompareTag( "Player" ) && collision.gameObject.GetComponent<PlayerMovementController>().RemainingAirJupms > 0))
                {
                    onInteract?.Invoke(true);
                    //_animator.SetBool("canLight", true);
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
         
                //end light animation
               // _animator.SetBool("canLight",false);
                onInteract?.Invoke(false);
                if (interactable.ShouldBeStored)
                {
                    storedInteractable = null;
                }
                interactable.OnFairyExit();
            }
        }
    }

}