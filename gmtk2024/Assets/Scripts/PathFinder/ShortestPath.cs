using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShortestPath
{
    public (Dictionary<int[], int[]>, int[]) Search(Dictionary<int, List<int>> graph)
    {
        int counter = 0;
        int n = graph.Count;
        int allVisited = (1 << n) - 1;
        Queue <int[]> queue = new Queue <int[]>();
        HashSet<int> visited = new HashSet<int>();

        Dictionary<int[], int[]> cameFrom = new Dictionary<int[], int[]>();

        for (int i = 0; i < n; i++) // O(n)
        {
            queue.Enqueue(new int[] { 1 << i, i, 0 });
            visited.Add((1 << i) * 16 + 1);
        }

        while (queue.Count > 0)
        {
            int[] cur = queue.Dequeue();
            
            if (cur[0] == allVisited)
            {
                Debug.Log(cur[2].ToString());
                Debug.Log("Counter: " + counter.ToString());
                return (cameFrom, cur);
            }

            foreach (int neighbor in graph[cur[1]])
            {
                counter++;
                int newMask = cur[0] | (1 << neighbor);
                int hashValue = newMask * 16 + neighbor;

                if (!visited.Contains(hashValue))
                {
                    
                    visited.Add(hashValue);
                    var mask = new int[] { newMask, neighbor, cur[2] + 1 };
                    cameFrom[mask] = cur;
                    queue.Enqueue(mask);
                }
            }
        }
        return (cameFrom, null);
    }

    public List<int> reconstructPath(Dictionary<int[], int[]> cameFrom, int[] end)
    {
        int[] current = end;
        List<int> path = new List<int>();
        if (!cameFrom.ContainsKey(current))
        {
            return path;
        }
        while (current[2] != 0)
        {
            path.Add(current[1]);
            current = cameFrom[current];
        }

        path.Add(current[1]);
        List<int> newPath = new List<int>();
        for (int i = path.Count - 1; i >= 0; i--)
        {
            newPath.Add(path[i]);
        }

        return newPath;
    }



}
