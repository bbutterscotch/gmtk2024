using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder : MonoBehaviour
{

    private MapController mc;
    private Tilemap tilemap;
    [SerializeField] private float delay = 100f;
    private List<Vector3Int> tiles;
    private List<int> path;
    private List<List<int>> edges;

    // Start is called before the first frame update
    void Start()
    {
        mc = FindObjectOfType<MapController>();
        tilemap = mc.walkable;
        path = new List<int>();
        tiles = new List<Vector3Int>();
        getAllTiles(-10, 10, -10, 10);
        int start = tiles.FindIndex(a => (a.x == mc.startTile.x && a.y == mc.startTile.y));
        StartCoroutine(findNewPath(delay, start));
    }

    IEnumerator findNewPath(float delay, int start)
    {
        getAllTiles(-10, 10, -10, 10);
        start = tiles.FindIndex(a => (a.x == mc.startTile.x && a.y == mc.startTile.y));
        path = new List<int>();
        defineGraph(start);
        path.Add(start);
        int pathIndex = 0;
        for (int i = 0; i < path.Count; i++)
        {
            int count = 1;
            while (pathIndex + count < path.Count && !edges[path[pathIndex + count]].Contains(path[pathIndex - 1 + count]))
            {

                path.Insert(pathIndex + count, path[pathIndex - count]);
                Debug.Log(count);
                count++;
                i--;
            }
            pathIndex += 1;
        }
        string toPrint = "";
        foreach (int i in path)
        {
            toPrint += i + ": " + tiles[i] + " --> ";
        }
        Debug.Log(toPrint);
        yield return new WaitForSeconds(delay);
        
        StartCoroutine(findNewPath(delay, start));
    }

    void getAllTiles(int minX, int maxX, int minY, int maxY)
    {
        tiles = new List<Vector3Int>();
        for (int i = minX; i < maxX; i++)
        {
            for (int j = minY; j < maxY; j++)
            {
                if (tilemap.HasTile(new Vector3Int(i, j)))
                {
                    tiles.Add(new Vector3Int(i, j));
                }
            }
        }
    }

    void DFSRec(List<List<int>> adj, bool[] visited, int s)
    {
        visited[s] = true;
        path.Add(s);
        foreach (int i in adj[s])
        {
            if (!visited[i])
            {
                DFSRec(adj, visited, i);
            }
        }
    }

    void PerformDFS(List<List<int>> adj, int s)
    {
        bool[] visited = new bool[adj.Count];
        DFSRec(adj, visited, s);
    }

    void defineGraph(int start)
    {
        int V = tiles.Count;
        edges = new List<List<int>>(V);
        foreach (Vector3Int tile in tiles)
        {
            edges.Add(getEdges(tile));
        }

        //for (int i = 0; i < edges.Count; i++)
        //{
        //    AddEdge(adj, edges[i][0], edges[i][1]);
        //}
        PerformDFS(edges, start);

    }

    List<int> getEdges(Vector3Int start)
    {
        List<int> temp = new List<int>();
        Vector3Int[] toCheck = new Vector3Int[6];
        // if even
        if (start.y % 2 == 0)
        {
            toCheck[0] = start + Vector3Int.up;
            toCheck[1] = start + Vector3Int.right;
            toCheck[2] = start + Vector3Int.down;
            toCheck[3] = start + Vector3Int.down + Vector3Int.left;
            toCheck[4] = start + Vector3Int.left;
            toCheck[5] = start + Vector3Int.up + Vector3Int.left;
        }
        else
        {
            toCheck[0] = start + Vector3Int.up + Vector3Int.right;
            toCheck[1] = start + Vector3Int.right;
            toCheck[2] = start + Vector3Int.down + Vector3Int.right;
            toCheck[3] = start + Vector3Int.down;
            toCheck[4] = start + Vector3Int.left;
            toCheck[5] = start + Vector3Int.up;
        }
        for (int i = 0; i < 6; i++)
        {
            if (tilemap.HasTile(toCheck[i]))
            {
                temp.Add(tiles.FindIndex(a => (a.x == toCheck[i].x && a.y == toCheck[i].y)));
            }
        }
        return temp;
    }

    public List<int> getPath()
    {
        return path;
    }

    public List<Vector3Int> getTiles()
    {
        return tiles;
    }
}
