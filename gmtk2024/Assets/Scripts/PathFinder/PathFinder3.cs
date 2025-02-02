using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.EventSystems.EventTrigger;

public class PathFinder3 : MonoBehaviour
{
    Dictionary<int, Vector3Int> tilesIndex;
    Dictionary<Vector3Int, int> tiles;
    Dictionary<int, List<int>> graph;
    private MapController mc;
    private Tilemap tilemap;
    [SerializeField] public float delay = 2f;
    [SerializeField] public Vector3Int start = new Vector3Int(0, 2);
    [SerializeField] public Vector3Int goal = new Vector3Int(-1, 2);
    ShortestPath sp;
    public List<Vector3Int> path;

    private void Awake()
    {
        mc = FindFirstObjectByType<MapController>();
        tiles = new Dictionary<Vector3Int, int>();
        tilesIndex = new Dictionary<int, Vector3Int>();
        graph = new Dictionary<int, List<int>>();
        tilemap = mc.walkable;
        sp = new ShortestPath();
    }

    private void Start()
    {
        
    }

    // Start is called before the first frame update
    public IEnumerator getPath()
    {
        
        path = findPath(tilemap);
        //Debug.Log(GraphToString(graph, tilesIndex));
        yield return null;
    }

    public List<Vector3Int> findPath(Tilemap tm)
    {
        //var tuple = getTiles(tm);
        //tiles = tuple.Item2;
        //Debug.Log("Tiles: " + tiles.Count.ToString());
        //tilesIndex = tuple.Item1;
        //Debug.Log("Tiles Index: " + tilesIndex.Count.ToString());
        //Dictionary<int, List<int>> graph = generateGraph(tm, tilesIndex, tiles);
        //Debug.Log("Graph: " + graph.Count.ToString());
        //Debug.Log(GraphToString(graph, tilesIndex));
        float startTime = Time.realtimeSinceStartup;
        var cameFrom = sp.Search(graph);
        float endTime = Time.realtimeSinceStartup;
        float searchTime = endTime - startTime;
        Debug.Log("Search Time: " + searchTime.ToString() + "s");
        List<Vector3Int> path = new List<Vector3Int>();
        if (cameFrom.Item2 != null)
        {
            float startTime2 = Time.realtimeSinceStartup;
            List<int> pathIndex = sp.reconstructPath(cameFrom.Item1, cameFrom.Item2);
            float endTime2 = Time.realtimeSinceStartup;
            float reconstructTime = endTime2 - startTime2;
            Debug.Log("Reconstruction Time: " + reconstructTime.ToString() + "s");
            path = pathIndexToLocation(pathIndex, tilesIndex);
            //Debug.Log(printPath(path));
            path = shiftPath(path, start, goal); // O(n)
            path.Add(start); // O(1)
            Debug.Log(printPath(path)); // O(n)
        }
        return path;
    }

    public List<Vector3Int> shiftPath(List<Vector3Int> path, Vector3Int start, Vector3Int goal)
    {
        List<Vector3Int> toReturn = new List<Vector3Int>(new Vector3Int[path.Count]);
        int startIndex = path.FindIndex(x => x == start);
        int shift = 0;
        if (path[startIndex + 1] == goal)
        {
            // shift right
            shift = path.Count - startIndex - 1;
            for (int i = 0; i < path.Count; i++)
            {
                toReturn[(i + shift) % path.Count] = path[i];
            }
            toReturn.Reverse();

        } else
        {
            // shift left
            shift = startIndex - path.Count + 1;
            for (int i = 0; i < path.Count; i++)
            {
                toReturn[(i - shift) % path.Count] = path[i];
            }
        }

        
        return toReturn;
    }

    public List<Vector3Int> pathIndexToLocation(List<int> path, Dictionary<int, Vector3Int> tilesIndex)
    {
        List<Vector3Int> toReturn = new List<Vector3Int>();
        foreach (int i in path)
        {
            toReturn.Add(tilesIndex[i]);
        }
        return toReturn;
    }

    public (Dictionary<int, Vector3Int>, Dictionary<Vector3Int, int>) getTiles(Tilemap tm)
    {
        Dictionary<int, Vector3Int> toReturn1 = new Dictionary<int, Vector3Int>();
        Dictionary<Vector3Int, int> toReturn2 = new Dictionary<Vector3Int, int>();
        int count = 0;
        var bounds = tm.cellBounds;
        for (int i = bounds.xMin; i < bounds.xMax; i++)
        {
            for (int j = bounds.yMin; j < bounds.yMax; j++)
            {
                Vector3Int pos = new Vector3Int(i, j);
                if (tm.HasTile(pos))
                {
                    toReturn1[count] = pos;
                    toReturn2[pos] = count;
                    count++;
                }
            }
        }
        return (toReturn1, toReturn2);
    }

    public Dictionary<int, List<int>> generateGraph(Tilemap tm, Dictionary<int, Vector3Int> tilesIndex, Dictionary<Vector3Int, int> tiles)
    {
        Dictionary<int, List<int>> toReturn = new Dictionary<int, List<int>>();

        // for tile in tiles
        // get the neighbors

        foreach (KeyValuePair<int, Vector3Int> entry in tilesIndex)
        {
            List<Vector3Int> neighbors = getNeighbors(tm, entry.Value);
            List<int> nodes = new List<int>();
            foreach (Vector3Int pos in neighbors)
            {
                nodes.Add(tiles[pos]);
            }
            toReturn[entry.Key] = nodes;
        }


        return toReturn;
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

    public string GraphToString(Dictionary<int, List<int>> graph, Dictionary<int, Vector3Int> tilesIndex)
    {
        string toReturn = string.Empty;
        foreach (KeyValuePair<int, List<int>> entry in graph)
        {

            toReturn += Vector3IntToString(tilesIndex[entry.Key]) + " --> ";
            foreach (int pos in entry.Value)
            {
                toReturn += Vector3IntToString(tilesIndex[pos]) + ", ";
            }
            toReturn += "\n";
        }
        return toReturn;
    }

    public string Vector3IntToString(Vector3Int a)
    {
        return "[" + a.x.ToString() + ", " + a.y.ToString() + "]";
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

    public bool placeTile(Vector3Int location)
    {
        if (tiles.ContainsKey(location))
        {
            Debug.LogError("Duplicate Tile");
            return false;
        }
        if (!tilemap.HasTile(location))
        {
            Debug.LogError("Tile not in TileMap");
            return false;
        }
        // Add to list of all tiles
        int tileIndex = tiles.Count;
        tiles.Add(location, tileIndex);
        tilesIndex.Add(tileIndex, location);
        // Get all the neighbors
        List<Vector3Int> neighbors = getNeighbors(tilemap, location);
        List<int> nodes = new List<int>();
        foreach (Vector3Int pos in neighbors)
        {
            nodes.Add(tiles[pos]);
            if (tiles.ContainsKey(pos))
            {
                graph[tiles[pos]].Add(tileIndex);
            }
        }
        graph[tileIndex] = nodes;
        return true;

    }
}
