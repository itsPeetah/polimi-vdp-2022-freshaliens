using UnityEngine;
using Freshaliens.Player.Components;
using UnityEngine.Serialization;

namespace Freshaliens.Enemy.Components
{

    public class AIPatrol : MonoBehaviour
    {
        public enum EnemyType
        {
            Blob, Shooter
        }
        public bool mustPatrol;
        private bool mustTurn, stunned;


        [SerializeField] public float walkSpeed, aggroRange, stopRange;
        private float distToPlayer;
        private Transform player;


        [SerializeField] public Rigidbody2D rb;
        [SerializeField] public Transform groundCheckPos;
        [SerializeField] public LayerMask groundLayer;
        [FormerlySerializedAs("collider")] [SerializeField] public Collider2D ownCollider;
        [SerializeField] public EnemyType type;
       

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            player = PlayerMovementController.Instance.transform;
            mustPatrol = true; //in default è null
            
        }

        public void setStun(bool stun)
        {
            stunned = stun;
        }

        // Update is called once per frame
        void Update()
        {

            if (stunned)
            {
                rb.velocity = Vector2.zero;
                return;
            }

            mustPatrol = true;
            distToPlayer = Vector2.Distance(transform.position, player.position);

            if (distToPlayer < aggroRange) // rincorre il player fin quando è in un certo range, se supera il range ritorna a fare il patrol
            {
                ChasePlayer();
                mustPatrol = false;

            }

            if (type == EnemyType.Shooter)
                PatrolShooter();
            else if (type == EnemyType.Blob)
            {
                PatrolFighter();
            }

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


            if (mustTurn == true || ownCollider.IsTouchingLayers(groundLayer))
            {
                Flip();
            }


        }

        void PatrolFighter() //to patrol blob just set aggrorange to 0
        {
            rb.velocity = new Vector2(walkSpeed, rb.velocity.y);

            if (mustTurn == true || ownCollider.IsTouchingLayers(groundLayer))
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
}