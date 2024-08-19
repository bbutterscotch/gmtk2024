using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemySpawner : MonoBehaviour
{

    PathFinder pf;
    List<Vector3Int> tiles;
    MapController mc;
    Tilemap tilemap;
    [SerializeField] GameObject enemy;
    [SerializeField] GameObject bear;
    [SerializeField] GameObject mite;

    // Start is called before the first frame update
    void Start()
    {
        pf = FindObjectOfType<PathFinder>();
        mc = FindObjectOfType<MapController>();
        tilemap = mc.walkable;
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
        tiles = pf.tiles;
        int index = Random.Range(0, tiles.Count);
        Instantiate(enemy, tilemap.CellToWorld(tiles[index]), Quaternion.identity);
    }

    public void spawnBear()
    {
        tiles = pf.tiles;
        int index = Random.Range(0, tiles.Count);
        Instantiate(bear, tilemap.CellToWorld(tiles[index]), Quaternion.identity);
    }

    public void spawnMite()
    {
        tiles = pf.tiles;
        int index = Random.Range(0, tiles.Count);
        Instantiate(mite, tilemap.CellToWorld(tiles[index]), Quaternion.identity);
    }

    public void spawnMites()
    {
        tiles = pf.tiles;
        int index = Random.Range(0, tiles.Count);
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (tilemap.HasTile(tiles[index] + new Vector3Int(x, y))) {
                    int spawn = Random.Range(0, 2);
                    if (spawn == 1)
                    {
                        Instantiate(mite, tilemap.CellToWorld(tiles[index] + new Vector3Int(x, y)), Quaternion.identity);
                    }
                }
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
