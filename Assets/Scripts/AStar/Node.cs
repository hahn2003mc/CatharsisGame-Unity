using System.Collections.Generic;
using UnityEngine;
public class Node
{
    public Vector3 worldPosition;
    public bool walkable;
    public int gridX;
    public int gridY;

    public int gCost;    // Cost from start node
    public int hCost;    // Heuristic cost to target
    public Node parent;  // For path retracing

    public int fCost => gCost + hCost;  // Total cost

    public Node(Vector3 _worldPos, bool _walkable, int _gridX, int _gridY)
    {
        worldPosition = _worldPos;
        walkable = _walkable;
        gridX = _gridX;
        gridY = _gridY;
    }
}