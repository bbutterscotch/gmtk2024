using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapController : MonoBehaviour
{

    [SerializeField] public Tilemap walkable;
    [SerializeField] public Tilemap unwalkable;
    [SerializeField] Tile entranceTile;
    [SerializeField] Tile pondTile;
    [SerializeField] Tile meadowTile;
    [SerializeField] Tile beekeeperTile;
    [SerializeField] Tile woodlandTile;
    [SerializeField] Tile gardenTile;
    [SerializeField] Tile nurseryTile;
    [SerializeField] Tile queenbeeTile;
    HiveResources hv;

    public Vector3Int center = new Vector3Int(0, 0, 0);
    public Vector3Int startTile = new Vector3Int(0, 2, 0);


    // Start is called before the first frame update
    void Start()
    {
        hv = FindObjectOfType<HiveResources>();

        // 12 tiles to start + 1 nursery tile
        unwalkable.SetTile(center, queenbeeTile);
        unwalkable.SetTile(center + Vector3Int.up, nurseryTile);
        hv.nurseryTiles++;
        walkable.SetTile(startTile, entranceTile);

        List<Vector3Int> positions = new List<Vector3Int> {startTile + Vector3Int.right, startTile + Vector3Int.left, 
            startTile + new Vector3Int(0, -4, 0), startTile + new Vector3Int(-1, -4, 0), startTile + new Vector3Int(1, -4, 0), 
            startTile + new Vector3Int(1, -1, 0), startTile + new Vector3Int(-2, -1, 0), startTile + new Vector3Int(2, -2, 0), 
            startTile + new Vector3Int(-2, -2, 0), startTile + new Vector3Int(1, -3, 0), startTile + new Vector3Int(-2, -3, 0)};
        for (int i = 0; i < 11; i++)
        {
            int posIndex = Random.Range(0, positions.Count);
            if (i < 2)
            {
                walkable.SetTile(positions[posIndex], gardenTile);
                hv.gardenTiles++;
            } else if (i < 4)
            {
                walkable.SetTile(positions[posIndex], woodlandTile);
                hv.woodlandTiles++;
            } else if (i < 6)
            {
                walkable.SetTile(positions[posIndex], meadowTile);
                hv.meadowTiles++;
            } else if (i < 9)
            {
                walkable.SetTile(positions[posIndex], pondTile);
                hv.pondTiles++;
            } else
            {
                walkable.SetTile(positions[posIndex], beekeeperTile);
                hv.beekeeperTiles++;
            }
            positions.RemoveAt(posIndex);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
