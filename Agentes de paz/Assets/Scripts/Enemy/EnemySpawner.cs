using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class EnemyWave
{
    public GameObject enemyPrefab;
    public int enemyCount;
    public float spawnDelay;
    public float delayAfterWave; // Nuevo campo para delay después de este grupo
}

[System.Serializable]
public class Wave
{
    public EnemyWave[] enemyGroups; // Renombrado para mayor claridad
    public float timeBeforeNextWave;
}

public class EnemySpawner : MonoBehaviour
{
    public Wave[] waves;
    public Transform spawnPoint;
    public List<Transform> waypoints;
    public Toggle autoModeToggle;
    public static event Action<int> OnWaveCompleted; 
    public static event Action OnAllWavesCompleted;

    public int currentWaveIndex = 0;
    private int currentGroupIndex = 0;
    private bool isWaveInProgress = false;

    void Start()
    {
        UIManager.instance.UpdateWaveText(currentWaveIndex + 1);
    }

    void Update()
    {
        if (!isWaveInProgress && currentWaveIndex < waves.Length && autoModeToggle.isOn && currentWaveIndex != 0)
        {
            StartNextWave();
        }
    }

    public void StartNextWave()
    {
        if (!isWaveInProgress && currentWaveIndex < waves.Length)
        {
            isWaveInProgress = true;
            StartCoroutine(SpawnWaves());
        }
    }

    IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            Wave currentWave = waves[currentWaveIndex];
            currentGroupIndex = 0;

            while (currentGroupIndex < currentWave.enemyGroups.Length)
            {
                EnemyWave currentGroup = currentWave.enemyGroups[currentGroupIndex];

                for (int i = 0; i < currentGroup.enemyCount; i++)
                {
                    SpawnEnemy(currentGroup.enemyPrefab);
                    EnemyManager.instance.RegisterEnemy();

                    if (i < currentGroup.enemyCount - 1 && currentGroup.spawnDelay > 0)
                    {
                        yield return new WaitForSeconds(currentGroup.spawnDelay);
                    }
                }

                // Esperar delay después del grupo si está definido
                if (currentGroup.delayAfterWave > 0)
                {
                    yield return new WaitForSeconds(currentGroup.delayAfterWave);
                }

                currentGroupIndex++;
            }

            yield return new WaitUntil(() => EnemyManager.instance.enemiesAlive == 0);

            // Obtener el número de onda completada (base 1 para UI)
            int completedWaveNumber = currentWaveIndex + 1;

            // Activar evento de onda completada
            OnWaveCompleted?.Invoke(completedWaveNumber);

            // Guardar el delay de la onda ACTUAL antes de incrementar
            float currentWaveDelay = currentWave.timeBeforeNextWave;

            currentWaveIndex++;
            UIManager.instance.UpdateWaveText(currentWaveIndex + 1);
            isWaveInProgress = false;

            if (currentWaveIndex < waves.Length)
            {
                // Usar el delay de la onda que ACABA de terminar
                if (currentWaveDelay > 0)
                {
                    yield return new WaitForSeconds(currentWaveDelay);
                }
                yield break;
            }
        }

        while (EnemyManager.instance.enemiesAlive > 0)
        {
            yield return null;
        }
        OnAllWavesCompleted?.Invoke();
        yield break;
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.waypoints = waypoints;
        }
    }
}