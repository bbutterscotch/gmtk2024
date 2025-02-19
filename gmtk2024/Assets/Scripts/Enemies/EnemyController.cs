using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class EnemyController : MonoBehaviour
{
    private int beesKilled = 0;
    [SerializeField] int beesToKill = 2;

    private void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        beesKilled++;
        if (beesKilled == beesToKill)
        {
            Destroy(gameObject);
        }
    }
}
