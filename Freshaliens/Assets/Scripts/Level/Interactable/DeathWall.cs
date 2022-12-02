using UnityEngine;


namespace Freshaliens.Interaction
{
    public class DeathWall : Interactable
    {
        [Header("Layers Settings")]
        [SerializeField] private int _ninjaLayer = 7;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.CompareTag("Player"))
            {
                return;
            }

            GameObject playerObject = collider.gameObject;
            int colliderLayer = playerObject.layer;
            LivesManager livesManager = playerObject.GetComponent<LivesManager>();
            
            
            if (colliderLayer==_ninjaLayer) 
            {
                livesManager.DeathPLayer();  
            }
                
        }
    }
    
    }


