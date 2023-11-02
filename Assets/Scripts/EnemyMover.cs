using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private List<Waypoint> Path = new List<Waypoint>();
    [SerializeField] [Range(0.0f, 5.0f)] private float MovingSpeed = 1.0f;

    void OnEnable()
    {
        FindPath();
        ReturnToStart();
    
        StartCoroutine(FollowPath());
    }

    private void FindPath()
    {
        Path.Clear();

        GameObject[] TileWaypoints = GameObject.FindGameObjectsWithTag("Path");
        foreach(GameObject waypoint in TileWaypoints)
        {
            Path.Add(waypoint.GetComponent<Waypoint>());
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

        gameObject.SetActive(false);
    }
}
