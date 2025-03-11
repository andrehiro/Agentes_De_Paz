using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EnemyWave
{
    public GameObject enemyPrefab;
    public int enemyCount;
    public float spawnDelay;
    public float delayAfterWave;
}

[System.Serializable]
public class Wave
{
    public EnemyWave[] enemyGroups;
    public float timeBeforeNextWave;
}

// 🟢 Ahora el EnemySpawner define los stealth waypoints
public class EnemySpawner : MonoBehaviour
{
    public Wave[] waves;
    public Transform spawnPoint;
    public List<Transform> waypoints;

    // Waypoints de sigilo (se configuran en Unity para cada nivel)
    public List<int> stealthWaypoints;
    public List<int> stealthExitWaypoints;

    public static event System.Action<int> OnWaveCompleted;
    public static event System.Action OnAllWavesCompleted;

    public int currentWaveIndex = 0;
    private int currentGroupIndex = 0;
    private bool isWaveInProgress = false;

    void Start()
    {
        UIManager.instance.UpdateWaveText(currentWaveIndex + 1);
    }

    void Update()
    {
        if (!isWaveInProgress && currentWaveIndex < waves.Length && GameStateManager.instance.autoModeToggle.isOn && currentWaveIndex != 0)
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

                if (currentGroup.delayAfterWave > 0)
                {
                    yield return new WaitForSeconds(currentGroup.delayAfterWave);
                }

                currentGroupIndex++;
            }

            yield return new WaitUntil(() => EnemyManager.instance.enemiesAlive == 0);

            int completedWaveNumber = currentWaveIndex + 1;
            OnWaveCompleted?.Invoke(completedWaveNumber);

            float currentWaveDelay = currentWave.timeBeforeNextWave;

            currentWaveIndex++;
            UIManager.instance.UpdateWaveText(currentWaveIndex + 1);
            isWaveInProgress = false;

            if (currentWaveIndex < waves.Length)
            {
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
            enemyMovement.SetStealthWaypoints(stealthWaypoints, stealthExitWaypoints); // 🟢 Pasar los stealth waypoints al enemigo
        }
    }
}
