using System;
using System.Collections;
using System.Collections.Generic;
using Freshaliens.Interaction;
using Freshaliens.Player.Components;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


namespace Freshaliens.Interaction
{
    public class Obstacle : Interactable
    {
        [Header("Layers Settings")]
        [SerializeField] private int _ninjaLayer = 7;
        [SerializeField] private int _fairyLayer = 8;
        
        private LivesManager _livesManager;
        public Target _target = Target.Both;

        public enum Target
        {
            Both,
            Ninja,
            Fairy
        }

    //     private void Start()
    //     {
    //         _livesManager = GetComponent<LivesManager>(); 
    //         
    //     }
    //
    //     private void OnTriggerEnter2D(Collider2D collider)
    //     {
    //         if (!collider.CompareTag("Player"))
    //         {
    //             return;
    //         }
    //         int colliderLayer = collider.gameObject.layer;
    //         // MovementController player = collider.gameObject.GetComponent<MovementController>();
    //         
    //         // switch (_target)
    //         // {
    //         //     case Target.Ninja:
    //         //         if (colliderLayer==_ninjaLayer) 
    //         //         {
    //         //             _livesManager.PlayerHit(player);  
    //         //             Debug.Log("hit ninja");
    //         //         }
    //         //         break;
    //         //     case Target.Fairy:
    //         //         if (colliderLayer==_fairyLayer)
    //         //         {
    //         //             _livesManager.PlayerHit(player);
    //         //             Debug.Log("hit fairy");
    //         //         }
    //         //         break;
    //         //     case Target.Both:
    //         //         _livesManager.PlayerHit(player);
    //         //         Debug.Log("hit whoever");
    //         //         break;
    //         // }
    //     }
    
    }

}
