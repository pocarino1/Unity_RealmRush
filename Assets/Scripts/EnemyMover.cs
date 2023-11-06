using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField][Range(0.0f, 5.0f)] private float MovingSpeed = 1.0f;

    private List<Vector3> Path = new List<Vector3>();
    private Enemy EnemyClass = null;
    private GridManager GameGridManager = null;
    private PathFinder GamePathFinder = null;
    private bool IsPathFinderMode = false;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        EnemyClass = GetComponent<Enemy>();

        GameGridManager = FindObjectOfType<GridManager>();
        GamePathFinder = FindObjectOfType<PathFinder>();

        IsPathFinderMode = GameGridManager != null && GamePathFinder != null ? true : false;
    }

    void OnEnable()
    {
        if (IsPathFinderMode)
        {
            ReturnToStart();
            RecalculatePath(true);
        }
        else
        {
            StopAllCoroutines();

            RecalculatePath(true);
            ReturnToStart();

            if (Path.Count > 0)
            {
                StartCoroutine(FollowPath());
            }
        }
    }

    private void RecalculatePath(bool ResetPath)
    {
        if(IsPathFinderMode)
        {
            StopAllCoroutines();
        }

        Path.Clear();

        if (GameGridManager != null && GamePathFinder != null)
        {
            Vector2Int Coordinates = new Vector2Int();
            Coordinates = ResetPath ? GamePathFinder.StartCoordinates : GameGridManager.GetCoordinatesFromPosition(transform.position);
            List<Node> FinderPath = GamePathFinder.GetNewPath(Coordinates);
            foreach (Node node in FinderPath)
            {
                Path.Add(GameGridManager.GetPositionFromCoordinates(node.coordinates));
            }
        }
        else
        {
            GameObject TileWaypointParent = GameObject.FindGameObjectWithTag("Path");
            foreach (Transform Child in TileWaypointParent.transform)
            {
                Tile ChildWaypoint = Child.GetComponent<Tile>();
                if (ChildWaypoint != null)
                {
                    Path.Add(ChildWaypoint.transform.position);
                }
            }
        }

        if (Path.Count > 0 && IsPathFinderMode)
        {
            StartCoroutine(FollowPath());
        }
    }

    private void ReturnToStart()
    {
        transform.position = IsPathFinderMode ? GameGridManager.GetPositionFromCoordinates(GamePathFinder.StartCoordinates) : Path[0];
    }

    IEnumerator FollowPath()
    {
        int StartIndex = GameGridManager != null && GamePathFinder != null ? 1 : 0;
        for (int i = StartIndex; i < Path.Count; ++i)
        {
            Vector3 StartPosition = transform.position;
            float TravelPercent = 0.0f;

            transform.LookAt(Path[i]);

            while (TravelPercent < 1.0f)
            {
                TravelPercent += Time.deltaTime * MovingSpeed;
                transform.position = Vector3.Lerp(StartPosition, Path[i], TravelPercent);

                yield return new WaitForEndOfFrame();
            }
        }

        FinishPath();
    }

    private void FinishPath()
    {
        if (EnemyClass != null)
        {
            EnemyClass.StealGold();
        }

        gameObject.SetActive(false);
    }
}
