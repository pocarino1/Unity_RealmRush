using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bank : MonoBehaviour
{
    [SerializeField] private int StartingGold = 150;
    [SerializeField] private TextMeshProUGUI GoldUI = null;

    private int CurrentGold = 0;
    public int Gold
    {
        get
        {
            return CurrentGold;
        }
    }

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        CurrentGold = StartingGold;
        UpdateGoldText();
    }

    private void UpdateGoldText()
    {
        if(GoldUI != null)
        {
            GoldUI.text = "Gold : " + CurrentGold;
        }
    }

    public void ChangeGold(int Amount)
    {
        CurrentGold += Amount;
        UpdateGoldText();

        if(CurrentGold < 0)
        {
            // Lose the Game..
            ReloadScene();
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
