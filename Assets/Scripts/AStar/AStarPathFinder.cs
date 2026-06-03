using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinder : MonoBehaviour
{
    public PathFindingGrid grid;

    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.GetNodeFromWorldPosition(startPos);
        Node targetNode = grid.GetNodeFromWorldPosition(targetPos);

        if (startNode == null || targetNode == null || !targetNode.walkable)
        {
            return null; // Can't path to target
        }

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        openSet.Add(startNode);

        startNode.gCost = 0;
        startNode.hCost = grid.GetDistance(startNode, targetNode);
        startNode.parent = null;

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            // Get node with lowest fCost
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost ||
                    (openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            // Reached target
            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbor in grid.GetNeighbors(currentNode))
            {
                if (!neighbor.walkable || closedSet.Contains(neighbor)) continue;

                int tentativeGCost = currentNode.gCost + grid.GetDistance(currentNode, neighbor);
                if (tentativeGCost < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = tentativeGCost;
                    neighbor.hCost = grid.GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return null; // No path found
    }

    private List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }
}