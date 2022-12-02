using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Freshaliens.Level.Components;

using Freshaliens.Player.Components;


public class LivesManager : MonoBehaviour
{
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

    // ReSharper disable Unity.PerformanceAnalysis
    private void PlayerHit(PlayerMovementController hitPlayer)
    {
        Debug.Log("HITTATO");
        numberOfLives--;
        if (numberOfLives == 0)
        {
            Debug.Log("GAME OVER");

            hitPlayer.transform.position = Checkpoint.LastActiveCheckpoint.transform.position;
            numberOfLives = initialNumberOfLives;

        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;

        if (( (1 << layer) & hitLayers) != 0) {
            PlayerHit(player1);
        }
    }

    public void HitPlayer()
    {
        PlayerHit(player1);
    }
    
}
        

    
   

