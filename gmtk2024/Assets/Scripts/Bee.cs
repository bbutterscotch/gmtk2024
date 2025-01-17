using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.FilePathAttribute;
using FMODUnity;
using UnityEditor.MemoryProfiler;

public class Bee : MonoBehaviour
{
    private MapController mc;
    private Tilemap tilemap;
    [SerializeField] private float delay = 2.0f;
    List<Vector3Int> path;
    private List<Vector3Int> tiles;
    PathFinder3 pf;
    HiveResources hv;
    CycleController cc;
    private int nectar = 0;
    private int pollen = 0;
    private int honey = 0;
    private int wax = 0;
    private int royalJelly = 0;
    private int startingCycle;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private EventReference beeDeathSound;
    [SerializeField] private EventReference cycleDepositSound;
    public PathFinder3 pathFinder;
    public int pathIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        // gets the tilemap
        mc = FindObjectOfType<MapController>();
        tilemap = mc.walkable;
        tiles = getAllTiles(-10, -10, 10, 10);
        pf = FindObjectOfType<PathFinder3>();
        hv = FindObjectOfType<HiveResources>();
        cc = FindObjectOfType<CycleController>();
        startingCycle = cc.currentCycle;
        path = pf.getPath();
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(moveTile(delay));
    }

    public List<Vector3Int> getAllTiles(int minX, int minY, int maxX, int maxY)
    {
        List<Vector3Int> toReturn = new List<Vector3Int>();
        for (int i = minX; i < maxX; i++)
        {
            for (int j = minY; j < maxY; j++)
            {
                Vector3Int location = new Vector3Int(i, j);
                if (tilemap.HasTile(location))
                {
                    toReturn.Add(location);
                }
            }
        }
        return toReturn;
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

    IEnumerator moveTile(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        //path = pathFinder.findPath(tilemap.WorldToCell(this.gameObject.transform.position) + Vector3Int.back, tiles[0]);
        Vector3 distance = tilemap.CellToWorld(path[pathIndex]) - new Vector3Int(0, 0, 1) - transform.position;
        StartCoroutine(moveOverTime(distance));
        //transform.position = tilemap.CellToWorld(tiles[path[pathIndex]]);
        TileBase currTile = tilemap.GetTile(path[pathIndex]);
        if (currTile.name.Equals("Tile_Beekeeper_Drop") || currTile.name.Equals("Tile_Beekeeper_Hit"))
        {
            honey++;
            playHitAnimation(path[pathIndex], "Beekeeper");
        } else if (currTile.name.Equals("Tile_Pond_Drop") || currTile.name.Equals("Tile_Pond_Hit"))
        {
            nectar++;
            playHitAnimation(path[pathIndex], "Pond");
        } else if (currTile.name.Equals("Tile_Meadow_Drop") || currTile.name.Equals("Tile_Meadow_Hit"))
        {
            pollen++;
            playHitAnimation(path[pathIndex], "Meadow");
        } else if (currTile.name.Equals("Tile_Woodland_Drop") || currTile.name.Equals("Tile_Woodland_Hit"))
        {
            wax++;
            playHitAnimation(path[pathIndex], "Woodland");
        } else if (currTile.name.Equals("Tile_Garden_Drop") || currTile.name.Equals("Tile_Garden_Hit"))
        {
            royalJelly++;
            playHitAnimation(path[pathIndex], "Garden");
        }
        
        // edges refers to the index in tiles
        // while next tile does not have an edge to the current tile
        // add the previous tile until an edge with the next tile is found
        
        if (path[pathIndex] == pf.start && pathIndex != 0)
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
            AudioController.instance.PlayOneShot(cycleDepositSound, this.transform.position);

            // Bee death
            if (startingCycle + 10 <= cc.currentCycle)
            {
                hv.bees--;
                Destroy(gameObject);
                FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Beemony", hv.bees);
                AudioController.instance.PlayOneShot(beeDeathSound, this.transform.position);
            }

            // reset path + update tiles
            path = pf.path;
            pathIndex = 0;
            
            StartCoroutine(moveTile(delay));
        } else
        {
            pathIndex += 1;
            StartCoroutine(moveTile(delay));
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
        for (int i = 0; i < 12; i++)
        {
            transform.position += distance / 12;
            yield return new WaitForSeconds(delay*(1.0f / 24.0f));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("Enemy!");
            hv.bees--;
            Destroy(gameObject);
            //FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Beemony", hv.bees);
            //AudioController.instance.PlayOneShot(beeDeathSound, this.transform.position);
            //AudioController.instance.PlayOneShot(denyResourceSound, this.transform.position);

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
