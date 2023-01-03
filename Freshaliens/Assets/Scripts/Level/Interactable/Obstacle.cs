using UnityEngine;
using UnityEngine.Serialization;

namespace Freshaliens.Interaction
{
    public class Obstacle : Interactable
    {
        private enum Target
        {
            Both,
            Ninja,
            Fairy
        }

        [Header("Layers Settings")]
        [SerializeField] private int _ninjaLayer = 7;
        [SerializeField] private int _fairyLayer = 8;
        
        [SerializeField] private Target _target = Target.Both;
        [SerializeField] private bool triggerRespawnWhenHittingPlayer = false;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.CompareTag("Player"))
            {
                return;
            }


            GameObject playerObject = collider.gameObject;
            ///((CLA)) we get our position for the animation
            Vector3 obstaclePosition =gameObject.transform.position;
            //--------------------------------
            int colliderLayer = playerObject.layer;
            LivesManager livesManager = playerObject.GetComponent<LivesManager>();

            switch (_target)
            {
                case Target.Ninja:
                    if (colliderLayer == _ninjaLayer)
                    {
                        //livesManager.HitPlayer(triggerRespawnWhenHittingPlayer);
                        ///((CLA))
                        livesManager.HitPlayer(triggerRespawnWhenHittingPlayer,obstaclePosition);
                        //-----------------------------
                    }
                    break;
                case Target.Fairy:
                    if (colliderLayer == _fairyLayer)
                    {
                        //livesManager.HitPlayer(false);
                        ///((CLA))
                        livesManager.HitPlayer(false,obstaclePosition);
                        //-----------------------------
                    }
                    break;
                case Target.Both:
                    //livesManager.HitPlayer(triggerRespawnWhenHittingPlayer);
                    ///((CLA))
                    livesManager.HitPlayer(triggerRespawnWhenHittingPlayer,obstaclePosition);
                    //-----------------------------
                    break;
            }
        }
      
    
    }

}
