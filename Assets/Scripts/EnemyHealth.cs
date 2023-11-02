using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int MaxHealth = 100;
    
    private int CurrentHealth = 0;

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
        CurrentHealth -= 20;

        if(CurrentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
