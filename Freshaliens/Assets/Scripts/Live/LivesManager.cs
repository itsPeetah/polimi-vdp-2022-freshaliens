using System.Collections;
using UnityEngine;
using Freshaliens.Level.Components;


using Freshaliens.Player.Components;
using Freshaliens.Management;


public class LivesManager : MonoBehaviour
{
    [SerializeField] private LayerMask hitLayers = -1;
    [SerializeField] private int deathLayer = 16;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;

        if (( (1 << layer) & hitLayers) != 0) {
            HitPlayer(false);
        }
    }

    public void HitPlayer(bool triggerRespawn = false) {
        LevelManager.Instance.DamagePlayer(skipInvulnerableCheck: triggerRespawn);
        if (triggerRespawn && !LevelManager.Instance.GameOver) LevelManager.Instance.RespawnPlayer();
    }
    
}
        

    
   

