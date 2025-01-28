using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    private int currentWaveIndex = 0;

    // Iniciar el spawn de enemigos
    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    // Spawnear las oleadas de enemigos
    IEnumerator SpawnWaves()
{
    while (currentWaveIndex < waves.Length)
    {
        Wave currentWave = waves[currentWaveIndex];
        Debug.Log("Starting wave " + currentWaveIndex);

        // Spawnear todos los enemigos de la oleada
        foreach (EnemyWave enemyWave in currentWave.enemies)
        {
            for (int i = 0; i < enemyWave.enemyCount; i++)
            {
                SpawnEnemy(enemyWave.enemyPrefab);
                EnemyManager.instance.RegisterEnemy();
                Debug.Log("Enemy spawned: " + enemyWave.enemyPrefab.name);

                // Esperar el tiempo de spawnDelay antes de generar el siguiente enemigo
                if (enemyWave.spawnDelay > 0)
                    yield return new WaitForSeconds(enemyWave.spawnDelay);
            }
        }

        // Esperar a que todos los enemigos sean eliminados antes de avanzar
        while (EnemyManager.instance.enemiesAlive > 0)
        {
            Debug.Log("Waiting for enemies to be defeated: " + EnemyManager.instance.enemiesAlive + " remaining");
            yield return null;
        }

        // Avanzar a la siguiente oleada
        currentWaveIndex++;
        yield return new WaitForSeconds(currentWave.timeBeforeNextWave);
    }

    // Mostrar la UI de victoria si no quedan enemigos
    if (EnemyManager.instance.enemiesAlive <= 0)
    {
        UIManager.instance.ShowWinGameUI();
        Debug.Log("All enemies defeated. Victory!");
    }
}

    // Spawnear un enemigo y asignarle los waypoints
    void SpawnEnemy(GameObject enemyPrefab)
    {
        // Instanciar el enemigo
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        // Asignar los waypoints al enemigo
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.waypoints = waypoints;
        }
    }
}