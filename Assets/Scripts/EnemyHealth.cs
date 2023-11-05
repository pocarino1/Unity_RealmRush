using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int MaxHealth = 100;
    [SerializeField] private int DifficultyIncreaseHealth = 20;
    
    private int CurrentHealth = 0;
    private Enemy EnemyClass = null;
    private int EnemyLevel = 1;

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

        if(CurrentHealth <= 0)
        {
            if(EnemyClass != null)
            {
                EnemyClass.RewardGold(EnemyLevel);
            }

            EnemyLevel++;
            MaxHealth += DifficultyIncreaseHealth;
            gameObject.SetActive(false);
        }
    }
}
