using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Graph
{
    public Dictionary<Vector3Int, List<Vector3Int>> connections;

    public Graph(Tilemap tm, int minX, int minY, int maxX, int maxY)
    {
        connections = new Dictionary<Vector3Int, List<Vector3Int>>();
        for (int i = minX; i < maxX; i++)
        {
            for (int j = minY; j < maxY; j++)
            {
                Vector3Int location = new Vector3Int(i, j);
                if (tm.HasTile(location)) {
                    connections.Add(location, getNeighbors(tm, location));
                }
            }
        }
    }

    public List<Vector3Int> getNeighbors(Tilemap tm, Vector3Int location) {
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

    public string toString()
    {
        string toReturn = string.Empty;
        foreach (KeyValuePair<Vector3Int, List<Vector3Int>> entry in connections)
        {
            toReturn += entry.Key.ToString() + " --> ";
            foreach (Vector3Int item in entry.Value)
            {
                toReturn += item.ToString() + " ";
            }
            toReturn += "\n";
        }
        return toReturn;
    }
}