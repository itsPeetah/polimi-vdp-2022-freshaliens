using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileTarget : MonoBehaviour
{
    [SerializeField] private UnityEvent onHit;

    public void Hit()
    {
        onHit.Invoke();
        Debug.Log("HIT CALLED");
    }
}
