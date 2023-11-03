using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] private int TileWidth = 10;
    [SerializeField] private int TileHeight = 10;
    [SerializeField] private Color BaseColor = Color.white;
    [SerializeField] private Color BlockedColor = Color.gray;

    private TextMeshPro CoordinateLabel = null;
    private Vector2Int Coordinates = new Vector2Int();
    private Waypoint TileWaypoint = null;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        CoordinateLabel = GetComponent<TextMeshPro>();
        CoordinateLabel.enabled = false;
        
        TileWaypoint = GetComponentInParent<Waypoint>();

        DisplayCoordinates();
    }

    void Update()
    {
        if(!Application.isPlaying)
        {
            DisplayCoordinates();
            UpdateObjectName(); 
            CoordinateLabel.enabled = true;          
        }

        UpdateCoordinatesColor();
        ToggleLabels();
    }

    private void DisplayCoordinates()
    {
        Coordinates.x = Mathf.RoundToInt(transform.parent.position.x / TileWidth);
        Coordinates.y = Mathf.RoundToInt(transform.parent.position.z / TileHeight);

        CoordinateLabel.text = Coordinates.x + "," + Coordinates.y;
    }

    private void UpdateObjectName()
    {
        transform.parent.name = Coordinates.ToString();
    }

    private void UpdateCoordinatesColor()
    {
        CoordinateLabel.color = TileWaypoint != null && TileWaypoint.Placeable ? BaseColor : BlockedColor;
    }

    private void ToggleLabels()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            CoordinateLabel.enabled = !CoordinateLabel.enabled;
        }
    }
}
