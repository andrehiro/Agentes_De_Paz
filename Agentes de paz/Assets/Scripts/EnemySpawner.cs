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

            foreach (EnemyWave enemyWave in currentWave.enemies)
            {
                for (int i = 0; i < enemyWave.enemyCount; i++)
                {
                    SpawnEnemy(enemyWave.enemyPrefab);
                    yield return new WaitForSeconds(enemyWave.spawnDelay);
                }
            }

            yield return new WaitForSeconds(currentWave.timeBeforeNextWave);
            currentWaveIndex++;
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
