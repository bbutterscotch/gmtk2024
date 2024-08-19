using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Bee : MonoBehaviour
{
    private MapController mc;
    private Tilemap tilemap;
    [SerializeField] private float delay = 1.0f;
    List<int> path;
    private List<Vector3Int> tiles;
    PathFinder pf;
    HiveResources hv;
    CycleController cc;
    private int nectar = 0;
    private int pollen = 0;
    private int honey = 0;
    private int wax = 0;
    private int royalJelly = 0;
    private int startingCycle;

    // Start is called before the first frame update
    void Start()
    {
        // gets the tilemap
        mc = FindObjectOfType<MapController>();
        tilemap = mc.walkable;
        pf = FindObjectOfType<PathFinder>();
        hv = FindObjectOfType<HiveResources>();
        cc = FindObjectOfType<CycleController>();
        startingCycle = cc.currentCycle;
        path = pf.path;
        tiles = pf.tiles;
        StartCoroutine(moveTile(delay, 0));
    }

    IEnumerator moveTile(float delayTime, int pathIndex)
    {
        Vector3 distance = tilemap.CellToWorld(tiles[path[pathIndex]]) - transform.position;
        StartCoroutine(moveOverTime(distance));
        //transform.position = tilemap.CellToWorld(tiles[path[pathIndex]]);
        TileBase currTile = tilemap.GetTile(tiles[path[pathIndex]]);
        if (currTile.name.Equals("Tile_Beekeeper"))
        {
            honey++;
        } else if (currTile.name.Equals("Tile_Pond"))
        {
            nectar++;
        } else if (currTile.name.Equals("Tile_Meadow"))
        {
            pollen++;
        } else if (currTile.name.Equals("Tile_Woodland"))
        {
            wax++;
        } else if (currTile.name.Equals("Tile_Garden"))
        {
            royalJelly++;
        }
        yield return new WaitForSeconds(delayTime);
        // edges refers to the index in tiles
        // while next tile does not have an edge to the current tile
        // add the previous tile until an edge with the next tile is found
        
        if (pathIndex == path.Count - 1)
        {
            // reset resources
            hv.nectar += nectar;
            nectar = 0;
            hv.pollen += pollen;
            pollen = 0;
            hv.honey += honey;
            honey = 0;
            hv.wax += wax;
            wax = 0;
            hv.royalJelly += royalJelly;
            royalJelly = 0;

            // Bee death
            if (startingCycle + 10 <= cc.currentCycle)
            {
                hv.bees--;
                Destroy(gameObject);
            }

            // reset path + update tiles
            path = pf.path;
            tiles = pf.tiles;
            
            StartCoroutine(moveTile(delay, 0));
        } else
        {
            StartCoroutine(moveTile(delay, pathIndex + 1));
        }
        
    }

    IEnumerator moveOverTime(Vector3 distance)
    {
        for (int i = 0; i < 6; i++)
        {
            transform.position += distance / 6;
            yield return new WaitForSeconds(delay*(1.0f / 12.0f));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("Enemy!");
            hv.bees--;
            Destroy(gameObject);
            
        } 
        // else wave hi to other bee
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Mite")
        {
            if (nectar > 0)
            {
                nectar--;
            }
            if (pollen > 0)
            {
                pollen--;
            }
            if (honey > 0) 
            { 
                honey--;
            }
            if (wax > 0)
            {
                wax--;
            }
            if (royalJelly > 0)
            {
                royalJelly--;
            }
        }
    }
}
