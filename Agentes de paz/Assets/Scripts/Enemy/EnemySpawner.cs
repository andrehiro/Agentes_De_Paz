using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class EnemyWave
{
    public GameObject enemyPrefab;
    public int enemyCount;
    public float spawnDelay;
}

[System.Serializable]
public class Wave
{
    public EnemyWave[] enemies;
    public float timeBeforeNextWave;
}

public class EnemySpawner : MonoBehaviour
{
    public Wave[] waves;
    public Transform spawnPoint;
    public List<Transform> waypoints;
    public Toggle autoModeToggle;
    private int currentWaveIndex = 0;
    private bool isWaveInProgress = false;

    void Start()
    {
        UIManager.instance.UpdateWaveText(currentWaveIndex + 1);
    }

    void Update()
    {
        // Iniciar la siguiente oleada si el modo automático está activado
        if (!isWaveInProgress && currentWaveIndex < waves.Length && autoModeToggle.isOn && currentWaveIndex != 0)
        {
            StartNextWave();
        }
    }

    // Método para iniciar la oleada cuando se presione el botón
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

            foreach (EnemyWave enemyWave in currentWave.enemies)
            {
                for (int i = 0; i < enemyWave.enemyCount; i++)
                {
                    SpawnEnemy(enemyWave.enemyPrefab);
                    EnemyManager.instance.RegisterEnemy();

                    if (enemyWave.spawnDelay > 0)
                    {
                        yield return new WaitForSeconds(enemyWave.spawnDelay);
                    }
                }
            }

            // Esperar hasta que todos los enemigos sean derrotados
            yield return new WaitUntil(() => EnemyManager.instance.enemiesAlive == 0);

            // Avanzar a la siguiente oleada
            currentWaveIndex++;
            UIManager.instance.UpdateWaveText(currentWaveIndex + 1);
            isWaveInProgress = false; // Permitir iniciar la siguiente oleada con el botón

            // Si quedan oleadas, esperar el botón para la siguiente
            if (currentWaveIndex < waves.Length)
            {
                yield break; 
            }
        }

        // Verificar que no haya más enemigos antes de mostrar la victoria
        while (EnemyManager.instance.enemiesAlive > 0)
        {
            yield return null;
        }
        UIManager.instance.ShowWinGameUI();
        TowerPlacer.instance.CancelTowerPlacement();
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