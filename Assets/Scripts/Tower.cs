using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    [SerializeField] private float BuildDelay = 1.0f;
    [SerializeField] private int MaxTowerLevel = 5;
    [SerializeField] private int CreateCost = 75;
    [SerializeField] private int MinAttackDamage = 20;
    [SerializeField] private int MaxAttackDamage = 35;
    [SerializeField] private int LevelDamageBonus = 10;
    [SerializeField] private GameObject MaskBGUI = null;
    [SerializeField] private GameObject UpgradeUI = null;
    [SerializeField] private GameObject StarsUI = null;
    [SerializeField] private Text LevelUpText = null;
    [SerializeField] private Text UpgradeGoldText = null;
    [SerializeField] private Button UpgradeButton = null;
    [SerializeField] private float UpgradeUIOffsetY = 60.0f;
    [SerializeField] private List<Image> StarImageList = new List<Image>();
    [SerializeField] private float StarsUIOffsetY = 0.0f;
    [SerializeField] private GameObject TowerBottom = null;
    [SerializeField] private GameObject TowerHead = null;
    [SerializeField] private GameObject Arrow = null;

    private Bank UserBank = null;

    private int TowerLevel = 1;

    public bool IsEnableLevelUp()
    {
        return TowerLevel < MaxTowerLevel ? true : false;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        SetVisibleUpgradeUI(false);
        InitializeStarsUI();

        if (UpgradeButton != null)
        {
            UpgradeButton.onClick.AddListener(OnClickUpgrade);
        }

        StartCoroutine(Build());
    }

    IEnumerator Build()
    {
        if(TowerBottom != null)
        {
            TowerBottom.SetActive(false);
        }

        if(TowerHead != null)
        {
            TowerHead.SetActive(false);
        }

        if(Arrow != null)
        {
            Arrow.SetActive(false);
        }

        if(TowerBottom != null)
        {
            TowerBottom.SetActive(true);
        }

        yield return new WaitForSeconds(BuildDelay);

        if(TowerHead != null)
        {
            TowerHead.SetActive(true);
        }

        yield return new WaitForSeconds(BuildDelay);

        if(Arrow != null)
        {
            Arrow.SetActive(true);
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        CheckUpgradeUI();
    }

    private void CheckUpgradeUI()
    {
        if (UserBank == null)
        {
            return;
        }

        if (UpgradeUI == null || !UpgradeUI.activeSelf)
        {
            return;
        }

        int NeedGold = TowerLevel * 50;
        bool EnableUpgrade = UserBank.Gold >= NeedGold ? true : false;
        if (UpgradeButton.interactable != EnableUpgrade)
        {
            UpgradeButton.interactable = EnableUpgrade;
        }
    }

    public GameObject CreateTower(Tower tower, Vector3 CreatePosition)
    {
        if (tower == null)
        {
            return null;
        }

        Bank bank = FindObjectOfType<Bank>();
        if (bank == null)
        {
            return null;
        }

        if (bank.Gold < CreateCost)
        {
            return null;
        }

        bank.ChangeGold(-CreateCost);
        return Instantiate(tower.gameObject, CreatePosition, Quaternion.identity);
    }

    public int GetAttackDamage()
    {
        int Damage = Random.Range(MinAttackDamage, MaxAttackDamage);
        Damage += TowerLevel * LevelDamageBonus;
        return Damage;
    }

    public void SetVisibleUpgradeUI(bool Visible)
    {
        if (UserBank == null)
        {
            UserBank = FindObjectOfType<Bank>();
            if (UserBank == null)
            {
                return;
            }
        }

        if (MaskBGUI != null)
        {
            MaskBGUI.SetActive(Visible);
        }

        if (UpgradeUI != null)
        {
            if (Visible)
            {
                Vector3 ScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
                RectTransform UIRect = UpgradeUI.GetComponent<RectTransform>();
                if (UIRect != null)
                {
                    ScreenPosition.y += UpgradeUIOffsetY;
                    UIRect.position = ScreenPosition;
                }

                if (LevelUpText != null)
                {
                    LevelUpText.text = "Lv." + (TowerLevel + 1);
                }

                int NeedGold = TowerLevel * 50;
                if (UpgradeGoldText != null)
                {
                    UpgradeGoldText.text = NeedGold.ToString();
                }

                bool EnableLevelup = UserBank != null ? UserBank.Gold >= NeedGold ? true : false : false;
                UpgradeButton.interactable = EnableLevelup;
            }

            UpgradeUI.SetActive(Visible);
        }
    }

    private void OnClickUpgrade()
    {
        if (UserBank != null)
        {
            int NeedGold = TowerLevel * 50;
            if (UserBank.Gold >= NeedGold)
            {
                UserBank.ChangeGold(-NeedGold);
                TowerLevel++;

                UpdateStarsUI();
            }
        }

        SetVisibleUpgradeUI(false);
    }

    public void OnMaskClick()
    {
        SetVisibleUpgradeUI(false);
    }

    private void InitializeStarsUI()
    {
        if (StarsUI != null)
        {
            Vector3 ScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
            RectTransform UIRect = StarsUI.GetComponent<RectTransform>();

            ScreenPosition.y += StarsUIOffsetY;
            UIRect.position = ScreenPosition;
        }

        UpdateStarsUI();
    }

    private void UpdateStarsUI()
    {
        for (int i = 0; i < StarImageList.Count; ++i)
        {
            if (StarImageList[i] != null)
            {
                StarImageList[i].gameObject.SetActive(i < TowerLevel ? true : false);
            }
        }
    }
}
