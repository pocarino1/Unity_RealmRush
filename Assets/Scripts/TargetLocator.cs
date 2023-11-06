using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] private Transform WeaponTransform = null;
    [SerializeField] private float AttackRange = 15.0f;
    [SerializeField] private ParticleSystem Arrow = null;

    private Enemy TargetEnemy = null;

    // Update is called once per frame
    void Update()
    {
        bool ChangeTargetEnemy = ChecekChangeTargetEnemy();
        if(ChangeTargetEnemy)
        {
            Attack(false);
            FindEnemyTarget();
        }

        AimWeapon();
    }

    private bool ChecekChangeTargetEnemy()
    {
        if(TargetEnemy == null || !TargetEnemy.gameObject.activeSelf)
        {
            return true;
        }

        float TargetDistance = Vector3.Distance(transform.position, TargetEnemy.transform.position);
        if(AttackRange < TargetDistance)
        {
            return true;
        }

        return false;
    }

    private void FindEnemyTarget()
    {
        float MaxDistance = Mathf.Infinity;
        Enemy[] EnemyObjects = FindObjectsOfType<Enemy>();

        foreach(Enemy EnemyObject in EnemyObjects)
        {
            if(EnemyObject.gameObject.activeSelf)
            {
                float TargetDistance = Vector3.Distance(transform.position, EnemyObject.transform.position);
                if(MaxDistance > TargetDistance)
                {
                    TargetEnemy = EnemyObject;
                    MaxDistance = TargetDistance;
                }
            }
        }
    }

    private void AimWeapon()
    {
        if(WeaponTransform != null && TargetEnemy != null)
        {
            WeaponTransform.LookAt(TargetEnemy.transform);

            float TargetDistance = Vector3.Distance(transform.position, TargetEnemy.transform.position);
            if(AttackRange > TargetDistance)
            {
                Attack(true);
            }
        }
    }

    private void Attack(bool Active)
    {
        if(Arrow != null)
        {
            ParticleSystem.EmissionModule ArrowEmissionModule = Arrow.emission;
            if(ArrowEmissionModule.enabled != Active)
            {
                ArrowEmissionModule.enabled = Active;
            }
        }
    }
}
