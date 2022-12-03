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
    private bool hit = false;
    private int numberOfLives;
    private bool _alreadyHit = false;
    
    void Start()
    {
        ninja = PlayerMovementController.Instance;
        numberOfLives = initialNumberOfLives;
        
    }
    
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
        Vector3 newNinjaPosition = Checkpoint.LastActiveCheckpoint.RespawnPosition;
        hitPlayer.transform.position = newNinjaPosition;
        FairyMovementController.Instance.RespawnWithNinja(newNinjaPosition);
        numberOfLives = initialNumberOfLives;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;

        if (( (1 << layer) & hitLayers) != 0) {
            PlayerHit(ninja);
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
            PlayerHit(ninja);
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
        PlayerHit(ninja);
        StartCoroutine(ResetAlreadyHit());
    }

    IEnumerator ResetAlreadyHit()
    {
        yield return new WaitForSeconds(0.5f);
        _alreadyHit = false;
        yield return null;
    }

    public void DeathPLayer()
    {
        PlayerDeath(ninja);
    }
    
}
        

    
   

