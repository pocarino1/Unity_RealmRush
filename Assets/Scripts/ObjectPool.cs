using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject EnemyPrefab = null;
    [SerializeField] [Range(0, 50)] private int EnemyPoolSize = 5;
    [SerializeField] [Range(0.1f, 30.0f)] private float EnemySpawnDelayTime = 1.0f;

    private GameObject[] EnemyPool;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        PopulatePool();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private void PopulatePool()
    {
        if (EnemyPrefab != null && EnemyPoolSize > 0)
        {
            EnemyPool = new GameObject[EnemyPoolSize];

            for (int i = 0; i < EnemyPool.Length; ++i)
            {
                EnemyPool[i] = Instantiate(EnemyPrefab, transform);
                EnemyPool[i].SetActive(false);
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        if (EnemyPrefab != null)
        {
            while (true)
            {
                ActivateEnemyInPool();
                yield return new WaitForSeconds(EnemySpawnDelayTime);
            }
        }
    }

    private void ActivateEnemyInPool()
    {
        for (int i = 0; i < EnemyPool.Length; ++i)
        {
            if (!EnemyPool[i].activeSelf)
            {
                EnemyPool[i].SetActive(true);
                return;
            }
        }
    }
}
