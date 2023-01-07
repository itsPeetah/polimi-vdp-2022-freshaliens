using UnityEngine;


namespace Freshaliens.Interaction
{
    public class DeathWall : Interactable
    {
        [Header("Layers Settings")]
        [SerializeField] private LayerMask playerLayer = 2 << 7;

        [Header("SUPER SECRET SETTINGS")]
        [SerializeField] private bool PLEASE_DO_NOT_CLICK_THIS_FOR_ANY_REASON = false;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.CompareTag("Player")) return;

            if (PLEASE_DO_NOT_CLICK_THIS_FOR_ANY_REASON) {
                PlayerData.Instance.GameBeaten = true;
                Application.OpenURL("https://www.youtube.com/watch?v=xvFZjo5PgG0");
                Application.Quit();
                return;
            }


            GameObject playerObject = collider.gameObject;
            LivesManager livesManager = playerObject.GetComponent<LivesManager>();
            if (livesManager != null && ((1 << playerObject.layer) & playerLayer) != 0)
            {
                livesManager.HitPlayer(true, Vector3.zero);
            }
                
        }
    }
    
    }


