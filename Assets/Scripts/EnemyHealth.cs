using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int MaxHealth = 100;
    [SerializeField] private int DifficultyIncreaseHealth = 20;
    [SerializeField] private GameObject HealthUI = null;
    [SerializeField] private Image HealthBar = null;
    [SerializeField] private Text HealthText = null;
    [SerializeField] private float HealthBarOffsetX = 0.0f;
    [SerializeField] private float HealthBarOffsetY = 0.0f;

    private int CurrentHealth = 0;
    private Enemy EnemyClass = null;
    private int EnemyLevel = 1;
    private StringBuilder HealthTextBuilder = new StringBuilder();

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        EnemyClass = GetComponent<Enemy>();        
    }

    void OnEnable()
    {
        CurrentHealth = MaxHealth;
        SetVisibleHealthBar(false);
    }

    /// <summary>
    /// OnParticleCollision is called when a particle hits a collider.
    /// </summary>
    /// <param name="other">The GameObject hit by the particle.</param>
    private void OnParticleCollision(GameObject other)
    {
        Tower TowerComponent = other.GetComponentInParent<Tower>();
        int Damage = TowerComponent != null ? TowerComponent.GetAttackDamage() : Random.Range(20, 35);
        CurrentHealth -= Damage;

        SetVisibleHealthBar(true);

        if (CurrentHealth <= 0)
        {
            if (EnemyClass != null)
            {
                EnemyClass.RewardGold(EnemyLevel);
            }

            SetVisibleHealthBar(false);

            EnemyLevel++;
            MaxHealth += DifficultyIncreaseHealth;
            gameObject.SetActive(false);
        }
    }

    private void SetVisibleHealthBar(bool Visible)
    {
        if (HealthUI == null)
        {
            return;
        }

        if (HealthUI.activeSelf != Visible)
        {
            HealthUI.SetActive(Visible);

            if (Visible)
            {
                UpdateHealthBarUI();
            }
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        UpdateHealthBarUI();
    }

    private void UpdateHealthBarUI()
    {
        if (HealthUI == null || !HealthUI.activeSelf)
        {
            return;
        }

        Vector3 ScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        RectTransform UIRect = HealthUI.GetComponent<RectTransform>();
        if (UIRect != null)
        {
            ScreenPosition.x += HealthBarOffsetX;
            ScreenPosition.y += HealthBarOffsetY;
            UIRect.position = ScreenPosition;
        }

        if (HealthBar != null)
        {
            RectTransform ImageRect = HealthBar.GetComponent<RectTransform>();
            if (ImageRect != null)
            {
                float XScale = (float)CurrentHealth / (float)MaxHealth;
                ImageRect.localScale = new Vector3(XScale, 1.0f, 1.0f);
            }
        }

        if (HealthText != null)
        {
            HealthTextBuilder.Clear();
            HealthTextBuilder.Append(CurrentHealth);
            HealthTextBuilder.Append("/");
            HealthTextBuilder.Append(MaxHealth);

            HealthText.text = HealthTextBuilder.ToString();
        }
    }
}
