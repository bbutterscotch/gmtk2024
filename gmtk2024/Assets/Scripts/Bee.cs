using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using FMODUnity;

public class Bee : MonoBehaviour
{
    private MapController mc;
    private Tilemap tilemap;
    [SerializeField] public float delay = 2.0f;
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
    private bool mite;

    // Start is called before the first frame update
    void Start()
    {
        // gets the tilemap
        mc = FindObjectOfType<MapController>();
        tilemap = mc.walkable;
        pf = FindObjectOfType<PathFinder3>();
        hv = FindObjectOfType<HiveResources>();
        cc = FindFirstObjectByType<CycleController>();
        startingCycle = cc.currentCycle;
        path = pf.path;
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(moveTile(delay));
    }

    void playHitAnimation(Vector3Int location, string tileName)
    {
        MiteController[] mites = FindObjectsOfType<MiteController>();
        for (int i = 0; i < mites.Length; i++) { 
            if (tilemap.WorldToCell(mites[i].transform.position) == location)
            {
                return;
            }
        }
        
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
        } else if (tileName == "Forest")
        {
            at = mc.forestHitTile;
        } else if (tileName == "Apiary")
        {
            at = mc.apiaryHitTile;
        } else if (tileName == "Park")
        {
            at = mc.parkHitTile;
        }
        tilemap.SetTile(location, at);
        tilemap.SetAnimationFrame(location, 0);
        //yield return new WaitForSeconds(at.m_AnimatedSprites.Length / at.m_MinSpeed);
        //tilemap.SetTile(location, t);

    }

    public IEnumerator moveTile(float delayTime)
    {
        if (!cc.pause)
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
            }
            else if (currTile.name.Equals("Tile_Pond_Drop") || currTile.name.Equals("Tile_Pond_Hit"))
            {
                nectar++;
                playHitAnimation(path[pathIndex], "Pond");
            }
            else if (currTile.name.Equals("Tile_Meadow_Drop") || currTile.name.Equals("Tile_Meadow_Hit"))
            {
                pollen++;
                playHitAnimation(path[pathIndex], "Meadow");
            }
            else if (currTile.name.Equals("Tile_Woodland_Drop") || currTile.name.Equals("Tile_Woodland_Hit"))
            {
                wax++;
                playHitAnimation(path[pathIndex], "Woodland");
            }
            else if (currTile.name.Equals("Tile_Garden_Drop") || currTile.name.Equals("Tile_Garden_Hit"))
            {
                royalJelly++;
                playHitAnimation(path[pathIndex], "Garden");
            }
            else if (currTile.name.Equals("Tile_Apiary_Upgrade") || currTile.name.Equals("Tile_Apiary_Hit"))
            {
                honey += 2;
                playHitAnimation(path[pathIndex], "Apiary");
            }
            else if (currTile.name.Equals("Tile_Forest_Upgrade") || currTile.name.Equals("Tile_Forest_Hit"))
            {
                wax += 2;
                playHitAnimation(path[pathIndex], "Forest");
            }
            else if (currTile.name.Equals("Tile_Pond_Upgrade") || currTile.name.Equals("Tile_Garden_Upgrade") || currTile.name.Equals("Tile_Meadow_Upgrade") || currTile.name.Equals("Tile_Park_Hit"))
            {
                nectar++;
                pollen++;
                royalJelly++;
                playHitAnimation(path[pathIndex], "Park");
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
            }
            else
            {
                pathIndex += 1;
                StartCoroutine(moveTile(delay));
            }
        } else
        {
            yield return null;
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
            //Debug.Log("Enemy!");
            hv.bees--;
            Destroy(gameObject);
            FMODUnity.RuntimeManager.StudioSystem.setParameterByName("Beemony", hv.bees);
            AudioController.instance.PlayOneShot(beeDeathSound, this.transform.position);
            //AudioController.instance.PlayOneShot(denyResourceSound, this.transform.position);

        } 
        if (collision.tag == "Mite")
        {
            mite = true;
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
        mite = false;
    }
}
