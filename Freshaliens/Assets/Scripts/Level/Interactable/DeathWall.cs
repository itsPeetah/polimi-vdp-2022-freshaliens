using UnityEngine;


namespace Freshaliens.Interaction
{
    public class DeathWall : Interactable
    {
        [Header("Layers Settings")]
        [SerializeField] private LayerMask playerLayer = 2 << 7;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.CompareTag("Player")) return;

            GameObject playerObject = collider.gameObject;
            LivesManager livesManager = playerObject.GetComponent<LivesManager>();
            if (livesManager != null && ((1 << playerObject.layer) & playerLayer) != 0)
            {
                livesManager.HitPlayer(true, Vector3.zero);
            }
                
        }
    }
    
    }


