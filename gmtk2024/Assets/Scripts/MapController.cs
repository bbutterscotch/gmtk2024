using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using FMODUnity;

public class MapController : MonoBehaviour
{

    [SerializeField] public Tilemap walkable;
    [SerializeField] public Tilemap unwalkable;
    [SerializeField] public Tilemap spriteMap;
    [SerializeField] public AnimatedTile entranceTile;
    [SerializeField] public AnimatedTile pondDropTile;
    [SerializeField] public AnimatedTile pondHitTile;
    [SerializeField] public AnimatedTile meadowDropTile;
    [SerializeField] public AnimatedTile meadowHitTile;
    [SerializeField] public AnimatedTile beekeeperDropTile;
    [SerializeField] public AnimatedTile beekeeperHitTile;
    [SerializeField] public AnimatedTile woodlandDropTile;
    [SerializeField] public AnimatedTile woodlandHitTile;
    [SerializeField] public AnimatedTile gardenDropTile;
    [SerializeField] public AnimatedTile gardenHitTile;
    [SerializeField] public AnimatedTile forestHitTile;
    [SerializeField] public AnimatedTile apiaryHitTile;
    [SerializeField] public AnimatedTile parkHitTile;
    [SerializeField] public Tile nurseryTile;
    [SerializeField] public AnimatedTile queenbeedropTile;
    [SerializeField] public Tile queenbeeTile;
    [SerializeField] public AnimatedTile queenbeeIdleTile;
    HiveResources hv;

    PathFinder3 pf;

    [SerializeField] private EventReference music;
    [SerializeField] private EventReference queenStartSound;

    public Vector3Int center = new Vector3Int(0, 0, 0);
    public Vector3Int startTile = new Vector3Int(0, 2, 0);

    private void Awake()
    {
        hv = FindFirstObjectByType<HiveResources>();
        pf = FindFirstObjectByType<PathFinder3>();
    }


    // Start is called before the first frame update
    void Start()
    {
        

        // 12 tiles to start + 1 nursery tile
        unwalkable.SetTile(center, queenbeedropTile);
        AudioController.instance.PlayOneShot(queenStartSound, this.transform.position);
        unwalkable.SetTile(center + Vector3Int.up, nurseryTile);
        hv.nurseryTiles++;
        walkable.SetTile(startTile, entranceTile);
        pf.placeTile(startTile);

        List<Vector3Int> positions = new List<Vector3Int> {startTile + Vector3Int.right, startTile + Vector3Int.left, 
            startTile + new Vector3Int(0, -4, 0), startTile + new Vector3Int(-1, -4, 0), startTile + new Vector3Int(1, -4, 0), 
            startTile + new Vector3Int(1, -1, 0), startTile + new Vector3Int(-2, -1, 0), startTile + new Vector3Int(2, -2, 0), 
            startTile + new Vector3Int(-2, -2, 0), startTile + new Vector3Int(1, -3, 0), startTile + new Vector3Int(-2, -3, 0)};
        for (int i = 0; i < 11; i++)
        {
            int posIndex = Random.Range(0, positions.Count);
            if (i < 2)
            {
                walkable.SetTile(positions[posIndex], gardenDropTile);
                AudioController.instance.SetParameter(music, "Garden", 1, this.transform.position);
                hv.gardenTiles++;
            } else if (i < 4)
            {
                walkable.SetTile(positions[posIndex], woodlandDropTile);
                AudioController.instance.SetParameter(music, "Woodland", 1, this.transform.position);
                hv.woodlandTiles++;
            } else if (i < 6)
            {
                walkable.SetTile(positions[posIndex], meadowDropTile);
                AudioController.instance.SetParameter(music, "Meadow", 1, this.transform.position);
                hv.meadowTiles++;
            } else if (i < 9)
            {
                walkable.SetTile(positions[posIndex], pondDropTile);
                AudioController.instance.SetParameter(music, "Pond", 1, this.transform.position);
                hv.pondTiles++;
            } else
            {
                walkable.SetTile(positions[posIndex], beekeeperDropTile);
                AudioController.instance.SetParameter(music, "Beekeeper", 1, this.transform.position);
                hv.beekeeperTiles++;
            }
            pf.placeTile(positions[posIndex]);
            positions.RemoveAt(posIndex);
        }
        StartCoroutine(UpdateQueenTile());
    }


    IEnumerator UpdateQueenTile()
    {
        yield return new WaitForSeconds(queenbeedropTile.m_AnimatedSprites.Length / queenbeedropTile.m_MinSpeed);
        unwalkable.SetTile(center, queenbeeTile);
        StartCoroutine(IdleQueenTile());
    }

    IEnumerator IdleQueenTile()
    {
        yield return new WaitForSeconds(5f);
        unwalkable.SetTile(center, queenbeeIdleTile);
        yield return new WaitForSeconds(queenbeeIdleTile.m_AnimatedSprites.Length / queenbeeIdleTile.m_MinSpeed);
        unwalkable.SetTile(center, queenbeeTile);
        StartCoroutine(IdleQueenTile());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
