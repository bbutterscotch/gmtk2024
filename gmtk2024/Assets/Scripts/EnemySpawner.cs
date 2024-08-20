using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using FMODUnity;

public class EnemySpawner : MonoBehaviour
{

    PathFinder pf;
    List<Vector3Int> tiles;
    MapController mc;
    Tilemap spriteMap;
    Tilemap walkable;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject bear;
    [SerializeField] GameObject mite;
    [SerializeField] private EventReference enemySpawnSound;

    // Start is called before the first frame update
    void Start()
    {
        pf = FindObjectOfType<PathFinder>();
        mc = FindObjectOfType<MapController>();
        spriteMap = mc.spriteMap;
        walkable = mc.walkable;
    }

    public void spawnEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int enemy = Random.Range(0, 3);
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
        tiles = pf.getTiles();
        int index = Random.Range(0, tiles.Count);
        Instantiate(enemy, spriteMap.CellToWorld(tiles[index]), Quaternion.identity);
        AudioController.instance.PlayOneShot(enemySpawnSound, this.transform.position);
    }

    public void spawnBear()
    {
        tiles = pf.getTiles();
        int index = Random.Range(0, tiles.Count);
        Instantiate(bear, spriteMap.CellToWorld(tiles[index]), Quaternion.identity);
        AudioController.instance.PlayOneShot(enemySpawnSound, this.transform.position);
    }

    public void spawnMite()
    {
        tiles = pf.getTiles();
        int index = Random.Range(0, tiles.Count);
        Instantiate(mite, spriteMap.CellToWorld(tiles[index]), Quaternion.identity);
        AudioController.instance.PlayOneShot(enemySpawnSound, this.transform.position);
    }

    public void spawnMites()
    {
        tiles = pf.getTiles();
        Debug.Log("TEST");
        int index = Random.Range(0, tiles.Count);
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (walkable.HasTile(tiles[index] + new Vector3Int(x, y))) {
                    int spawn = Random.Range(0, 2);
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
