using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Vector2Int GridSize;

    private Dictionary<Vector2Int, Node> grid = new Dictionary<Vector2Int, Node>();
    public Dictionary<Vector2Int, Node> Grid
    {
        get
        {
            return grid;
        }
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        CreateGrid();
    }

    public Node GetNode(Vector2Int coordinate)
    {
        if(grid.ContainsKey(coordinate))
        {
            return grid[coordinate];
        }
        
        return null;
    }

    public void BlockNode(Vector2Int coordinates)
    {
        if(grid.ContainsKey(coordinates))
        {
            grid[coordinates].isWalkable = false;
        }
    }

    public void ResetNode()
    {
        foreach(KeyValuePair<Vector2Int, Node> entry in Grid)
        {
            entry.Value.connectedTo = null;
            entry.Value.isExplored = false;
            entry.Value.isPath = false;
        }
    }

    public Vector2Int GetCoordinatesFromPosition(Vector3 position)
    {
        Vector2Int Coordinates = new Vector2Int();

        Coordinates.x = Mathf.RoundToInt(position.x / TileSize.Width);
        Coordinates.y = Mathf.RoundToInt(position.z / TileSize.Height);

        return Coordinates;
    }

    public Vector3 GetPositionFromCoordinates(Vector2Int coordinates)
    {
        Vector3 Position = new Vector3();
        Position.x = coordinates.x * TileSize.Width;
        Position.z = coordinates.y * TileSize.Height;

        return Position;
    }

    private void CreateGrid()
    {
        for(int x = 0 ; x < GridSize.x ; ++x)
        {
            for(int y = 0 ; y < GridSize.y ; ++y)
            {
                Vector2Int coordinates = new Vector2Int(x, y);
                grid.Add(coordinates, new Node(coordinates, true));
            }
        }
    }
}
