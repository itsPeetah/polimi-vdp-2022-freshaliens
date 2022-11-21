using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class AIPatrol : MonoBehaviour
{
     public enum EnemyType
    {
        Blob, Shooter
    }
   public bool mustPatrol;
   private bool mustTurn, canShoot;
    
   
    [SerializeField]public float walkSpeed, aggroRange, stopRange;
    private float distToPlayer;
    public Transform player;
    
    
    [SerializeField]public Rigidbody2D rb;
    [SerializeField]public Transform groundCheckPos;
    [SerializeField]public LayerMask groundLayer;
    [SerializeField] public Collider2D collider;
    [SerializeField] public EnemyType type;
    float weaponAngleRadians = 0;
    float fireTimer = 0;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mustPatrol = true; //in default è null
        canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
       
        mustPatrol=true;
        distToPlayer = Vector2.Distance(transform.position, player.position);

       if (distToPlayer < aggroRange) // rincorre il player fin quando è in un certo range, se supera il range ritorna a fare il patrol
        {
            ChasePlayer();
            mustPatrol = false;

        }
       if(type == EnemyType.Shooter)
       PatrolShooter();
        else if (type == EnemyType.Blob)
        {
            PatrolFighter();
        }


       /*if (distToPlayer <= range)
       {
           if (player.position.x > transform.position.x && transform.localScale.x < 0 ||
               player.position.x < transform.position.x && transform.localScale.x > 0)
           {
               Flip();
           }

           mustPatrol = false;
            
          if (enemyType == EnemyType.Shooter)
           {
               if(canShoot)
               StartCoroutine(Shoot());
               
           }
       }
       mustPatrol = true;*/
    }

    private void FixedUpdate()
    {
        if (mustPatrol)
        {
            mustTurn = !Physics2D.OverlapCircle(groundCheckPos.position, 1.0f, groundLayer);
        }
    }

    void PatrolShooter()
    {
        if (mustPatrol)
        {
            rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
        }
        else 
        {   
            rb.velocity = Vector2.zero;
        } 
       
        
        if (mustTurn == true || collider.IsTouchingLayers(groundLayer))
        {
            Flip();
        }
        
        
    }
    
    void PatrolFighter() //to patrol blob just set aggrorange to 0
    {
        rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
        
        if (mustTurn == true || collider.IsTouchingLayers(groundLayer))
        {
            Flip();
        }

        if (distToPlayer < stopRange)
        {
            rb.velocity = Vector2.zero;
        }

        
    }
    
   

    void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        walkSpeed *= -1;
        mustTurn = false;
    }

    void ChasePlayer()
    {
        if (player.position.x > transform.position.x && transform.localScale.x < 0 ||
            player.position.x < transform.position.x && transform.localScale.x > 0)
        {
            Flip();
        }
    }

    

}