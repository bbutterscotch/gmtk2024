using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;

public class CycleController : MonoBehaviour
{

    public float cycleLength = 20f;
    public int currentCycle;
    public float currentTime;
    public int timeSteps = 20;
    public int difficulty = 1;
    public int beesPerRound = 4;
    EnemySpawner enemySpawner;
    BeeSpawner beeSpawner;
    HiveResources hv;

    // Start is called before the first frame update
    void Start()
    {
        enemySpawner = FindObjectOfType<EnemySpawner>();
        beeSpawner = FindObjectOfType<BeeSpawner>();
        hv = FindObjectOfType<HiveResources>();
        currentCycle = 1;
        currentTime = 0f;
        StartCoroutine(spawnBees(5));
        StartCoroutine(updateTime());
    }

    IEnumerator spawnBees(int multiplier)
    {
        for (int i = 0; i < hv.nurseryTiles*multiplier; i++)
        {
            Debug.Log("Bee Spawned");
            beeSpawner.SpawnBee();
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator updateTime()
    {
        currentTime += cycleLength / timeSteps;
        if (currentTime % Math.Ceiling(cycleLength/beesPerRound) == 0)
        {
            Debug.Log("Spawn bee");
            StartCoroutine(spawnBees(1));
        }
        if (currentTime == cycleLength)
        {
            currentTime = 0f;
            currentCycle += 1;
            int enemiesToSpawn = (currentCycle / 5 + 1) * difficulty;
            enemySpawner.spawnEnemies(enemiesToSpawn);
            StartCoroutine(spawnBees(1));
        }
        yield return new WaitForSeconds(cycleLength / timeSteps);
        StartCoroutine(updateTime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
