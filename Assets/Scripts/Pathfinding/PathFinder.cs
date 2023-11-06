using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
    [SerializeField] private Vector2Int startCoordinates = new Vector2Int();
    public Vector2Int StartCoordinates
    {
        get
        {
            return startCoordinates;
        }
    }

    [SerializeField] private Vector2Int destinationCoordinates = new Vector2Int();
    public Vector2Int DestinationCoordinates
    {
        get
        {
            return destinationCoordinates;
        }
    }

    private Node StartNode = null;
    private Node DestinationNode = null;
    private Node CurrentSearchNode = null;
    private Queue<Node> Frontier = new Queue<Node>();
    private Dictionary<Vector2Int, Node> Reached = new Dictionary<Vector2Int, Node>();

    private Vector2Int[] directions = { Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down };
    private GridManager GameGridManager = null;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        GameGridManager = FindObjectOfType<GridManager>();
        if (GameGridManager != null)
        {
            StartNode = GameGridManager.Grid[startCoordinates];
            StartNode.isWalkable = true;

            DestinationNode = GameGridManager.Grid[destinationCoordinates];
            DestinationNode.isWalkable = true;

            GetNewPath();
        }
    }

    public List<Node> GetNewPath()
    {
        return GetNewPath(startCoordinates);
    }

    public List<Node> GetNewPath(Vector2Int Coordinates)
    {
        GameGridManager.ResetNode();
        BreadthFirstSearch(Coordinates);
        return BuildPath();
    }

    private void ExploreNeighbors()
    {
        List<Node> neighbors = new List<Node>();
        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborCoords = CurrentSearchNode.coordinates + direction;
            if (GameGridManager != null && GameGridManager.Grid.ContainsKey(neighborCoords))
            {
                neighbors.Add(GameGridManager.Grid[neighborCoords]);
            }
        }

        foreach (Node neighbor in neighbors)
        {
            if (!Reached.ContainsKey(neighbor.coordinates) && neighbor.isWalkable)
            {
                neighbor.connectedTo = CurrentSearchNode;

                Reached.Add(neighbor.coordinates, neighbor);
                Frontier.Enqueue(neighbor);
            }
        }
    }

    private void BreadthFirstSearch(Vector2Int Coordinates)
    {
        Frontier.Clear();
        Reached.Clear();

        bool isRunning = true;

        Frontier.Enqueue(GameGridManager.Grid[Coordinates]);
        Reached.Add(Coordinates, GameGridManager.Grid[Coordinates]);

        while (Frontier.Count > 0 && isRunning)
        {
            CurrentSearchNode = Frontier.Dequeue();
            if (CurrentSearchNode != null)
            {
                CurrentSearchNode.isExplored = true;
                ExploreNeighbors();

                if (CurrentSearchNode.coordinates == destinationCoordinates)
                {
                    isRunning = false;
                }
            }
        }
    }

    private List<Node> BuildPath()
    {
        List<Node> Path = new List<Node>();
        if (DestinationNode != null)
        {
            Node CurrentNode = DestinationNode;

            Path.Add(CurrentNode);
            CurrentNode.isPath = true;

            while (CurrentNode.connectedTo != null)
            {
                CurrentNode = CurrentNode.connectedTo;
                Path.Add(CurrentNode);
                CurrentNode.isPath = true;
            }

            Path.Reverse();
        }

        return Path;
    }

    public bool WillBlockPath(Vector2Int coordinates)
    {
        if (GameGridManager.Grid.ContainsKey(coordinates))
        {
            bool PrevWalkable = GameGridManager.Grid[coordinates].isWalkable;
            GameGridManager.Grid[coordinates].isWalkable = false;
            List<Node> NewPath = GetNewPath(coordinates);
            GameGridManager.Grid[coordinates].isWalkable = PrevWalkable;

            if (NewPath.Count <= 1)
            {
                GetNewPath();
                return true;
            }
        }

        return false;
    }

    public void NotifyReceivers()
    {
        BroadcastMessage("RecalculatePath", false, SendMessageOptions.DontRequireReceiver);
    }
}
