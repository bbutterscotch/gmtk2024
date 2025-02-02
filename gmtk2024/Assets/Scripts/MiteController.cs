using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MiteController : MonoBehaviour
{
    [SerializeField] private int beesToKill = 5;
    [SerializeField] private EventReference denyResourceSound;

    private int beesKilled = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Mite!");
        beesKilled++;
        AudioController.instance.PlayOneShot(denyResourceSound, this.transform.position);
        if (beesKilled == beesToKill)
        {
            Destroy(gameObject);
        }
    }
}
