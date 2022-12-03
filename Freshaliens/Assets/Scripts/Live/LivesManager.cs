using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Freshaliens.Level.Components;


using Freshaliens.Player.Components;
using Newtonsoft.Json.Bson;


public class LivesManager : MonoBehaviour
{
    [SerializeField] private PlayerMovementController player1;
    //[SerializeField] private SpriteRenderer[] _spriteRenderers;
    [SerializeField] private int initialNumberOfLives = 3;
    [SerializeField] private LayerMask hitLayers = -1;
    [SerializeField] private int deathLayer = 16;
    //Saving starting position for future respawns
    private Vector3 player1StartingPosition;
    private bool hit = false;
    private int numberOfLives;

    private bool _alreadyHit = false;

    // Start is called before the first frame update
    void Start()
    {
        numberOfLives = initialNumberOfLives;
        
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void PlayerHit(PlayerMovementController hitPlayer)
    {
        Debug.Log("Hit");
        numberOfLives--;
        if (numberOfLives == 0)
        {
            PlayerDeath(hitPlayer);
        }
        
    }

    private void PlayerDeath(PlayerMovementController hitPlayer)
    {
        Debug.Log("MORTOOOO");
        
        Vector3 newNinjaPosition = Checkpoint.LastActiveCheckpoint.RespawnPosition;
        hitPlayer.transform.position = newNinjaPosition;
        FairyMovementController.Instance.RespawnWithNinja(newNinjaPosition);
        numberOfLives = initialNumberOfLives;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;

        if (( (1 << layer) & hitLayers) != 0) {
            PlayerHit(player1);
        }
        
    }

    public void HitPlayer(bool enemy)
    {
        if (enemy)
        {
            HitByEnemy();
        }
        else
        {
            PlayerHit(player1);
        }
    }

    private void HitByEnemy()
    {
        if (_alreadyHit)
        {
            _alreadyHit = false;
            return;
        }

        _alreadyHit = true;
        PlayerHit(player1);
    }

    public void DeathPLayer()
    {
        PlayerDeath(player1);
    }
    
}
        

    
   

