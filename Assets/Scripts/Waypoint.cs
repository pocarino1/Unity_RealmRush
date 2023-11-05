using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private Tower TowerPrefab = null;
    [SerializeField] private bool IsPlaceable = false;

    public bool Placeable { get { return IsPlaceable; } set { IsPlaceable = value; } }
    private GameObject TowerObject = null;

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
                if (TowerPrefab != null)
                {
                    TowerObject = TowerPrefab.CreateTower(TowerPrefab, transform.position);
                    if (TowerObject != null)
                    {
                        IsPlaceable = false;
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
