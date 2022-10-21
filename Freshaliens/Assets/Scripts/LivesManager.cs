using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LivesManager : MonoBehaviour
{
    [SerializeField] private PlayerController player1;
    [SerializeField] private PlayerController player2;
    [SerializeField] private int initialNumberOfLives = 10;
    private int numberOfLives;

    //Saving starting position for future respawns
    private Vector3 player1StartingPosition;

    // Start is called before the first frame update
    void Start()
    {
        numberOfLives = initialNumberOfLives;
        player1StartingPosition = player1.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayerHit(PlayerController hitPlayer)
    {
        numberOfLives--;
        if (numberOfLives == 0)
        {
            Debug.Log("GAME OVER");
        }
        else
        {
            //if hit player1 respawns (situation when a player loses a life for falling)
            if (hitPlayer == player1)
            {
                Debug.Log("PLAYER 1 LOST A LIFE!");
                Debug.Log("You have " + numberOfLives + " remaining!");
                hitPlayer.transform.position = player1StartingPosition;
            }
            //if hit player2 blinks (situation when a player get hit from an enemy or a killing obstacle)
            else
            {
                Debug.Log("PLAYER 2 LOST A LIFE");
                Debug.Log("You have " + numberOfLives + " remaining!");
                player2.ActivateCoolDown();
            }
        }
    } 
}
