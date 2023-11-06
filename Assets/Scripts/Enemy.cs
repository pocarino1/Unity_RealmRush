using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int GoldReward = 25;
    [SerializeField] private int GoldPenalty = 25;

    private Bank UserBank = null;

    // Start is called before the first frame update
    void Start()
    {
        UserBank = FindObjectOfType<Bank>();
    }

    public void RewardGold(int Level)
    {
        if (UserBank != null)
        {
            UserBank.ChangeGold(GoldReward + Level);
        }
    }

    public void StealGold()
    {
        if (UserBank != null)
        {
            UserBank.ChangeGold(-GoldPenalty);
        }
    }
}
