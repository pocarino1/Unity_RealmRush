using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private Tower TowerPrefab = null;
    [SerializeField] private bool IsPlaceable = false;

    public bool Placeable { get { return IsPlaceable; } set { IsPlaceable = value; } }

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    private void OnMouseDown()
    {
        if(IsPlaceable)
        {
            if(TowerPrefab != null)
            {
                if(TowerPrefab.CreateTower(TowerPrefab, transform.position))
                {
                    IsPlaceable = false;
                }
            }
        }
    }
}
