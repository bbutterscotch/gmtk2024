using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using FMODUnity;

public class CycleController : MonoBehaviour
{

    public float cycleLength = 20f;
    public int currentCycle;
    public float currentTime;
    public int timeSteps = 100;
    public int difficulty = 1;
    public int beesPerRound = 1;
    EnemySpawner enemySpawner;
    BeeSpawner beeSpawner;
    HiveResources hv;
    [SerializeField] private EventReference cycleBellSound;
    [SerializeField] private EventReference music;
    public PathFinder3 pf;
    public bool pause = false;

    private void Awake()
    {
        enemySpawner = FindFirstObjectByType<EnemySpawner>();
        beeSpawner = FindFirstObjectByType<BeeSpawner>();
        hv = FindFirstObjectByType<HiveResources>();
        pf = FindFirstObjectByType<PathFinder3>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(initializePath(1f));
        currentCycle = 1;
        currentTime = 0f;
        StartCoroutine(updateTime());
        AudioController.instance.PlayOneShot(cycleBellSound, this.transform.position);
    }

    IEnumerator initializePath(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(pf.getPath());
    }

    IEnumerator spawnBees(int multiplier)
    {
        for (int i = 0; i < hv.nurseryTiles*multiplier; i++)
        {
            //Debug.Log("Bee Spawned");
            beeSpawner.SpawnBee();
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator updateTime()
    {
        if (!pause)
        {
            currentTime += cycleLength / timeSteps;
            if (currentTime % Math.Ceiling(cycleLength / beesPerRound) <= 0.2 && Math.Round(currentTime) != 0)
            {
                //Debug.Log("Spawn bee");
                StartCoroutine(spawnBees(1));
            }
            if (Math.Round(currentTime) == cycleLength)
            {
                currentTime = 0f;
                currentCycle += 1;
                AudioController.instance.PlayOneShot(cycleBellSound, this.transform.position);
                if (currentCycle % 2 != 0)
                {
                    AudioController.instance.SetParameter(music, "Cycle", 0, this.transform.position);
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Cycle", 0);
                }
                else
                {
                    AudioController.instance.SetParameter(music, "Cycle", 1, this.transform.position);
                    FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Cycle", 1);
                }
                int enemiesToSpawn = (currentCycle / 5 + 1) * difficulty;
                enemySpawner.spawnEnemies(enemiesToSpawn);
                StartCoroutine(spawnBees(1));
                float startTime = Time.realtimeSinceStartup;
                StartCoroutine(pf.getPath());
                float endTime = Time.realtimeSinceStartup;
                float pathFindingTime = endTime - startTime;
                Debug.Log("Path Time: " + pathFindingTime.ToString() + "s");
            }
            yield return new WaitForSeconds(cycleLength / timeSteps);
            StartCoroutine(updateTime());
        } else
        {
            yield return null;
        }
        
    }

    public void pauseGame()
    {
        pause = !pause;
        Debug.Log("Paused: " + pause.ToString());
        if (pause)
        {
            Time.timeScale = 0;
        } else
        {
            Time.timeScale = 1.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
