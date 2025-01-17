using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public class AStar
{
    public Vector3Int center = new Vector3Int(0, 0);

    public Dictionary<Vector3Int, Vector3Int> search(Graph g, Vector3Int start, Vector3Int goal)
    {
        PriorityQueue<Vector3Int, double> frontier = new PriorityQueue<Vector3Int, double>();
        frontier.Enqueue(start, 0);
        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        Dictionary<Vector3Int, double> costSoFar = new Dictionary<Vector3Int, double>();
        cameFrom.Add(start, new Vector3Int());
        costSoFar.Add(start, 0f);
        while (frontier.Count > 0) {
            Vector3Int current = frontier.Dequeue();
            if (current == goal)
            {
                
                break;
                
            }

            foreach (Vector3Int entry in g.connections[current]) {
                Vector3Int next = entry;
                double dist = getDistance(current, next);
                double newCost = costSoFar[current] + dist;
                if (!(costSoFar.ContainsKey(next)) || newCost < costSoFar[next]) {
                    costSoFar[next] = newCost;
                    frontier.Enqueue(next, dist);
                    cameFrom[next] = current;
                }
                
            }

        }
        //Debug.Log(cameFrom.ToString());
        return cameFrom;

    }

    public List<Vector3Int> reconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int start, Vector3Int goal)
    {
        Vector3Int current = goal;
        List<Vector3Int> path = new List<Vector3Int>();
        if (!cameFrom.ContainsKey(current))
        {
            return path;
        }
        while (!current.Equals(start))
        {
            path.Add(current);
            current = cameFrom[current];
        }

        path.Add(start);
        List<Vector3Int> newPath = new List<Vector3Int>();
        for (int i = path.Count - 1; i >= 0; i--)
        {
            newPath.Add(path[i]);
        }

        return newPath;
    }

    public double getDistance(Vector3Int first, Vector3Int second)
    {
        double distance = Math.Sqrt(Math.Pow((second.x - first.x), 2) + Math.Pow((second.y - first.y), 2));
        return distance;
    }

    public string PathToString(List<Vector3Int> path) {
        string toReturn = "";
        for (int i = 0; i < path.Count; i++) {
            toReturn += "[" + path[i].x.ToString() + ", " + path[i].y.ToString() + "], ";
        }
        return toReturn;
    }
}