using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    // Singleton
    private static ProjectilePool main;
    public static ProjectilePool Main => main;

    [Header("Settings")]
    [SerializeField] private bool isMainPool = false;
    [SerializeField] private string poolIdentifier = "Projectiles";
    [Header("Pool")]
    [SerializeField] private int poolSize = 20;
    [SerializeField] private GameObject pooledPrefab = null;
    [SerializeField] private Transform poolContainer = null;

    private Queue<Projectile> pooledGOs = new Queue<Projectile>();

    private void Awake()
    {
        if (isMainPool || main == null) main = this;
    }

    private void Start()
    {
        poolContainer.gameObject.SetActive(false); // Set the container inactive to automatically deactivate all children
        InitializePool();
    }

    /// <summary>
    /// Returns the projectile pool with corresponding ID or the main pool if no pools match the desired id
    /// </summary>
    public static ProjectilePool GetByID(string id) {
        foreach (ProjectilePool pp in FindObjectsOfType<ProjectilePool>()) {
            if (id == pp.poolIdentifier) return pp;
        }
        return main;
    }

    /// <summary>
    /// Initialize the pool by instantiating and enqueueing the gameobjects
    /// </summary>
    private void InitializePool() {
        pooledGOs = new Queue<Projectile>();
        for (int i = 0; i < poolSize; i++) {
            Projectile p = Instantiate(pooledPrefab, poolContainer).GetComponent<Projectile>();
            p.OwnerPool = this;
            pooledGOs.Enqueue(p);
        }
    }

    /// <summary>
    /// Get and spawn a projectile from the pool
    /// </summary>
    /// <param name="position">Spawn position of the projectile</param>
    /// <param name="rotation">Spawn rotation of the projectile</param>
    /// <param name="velocity">Spawn velocity of the projectile</param>
    /// <returns>The spawned projectile</returns>
    public Projectile Spawn(Vector3 position, Quaternion rotation, Vector2 velocity) {
        Projectile p = pooledGOs.Dequeue();
        Transform t = p.transform;
        t.parent = null;
        t.position = position;
        t.localRotation = rotation;
        p.Fire(velocity);

        pooledGOs.Enqueue(p);   // add back to pool
        return p;
    }
    public Projectile Spawn(Vector3 position, Vector2 velocity) => Spawn(position, Quaternion.identity, velocity);

    public void Reclaim(Projectile p) {
        p.transform.parent = poolContainer;
    }
}
