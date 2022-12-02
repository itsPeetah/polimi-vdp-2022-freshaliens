using UnityEngine;


namespace Freshaliens.Interaction
{
    public class Obstacle : Interactable
    {
        [Header("Layers Settings")]
        [SerializeField] private int _ninjaLayer = 7;
        [SerializeField] private int _fairyLayer = 8;
        
        [SerializeField] private Target _target = Target.Both;
        [SerializeField] private bool _isEnemy = false;

        private enum Target
        {
            Both,
            Ninja,
            Fairy
        }
        
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player"))
        {
            return;
        }
        

        GameObject playerObject = collider.gameObject;
        int colliderLayer = playerObject.layer;
        LivesManager livesManager = playerObject.GetComponent<LivesManager>();
        
        switch (_target)
        {
            case Target.Ninja:
                if (colliderLayer==_ninjaLayer) 
                {
                    livesManager.HitPlayer(_isEnemy);  
                }
                break;
            case Target.Fairy:
                if (colliderLayer==_fairyLayer)
                {
                    livesManager.HitPlayer(_isEnemy);
                }
                break;
            case Target.Both:
                livesManager.HitPlayer(_isEnemy);
                break;
        }
    }
    
    }

}
