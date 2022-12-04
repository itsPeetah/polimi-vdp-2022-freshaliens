using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Freshaliens.Level.Components;


using Freshaliens.Player.Components;
using Newtonsoft.Json.Bson;


public class LivesManager : MonoBehaviour
{
    private PlayerMovementController ninja;
    
    [SerializeField] private int initialNumberOfLives = 3;
    [SerializeField] private LayerMask hitLayers = -1;
    [SerializeField] private int deathLayer = 16;
    
    // STATE
    public static int numberOfLives = 3; // UGLY, I wanted a Singleton but it's not possible...
    private bool hit = false;
    private bool _alreadyHit = false;
    
    void Start()
    {
        ninja = PlayerMovementController.Instance;
        numberOfLives = initialNumberOfLives;
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;

        if (( (1 << layer) & hitLayers) != 0) {
            PlayerHit(false);
        }
    }
    
    private void PlayerHit(bool mustRespawn)
    {
        numberOfLives--;
        if (numberOfLives == 0)
        {
            PlayerDeath();
            return;
        }

        if (mustRespawn)
        {
            Respawn(Checkpoint.LastActiveCheckpoint.RespawnPosition);
        }
    }
    
    private void PlayerDeath()
    {
        Respawn(Checkpoint.StartingCheckpoint.RespawnPosition);
        numberOfLives = initialNumberOfLives;
    }
    
    private void Respawn(Vector3 position)
    {
        ninja.transform.position = position;
        FairyMovementController.Instance.RespawnWithNinja(position);
    }
    
    private void HitByEnemy()
    {
        if (_alreadyHit)
        {
            _alreadyHit = false;
            return;
        }

        _alreadyHit = true;
        PlayerHit(false);
        StartCoroutine(ResetAlreadyHit());
    }
    
    IEnumerator ResetAlreadyHit()
    {
        yield return new WaitForSeconds(0.5f);
        _alreadyHit = false;
        yield return null;
    }

    // For external usage
    public void HitPlayer(bool isEnemy)
    {
        if (isEnemy)
        {
            HitByEnemy();
        }
        else
        {
            PlayerHit(false);
        }
    }
    
    public void HitAndRespawn()
    {
        PlayerHit(true);
    }
    
    public void DeathPLayer()
    {
        PlayerDeath();
    }

    
}
        

    
   

