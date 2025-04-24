using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder4 : MonoBehaviour
{
    Dictionary<int, Vector3Int> tilesIndex;
    Dictionary<Vector3Int, int> tiles;
    public List<Vector3Int> path;
    private MapController mc;
    private Tilemap tilemap;
    [SerializeField] public Vector3Int start = new Vector3Int(0, 2);

    private void Awake()
    {
        mc = FindFirstObjectByType<MapController>();
        tilemap = mc.walkable;
        tiles = new Dictionary<Vector3Int, int>();
        tilesIndex = new Dictionary<int, Vector3Int>();
        path = new List<Vector3Int>();
        path.Add(new Vector3Int(1, 2));
        path.Add(new Vector3Int(1, 1));
        path.Add(new Vector3Int(2, 0));
        path.Add(new Vector3Int(1, -1));
        path.Add(new Vector3Int(1, -2));
        path.Add(new Vector3Int(0, -2));
        path.Add(new Vector3Int(-1, -2));
        path.Add(new Vector3Int(-2, -1));
        path.Add(new Vector3Int(-2, 0));
        path.Add(new Vector3Int(-2, 1));
        path.Add(new Vector3Int(-1, 2));
        path.Add(new Vector3Int(0, 2));
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Vector3Int> getNeighbors(Tilemap tm, Vector3Int location)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();
        Vector3Int[] toCheck = new Vector3Int[6];
        // if even
        if (location.y % 2 == 0)
        {
            toCheck[0] = location + Vector3Int.up;
            toCheck[1] = location + Vector3Int.right;
            toCheck[2] = location + Vector3Int.down;
            toCheck[3] = location + Vector3Int.down + Vector3Int.left;
            toCheck[4] = location + Vector3Int.left;
            toCheck[5] = location + Vector3Int.up + Vector3Int.left;
        }
        else
        {
            toCheck[0] = location + Vector3Int.up + Vector3Int.right;
            toCheck[1] = location + Vector3Int.right;
            toCheck[2] = location + Vector3Int.down + Vector3Int.right;
            toCheck[3] = location + Vector3Int.down;
            toCheck[4] = location + Vector3Int.left;
            toCheck[5] = location + Vector3Int.up;
        }
        for (int i = 0; i < 6; i++)
        {
            if (tm.HasTile(toCheck[i]))
            {
                neighbors.Add(toCheck[i]);
            }
        }

        return neighbors;
    }

    public void placeInitialTile(Vector3Int loc)
    {
        int tileIndex = tiles.Count;
        tiles.Add(loc, tileIndex);
        tilesIndex.Add(tileIndex, loc);
    }

    public void placeNewTile(Vector3Int loc)
    {
        int tileIndex = tiles.Count;
        tiles.Add(loc, tileIndex);
        tilesIndex.Add(tileIndex, loc);
        // Find all neighbors
        List<Vector3Int> neighbors = getNeighbors(tilemap, loc);
        int firstIndex = int.MinValue;
        foreach (Vector3Int neighbor in neighbors)
        {
            
            int currIndex = path.FindIndex(x => x == neighbor);    
            if (currIndex > firstIndex)
            {
                firstIndex = currIndex;
            }
        }
        if (firstIndex == path.Count - 1 && loc.x >= 0)
        {
            firstIndex = 0;
        }
        path.Insert(firstIndex, loc);
        if (firstIndex != 0 && !neighbors.Contains(path[firstIndex - 1]))
        {
            path.Insert(firstIndex, path[firstIndex + 1]);
        }
        
        Debug.Log(printPath(path));
    }

    public string printPath(List<Vector3Int> path)
    {
        string toReturn = "Path: ";
        foreach (Vector3Int pos in path)
        {
            toReturn += Vector3IntToString(pos) + ", ";
        }
        return toReturn;
    }

    public string Vector3IntToString(Vector3Int a)
    {
        return "[" + a.x.ToString() + ", " + a.y.ToString() + "]";
    }
}
