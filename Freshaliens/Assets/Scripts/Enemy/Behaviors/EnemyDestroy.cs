using System;
using Freshaliens.Management;
using UnityEngine;

public class EnemyDestroy : MonoBehaviour
{
    [SerializeField] private LayerMask hitLayers = -1;
    [SerializeField] private int numberOfLives = 3;
    private bool hit = false;
    public event Action<GameObject> OnDestroyEnemy;
    public event Action OnDamageEnemy;
    
    private void Update()
    {
        if (hit)
        {
            EnemyHit();
        }

        LevelManager levelManager = LevelManager.Instance;
        
        hit = false;
    }

    private void EnemyHit()
    {
        numberOfLives--;
        if (numberOfLives == 0)
        {
            // TODO CHECK THIS WHAT
            OnDestroyEnemy?.Invoke(gameObject);
            AudioManager1.instance.PlaySFX("nemicomorto");
            gameObject.SetActive(false);
        }
        else
        {
            OnDamageEnemy?.Invoke();
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        int layer = collision.gameObject.layer;

        if (( (1 << layer) & hitLayers) != 0) {
            hit = true;
        }
    }
}
