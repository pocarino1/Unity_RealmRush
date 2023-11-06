using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] private Color BaseColor = Color.white;
    [SerializeField] private Color BlockedColor = Color.gray;
    [SerializeField] private Color ExploredColor = Color.yellow;
    [SerializeField] private Color PathColor = new Color(1.0f, 0.5f, 0.0f);

    private TextMeshPro CoordinateLabel = null;
    private Vector2Int Coordinates = new Vector2Int();
    private Tile TileWaypoint = null;
    private GridManager GameGridManager = null;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        CoordinateLabel = GetComponent<TextMeshPro>();
        CoordinateLabel.enabled = false;
        
        TileWaypoint = GetComponentInParent<Tile>();
        GameGridManager = FindObjectOfType<GridManager>();

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
        Coordinates.x = Mathf.RoundToInt(transform.parent.position.x / TileSize.Width);
        Coordinates.y = Mathf.RoundToInt(transform.parent.position.z / TileSize.Height);

        CoordinateLabel.text = Coordinates.x + "," + Coordinates.y;
    }

    private void UpdateObjectName()
    {
        transform.parent.name = Coordinates.ToString();
    }

    private void UpdateCoordinatesColor()
    {
        if(GameGridManager != null)
        {
            Node node = GameGridManager.GetNode(Coordinates);
            if(node != null)
            {
                CoordinateLabel.color = node.isWalkable ? BaseColor : BlockedColor;

                if(node.isExplored)
                {
                    CoordinateLabel.color = ExploredColor;
                }

                if(node.isPath)
                {
                    CoordinateLabel.color = PathColor;
                }
                
                return;
            }

            CoordinateLabel.color = BlockedColor;
        }
        else
        {
            CoordinateLabel.color = TileWaypoint != null && TileWaypoint.Placeable ? BaseColor : BlockedColor;
        }
    }

    private void ToggleLabels()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            CoordinateLabel.enabled = !CoordinateLabel.enabled;
        }
    }
}
