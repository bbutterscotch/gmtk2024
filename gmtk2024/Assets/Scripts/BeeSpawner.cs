using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BeeSpawner : MonoBehaviour
{

    [SerializeField] GameObject beePrefab;
    private MapController mc;
    private Tilemap tilemap;
    private HiveResources hv;

    // Start is called before the first frame update
    void Start()
    {
        hv = FindObjectOfType<HiveResources>();
        mc = FindObjectOfType<MapController>();
        tilemap = mc.walkable;
    }


    public void SpawnBee()
    {
        hv.bees++;
        GameObject newBee = Instantiate(beePrefab, tilemap.CellToWorld(mc.startTile), Quaternion.identity);
        SpriteRenderer newSprite = newBee.GetComponent<SpriteRenderer>();
        Color[] colors = new Color[6];
        colors[0] = Color.red;
        colors[1] = Color.yellow;
        colors[2] = Color.green;
        colors[3] = Color.cyan;
        colors[4] = Color.blue;
        colors[5] = Color.magenta;
        newSprite.color = colors[Random.Range(0, 6)];
    }
}
