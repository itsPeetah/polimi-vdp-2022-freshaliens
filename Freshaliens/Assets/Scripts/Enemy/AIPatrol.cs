using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class AIPatrol : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public bool mustPatrol;
    [SerializeField]private bool mustTurn;
    
    [SerializeField]public float walkSpeed, range;
    [SerializeField]private float distToPlayer;
    public Transform player;
    
    [SerializeField]public Rigidbody2D rb;
    [SerializeField]public Transform groundCheckPos;
    
    [SerializeField]public LayerMask groundLayer;
    
    [SerializeField] public Collider2D collider;
    
    void Start()
    {
        mustPatrol = true; //in default è null
    }

    // Update is called once per frame
    void Update()
    {
        if (mustPatrol)
        {
            Patrol();
        }

        distToPlayer = Vector2.Distance(transform.position, player.position);
        if (distToPlayer <= range)
        {
            if (player.position.x > transform.position.x && transform.localScale.x < 0 ||
                player.position.x < transform.position.x && transform.localScale.x > 0)
            {
                Flip();
            }

            mustPatrol = false;
            Shoot();
        }
        mustPatrol = true;
    }

    private void FixedUpdate()
    {
        if (mustPatrol)
        {
            mustTurn = !Physics2D.OverlapCircle(groundCheckPos.position, 1.0f, groundLayer);
            

        }
    }

    void Patrol()
    {
        rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
        if (mustTurn == true || collider.IsTouchingLayers(groundLayer))
        {
            
            Flip();
           

        }

        
    }

    void Flip()
    {
        transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        walkSpeed *= -1;
        mustTurn = false;

    }

    void Shoot()
    {
        
    }


}