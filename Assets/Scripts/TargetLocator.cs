using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] private Transform WeaponTransform = null;
    [SerializeField] private ParticleSystem Arrow = null;

    private Enemy TargetEnemy = null;

    // Update is called once per frame
    void Update()
    {
        if(TargetEnemy == null || !TargetEnemy.gameObject.activeSelf)
        {
            FindEnemyTarget();
        }

        AimWeapon();
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
        if(TargetEnemy == null || !TargetEnemy.gameObject.activeSelf)
        {
            StopShotArrow();
            return;
        }

        if(WeaponTransform != null)
        {
            WeaponTransform.LookAt(TargetEnemy.transform);
            StartShotArrow();
        }
    }

    private void StartShotArrow()
    {
        if(Arrow != null && !Arrow.isPlaying)
        {
            Arrow.Play();
        }
    }

    private void StopShotArrow()
    {
        if(Arrow != null && Arrow.isPlaying)
        {
            Arrow.Stop();
        }
    }
}
