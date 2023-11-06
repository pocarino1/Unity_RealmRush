using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

static class TileSize
{
    public const int Width = 10;
    public const int Height = 10;
}

public class Tile : MonoBehaviour
{
    [SerializeField] private Tower TowerPrefab = null;
    [SerializeField] private bool IsPlaceable = false;

    public bool Placeable { get { return IsPlaceable; } set { IsPlaceable = value; } }
    private GameObject TowerObject = null;
    private GridManager GameGridManager = null;
    private PathFinder GamePathFinder = null;
    private Vector2Int Coordinates = new Vector2Int();

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        GameGridManager = FindObjectOfType<GridManager>();
        GamePathFinder = FindObjectOfType<PathFinder>();

        if(GameGridManager != null)
        {
            Coordinates = GameGridManager.GetCoordinatesFromPosition(transform.position);

            if(!IsPlaceable)
            {
                GameGridManager.BlockNode(Coordinates);
            }
        }   
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        
    }

    /// <summary>
    /// OnMouseUp is called when the user has released the mouse button.
    /// </summary>
    private void OnMouseUp()
    {
        bool IsGameObjectPointer = EventSystem.current.IsPointerOverGameObject();
        if (!IsGameObjectPointer)
        {
            if (IsPlaceable)
            {
                if(GameGridManager != null && GamePathFinder != null)
                {
                    Node CurrentNode = GameGridManager.GetNode(Coordinates);
                    if(CurrentNode == null || !CurrentNode.isWalkable || GamePathFinder.WillBlockPath(Coordinates))
                    {
                        return;
                    }
                }

                if (TowerPrefab != null)
                {
                    TowerObject = TowerPrefab.CreateTower(TowerPrefab, transform.position);
                    if (TowerObject != null)
                    {
                        IsPlaceable = false;

                        if(GameGridManager != null)
                        {
                            GameGridManager.BlockNode(Coordinates);

                            if(GamePathFinder != null)
                            {
                                GamePathFinder.NotifyReceivers();
                            }
                        }
                    }
                }
            }
            else
            {
                if (TowerObject != null)
                {
                    Tower TowerComponent = TowerObject.GetComponent<Tower>();
                    if (TowerComponent != null && TowerComponent.IsEnableLevelUp())
                    {
                        TowerComponent.SetVisibleUpgradeUI(true);
                    }
                }
            }
        }
    }
}
