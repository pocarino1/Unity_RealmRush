using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private int CreateCost = 75;

    public bool CreateTower(Tower Tower, Vector3 CreatePosition)
    {
        if (Tower == null)
        {
            return false;
        }

        Bank UserBank = FindObjectOfType<Bank>();
        if (UserBank == null)
        {
            return false;
        }

        if (UserBank.Gold < CreateCost)
        {
            return false;
        }

        UserBank.ChangeGold(-CreateCost);
        Instantiate(Tower.gameObject, CreatePosition, Quaternion.identity);
        return true;
    }
}
