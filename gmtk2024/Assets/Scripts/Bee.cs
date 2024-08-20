using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.FilePathAttribute;

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
    private SpriteRenderer spriteRenderer;

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
        path = pf.getPath();
        tiles = pf.getTiles();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(moveTile(delay, 0));
    }

    void playHitAnimation(Vector3Int location, string tileName)
    {
        AnimatedTile at = null;
        if (tileName == "Pond")
        {
            at = mc.pondHitTile;
        } else if (tileName == "Beekeeper")
        {
            at = mc.beekeeperHitTile;
        } else if (tileName == "Meadow")
        {
            at = mc.meadowHitTile;
        } else if (tileName == "Woodland")
        {
            at = mc.woodlandHitTile;
        } else if (tileName == "Garden")
        {
            at = mc.gardenHitTile;
        }
        tilemap.SetTile(location, at);
        tilemap.SetAnimationFrame(location, 0);
        //yield return new WaitForSeconds(at.m_AnimatedSprites.Length / at.m_MinSpeed);
        //tilemap.SetTile(location, t);

    }

    IEnumerator moveTile(float delayTime, int pathIndex)
    {
        Vector3 distance = tilemap.CellToWorld(tiles[path[pathIndex]]) - new Vector3Int(0, 0, 1) - transform.position;
        StartCoroutine(moveOverTime(distance));
        //transform.position = tilemap.CellToWorld(tiles[path[pathIndex]]);
        TileBase currTile = tilemap.GetTile(tiles[path[pathIndex]]);
        if (currTile.name.Equals("Tile_Beekeeper_Drop") || currTile.name.Equals("Tile_Beekeeper_Hit"))
        {
            honey++;
            playHitAnimation(tiles[path[pathIndex]], "Beekeeper");
        } else if (currTile.name.Equals("Tile_Pond_Drop") || currTile.name.Equals("Tile_Pond_Hit"))
        {
            nectar++;
            playHitAnimation(tiles[path[pathIndex]], "Pond");
        } else if (currTile.name.Equals("Tile_Meadow_Drop") || currTile.name.Equals("Tile_Meadow_Hit"))
        {
            pollen++;
            playHitAnimation(tiles[path[pathIndex]], "Meadow");
        } else if (currTile.name.Equals("Tile_Woodland_Drop") || currTile.name.Equals("Tile_Woodland_Hit"))
        {
            wax++;
            playHitAnimation(tiles[path[pathIndex]], "Woodland");
        } else if (currTile.name.Equals("Tile_Garden_Drop") || currTile.name.Equals("Tile_Garden_Hit"))
        {
            royalJelly++;
            playHitAnimation(tiles[path[pathIndex]], "Garden");
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
            path = pf.getPath();
            tiles = pf.getTiles();
            
            StartCoroutine(moveTile(delay, 0));
        } else
        {
            StartCoroutine(moveTile(delay, pathIndex + 1));
        }
        
    }

    IEnumerator moveOverTime(Vector3 distance)
    {
        if (distance.x > 0)
        {
            spriteRenderer.flipX = true;
        } else
        {
            spriteRenderer.flipX = false;
        }
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
