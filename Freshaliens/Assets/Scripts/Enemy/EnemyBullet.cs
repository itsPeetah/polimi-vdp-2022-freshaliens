using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float dieTime, damage;
    public GameObject diePEFFECT;
    void Start()
    {
        StartCoroutine(CountDownTimer());
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
       Die();
    }

    IEnumerator CountDownTimer()
    {
        yield return new WaitForSeconds(dieTime);
       Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
