using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 5.0f)] private float MovingSpeed = 1.0f;

    private List<Waypoint> Path = new List<Waypoint>();
    private Enemy EnemyClass = null;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        EnemyClass = GetComponent<Enemy>();
    }

    void OnEnable()
    {
        FindPath();
        ReturnToStart();
    
        StartCoroutine(FollowPath());
    }

    private void FindPath()
    {
        Path.Clear();

        GameObject TileWaypointParent = GameObject.FindGameObjectWithTag("Path");
        foreach(Transform Child in TileWaypointParent.transform)
        {
            Waypoint ChildWaypoint = Child.GetComponent<Waypoint>();
            if(ChildWaypoint != null)
            {
                Path.Add(ChildWaypoint);
            }
        }
    }

    private void ReturnToStart()
    {
        transform.position = Path[0].transform.position;
    }

    IEnumerator FollowPath()
    {
        foreach(Waypoint waypoint in Path)
        {
            Vector3 StartPosition = transform.position;
            Vector3 EndPosition = waypoint.transform.position;
            float TravelPercent = 0.0f;

            transform.LookAt(EndPosition);

            while(TravelPercent < 1.0f)
            {
                TravelPercent += Time.deltaTime * MovingSpeed;
                transform.position = Vector3.Lerp(StartPosition, EndPosition, TravelPercent);

                yield return new WaitForEndOfFrame();
            }
        }

        FinishPath();
    }

    private void FinishPath()
    {
        if(EnemyClass != null)
        {
            EnemyClass.StealGold();
        }

        gameObject.SetActive(false);
    }
}
