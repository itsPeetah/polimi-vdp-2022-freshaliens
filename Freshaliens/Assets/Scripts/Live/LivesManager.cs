using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

using Freshaliens.Player.Components;


public class LivesManager : MonoBehaviour
{
    [SerializeField] private Transform spawnPosition;
    [SerializeField] private PlayerMovementController player1;
    //[SerializeField] private SpriteRenderer[] _spriteRenderers;
    [SerializeField] private int initialNumberOfLives = 3;
    [SerializeField] private LayerMask hitLayers = -1;
    //Saving starting position for future respawns
    private Vector3 player1StartingPosition;
    private bool hit = false;
    private int numberOfLives;

    // Start is called before the first frame update
    void Start()
    {
        numberOfLives = initialNumberOfLives;
        
    }

    // Update is called once per frame
    void Update()
    {
        //Player 1 loosing of a life (now with an input key, then with a condition)
        if (hit)
        {
            PlayerHit(player1);
        }
        hit = false;

    }

   

    // ReSharper disable Unity.PerformanceAnalysis
    public void PlayerHit(PlayerMovementController hitPlayer)
    {
        Debug.Log("HITTATO");
        numberOfLives--;
        if (numberOfLives == 0)
        {
            Debug.Log("GAME OVER");
            
            hitPlayer.transform.position = spawnPosition.position;
            numberOfLives = initialNumberOfLives;

        }
        else
        {
            //if hit player1 respawns (situation when a player loses a life for falling)
            if (hitPlayer == player1)
            {
                Debug.Log("PLAYER 1 LOST A LIFE!");
                Debug.Log("You have " + numberOfLives + " remaining!");
               // hitPlayer.transform.position = player1StartingPosition;
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;

        if (( (1 << layer) & hitLayers) != 0) {
            Debug.Log("colpito");
            hit = true;
        }
    }

    public void HitPlayer()
    {
        //hit 
    }
    
    
    }
        

    
   

