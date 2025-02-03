using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using FMODUnity;
using System;
using System.Reflection;

public class EnemySpawner : MonoBehaviour
{

    PathFinder4 pf;
    List<Vector3Int> tiles;
    MapController mc;
    Tilemap spriteMap;
    Tilemap walkable;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject bear;
    [SerializeField] GameObject mite;
    [SerializeField] private EventReference enemySpawnSound;
    private System.Random rand;

    // Start is called before the first frame update
    void Start()
    {
        rand = new System.Random();
        pf = FindFirstObjectByType<PathFinder4>();
        mc = FindObjectOfType<MapController>();
        spriteMap = mc.spriteMap;
        walkable = mc.walkable;
    }

    public void spawnEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int enemy = rand.Next(0, 3);
            if (enemy == 0)
            {
                spawnBear();
            } else if (enemy == 1)
            {
                spawnEnemy();
            } else
            {
                spawnMites();
            }
        }
    }

    public void spawnEnemy()
    {
        tiles = pf.path;
        int index = rand.Next(0, tiles.Count);
        Instantiate(enemy, spriteMap.CellToWorld(tiles[index]), Quaternion.identity);
        AudioController.instance.PlayOneShot(enemySpawnSound, this.transform.position);
    }

    public void spawnEnemy(Vector3Int loc)
    {
        Instantiate(enemy, spriteMap.CellToWorld(loc), Quaternion.identity);
        AudioController.instance.PlayOneShot(enemySpawnSound, this.transform.position);
    }

    public void spawnBear()
    {
        tiles = pf.path;
        int index = rand.Next(0, tiles.Count);
        Instantiate(bear, spriteMap.CellToWorld(tiles[index]), Quaternion.identity);
        AudioController.instance.PlayOneShot(enemySpawnSound, this.transform.position);
    }

    public void spawnBear(Vector3Int loc)
    {
        Instantiate(bear, spriteMap.CellToWorld(loc), Quaternion.identity);
        AudioController.instance.PlayOneShot(enemySpawnSound, this.transform.position);
    }

    public void spawnMite()
    {
        tiles = pf.path;
        int index = rand.Next(0, tiles.Count);
        Instantiate(mite, spriteMap.CellToWorld(tiles[index]), Quaternion.identity);
        AudioController.instance.PlayOneShot(enemySpawnSound, this.transform.position);
    }

    public void spawnMite(Vector3Int loc)
    {
        Instantiate(mite, spriteMap.CellToWorld(loc), Quaternion.identity);
        AudioController.instance.PlayOneShot(enemySpawnSound, this.transform.position);
    }

    public void spawnMites()
    {
        tiles = pf.path;
        int index = rand.Next(0, tiles.Count);
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (walkable.HasTile(tiles[index] + new Vector3Int(x, y))) {
                    int spawn = rand.Next(0, 2);
                    if (spawn == 1)
                    {
                        Instantiate(mite, spriteMap.CellToWorld(tiles[index] + new Vector3Int(x, y)), Quaternion.identity);
                    }
                }
                
            }
        }
        AudioController.instance.PlayOneShot(enemySpawnSound, this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
