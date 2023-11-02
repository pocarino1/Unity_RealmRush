using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
public class CoordinateLabeler : MonoBehaviour
{
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
        }

        UpdateCoordinatesColor();
        ToggleLabels();
    }

    private void DisplayCoordinates()
    {
        Coordinates.x = Mathf.RoundToInt(transform.parent.position.x / UnityEditor.EditorSnapSettings.move.x);
        Coordinates.y = Mathf.RoundToInt(transform.parent.position.z / UnityEditor.EditorSnapSettings.move.z);

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
