using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways] // Ensures the script runs in edit mode and play mode
public class PathFindingGrid : MonoBehaviour
{
    [Header("Tilemap Settings")]
    public Tilemap obstacleTilemap;

    [Header("Grid Settings")]
    public int width = 10;
    public int height = 10;
    public bool allowDiagonals = true;

    [HideInInspector]
    public Node[,] grid;

    private int gridOriginX;
    private int gridOriginY;


    // Initialize the grid (call in Awake and OnValidate)
    private void InitializeGrid()
    {
        if (obstacleTilemap == null || width <= 0 || height <= 0)
        {
            grid = null;
            return;
        }

        obstacleTilemap.CompressBounds();
        BoundsInt bounds = obstacleTilemap.cellBounds;

        width = bounds.size.x;
        height = bounds.size.y;

        grid = new Node[width, height];

        gridOriginX = bounds.xMin;
        gridOriginY = bounds.yMin;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int tilePos = new Vector3Int(bounds.xMin + x, bounds.yMin + y, 0);
                bool walkable = !obstacleTilemap.HasTile(tilePos);
                Vector3 worldPos = obstacleTilemap.GetCellCenterWorld(tilePos);
                grid[x, y] = new Node(worldPos, walkable, x, y);

                if (!walkable)
                {
                    Debug.DrawRay(worldPos, Vector3.up * 0.5f, Color.red, 10f);
                }
            }
        }

    }

    // Awake runs in play mode
    private void Awake()
    {
        InitializeGrid();
    }

#if UNITY_EDITOR
    // OnValidate runs whenever something changes in inspector (edit mode friendly)
    private void OnValidate()
    {
        InitializeGrid();
    }
#endif

    /*

    // Draw the grid in Scene view
    private void OnDrawGizmosSelected()
    {
        if (grid == null) return;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Node node = grid[x, y];
                if (node == null) continue;

                Gizmos.color = node.walkable ? Color.white : Color.red;
                Gizmos.DrawWireCube(node.worldPosition, Vector3.one * 0.9f);

#if UNITY_EDITOR
                Handles.Label(node.worldPosition, $"({x},{y})");
#endif
            }
        }
    }
    */

    // --- Utility Methods ---
    public List<Node> GetNeighbors(Node node)
    {
        List<Node> neighbors = new List<Node>();
        int x = node.gridX;
        int y = node.gridY;

        if (x + 1 < width) neighbors.Add(grid[x + 1, y]);
        if (x - 1 >= 0) neighbors.Add(grid[x - 1, y]);
        if (y + 1 < height) neighbors.Add(grid[x, y + 1]);
        if (y - 1 >= 0) neighbors.Add(grid[x, y - 1]);

        if (allowDiagonals)
        {
            if (x + 1 < width && y + 1 < height) neighbors.Add(grid[x + 1, y + 1]);
            if (x + 1 < width && y - 1 >= 0) neighbors.Add(grid[x + 1, y - 1]);
            if (x - 1 >= 0 && y + 1 < height) neighbors.Add(grid[x - 1, y + 1]);
            if (x - 1 >= 0 && y - 1 >= 0) neighbors.Add(grid[x - 1, y - 1]);
        }

        return neighbors;
    }

    public int GetDistance(Node a, Node b)
    {
        int dx = Mathf.Abs(a.gridX - b.gridX);
        int dy = Mathf.Abs(a.gridY - b.gridY);

        return allowDiagonals
            ? 14 * Mathf.Min(dx, dy) + 10 * Mathf.Abs(dx - dy) // Octile distance
            : dx + dy; // Manhattan
    }

    public Node GetNodeFromWorldPosition(Vector3 worldPos)
    {
        // Remove the offset before calculating the cell
        Vector3 localPos = worldPos;
        Vector3Int cell = obstacleTilemap.WorldToCell(localPos);

        int x = cell.x - gridOriginX;
        int y = cell.y - gridOriginY;

        if (x < 0 || x >= width || y < 0 || y >= height) return null;
        return grid[x, y];
    }
}