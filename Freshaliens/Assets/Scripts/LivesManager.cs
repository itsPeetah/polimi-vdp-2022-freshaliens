using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LivesManager : MonoBehaviour
{
    [SerializeField] private PlayerController player1;
    [SerializeField] private PlayerController player2;
    [SerializeField] private SpriteRenderer[] _spriteRenderers;
    [SerializeField] private int initialNumberOfLives = 10;
    private int numberOfLives;
    
    private bool coolDown = false;
    
    [SerializeField] private string hit_p1 = "Hit_P1";
    [SerializeField] private string hit_p2 = "Hit_P2";

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
        //Player 1 loosing of a life (now with an input key, then with a condition)
        if (Input.GetButtonDown(hit_p1))
        {
            if (!coolDown)
            {
                PlayerHit(player1);
            }
        }
        
        //Player 2 loosing of a life (now with an input key, then with a condition)
        if (Input.GetButtonDown(hit_p2))
        {
            if (!coolDown)
            {
                PlayerHit(player2);
            }
        }
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
                StartCoroutine(ActivateCoolDownCoroutine());
            }
        }
    }
        

    
    //sets the player immune after it get hit, calls BlinkCoroutine, set the player to vulnerable again
    private IEnumerator ActivateCoolDownCoroutine()
    {
        Debug.Log("ActivateCoolDownCoroutine");
        coolDown = true;
        yield return BlinkCoroutine(_spriteRenderers, 5f, 2f);
        coolDown = false;
        
        yield return null;
    }
    
    //makes the player blinking to show its immune property during cool down time
    private IEnumerator BlinkCoroutine(SpriteRenderer[] spriteRenderers, float duration, float speed)
    {
        Debug.Log("BlinkCoroutine");
        var elapsedTime = 0f;
        while( elapsedTime <= duration )
        {
            foreach (var t in spriteRenderers)
            {
                Color color = t.color;
                
                color.a = 0.25f + Mathf.PingPong( elapsedTime * speed, .5f );

                t.color = color;
            }
            
            elapsedTime += Time.deltaTime;
            
            yield return null;
        }
        
        // resets all the alpha to one
        foreach (var t in spriteRenderers)
        {
            Color color = t.color;
            color.a = 1f;
            t.color = color;
        }
        
    }
}
