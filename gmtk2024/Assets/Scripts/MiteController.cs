using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiteController : MonoBehaviour
{

    [SerializeField] private int beesToKill = 5;
    private int beesKilled = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        beesKilled++;
        if (beesKilled == beesToKill)
        {
            Destroy(gameObject);
        }
    }
}
