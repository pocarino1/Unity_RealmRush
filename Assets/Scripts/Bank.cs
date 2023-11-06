using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bank : MonoBehaviour
{
    [SerializeField] private int StartingGold = 150;
    [SerializeField] private int IncreaseGoldValue = 3;
    [SerializeField] private float IncreaseDelayTime = 2.0f;
    [SerializeField] private TextMeshProUGUI GoldUI = null;
    [SerializeField] private TextMeshProUGUI MissionUI = null;
    [SerializeField] private GameObject SuccessMessageUI = null;
    [SerializeField] private int MissionGold = 500;

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

        UpdateMissionText();
        UpdateGoldText();

        SetSuccessMessageUI(false);

        InvokeRepeating("IncreaseGold", IncreaseDelayTime, IncreaseDelayTime);
    }

    private void SetSuccessMessageUI(bool Visible)
    {
        if(SuccessMessageUI != null && SuccessMessageUI.activeSelf != Visible)
        {
            SuccessMessageUI.SetActive(Visible);
        }
    }

    private void UpdateMissionText()
    {
        if(MissionUI != null)
        {
            MissionUI.text = "Mission : Collect " + MissionGold + " Gold!";
        }
    }

    private void IncreaseGold()
    {
        CurrentGold += IncreaseGoldValue;
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
        else if(CurrentGold >= MissionGold)
        {
            SetSuccessMessageUI(true);

            Invoke("LoadNextScene", 2.0f);
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadNextScene()
    {
        int NextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if(NextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            NextSceneIndex = 0;
        }

        SceneManager.LoadScene(NextSceneIndex);
    }
}
