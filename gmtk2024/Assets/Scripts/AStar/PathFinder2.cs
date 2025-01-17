using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PathFinder2 : MonoBehaviour
{
    private MapController mc;
    private Tilemap tilemap;
    public Graph map;
    public AStar AStar;
    public List<Vector3Int> path;

    public List<Vector3Int> findPath(Vector3Int start, Vector3Int goal)
    {
        AStar = new AStar();
        mc = FindObjectOfType<MapController>();
        tilemap = mc.walkable;
        map = new Graph(tilemap, -10, -10, 10, 10);
        //map.connections[start].Remove(goal);
        Debug.Log(map.toString());
        if (map.connections.Count > 0)
        {
            var cameFrom = AStar.search(map, start, goal);
            path = AStar.reconstructPath(cameFrom, start, goal);
            Debug.Log(AStar.PathToString(path));
        }

        return path;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
