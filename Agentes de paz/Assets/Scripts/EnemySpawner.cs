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
}

public class EnemySpawner : MonoBehaviour
{
    public Wave[] waves;   // Las diferentes oleadas de enemigos
    public Transform spawnPoint; // Punto de spawn
    public List<Transform> waypoints; // Waypoints a los que los enemigos se moverán
    public int timeBeforeNextWave = 5; // Tiempo entre oleadas
    private int currentWaveIndex = 0; // Índice de la oleada actual
    public int totalEnemiesAlive = 0; // Contador de enemigos vivos
    private int totalEnemiesInCurrentWave = 0; // Contador de enemigos en la oleada actual

    // Iniciar el spawn de enemigos al principio
    void Start()
    {
        StartNextWave(); // Comienza la primera oleada
    }

    // Método que se llama para iniciar la siguiente oleada
    public void StartNextWave()
    {
        if (currentWaveIndex < waves.Length)
        {
            totalEnemiesAlive = 0; // Reiniciar el contador de enemigos vivos para la nueva oleada
            Wave currentWave = waves[currentWaveIndex]; // Obtener la oleada actual

            foreach (EnemyWave enemyWave in currentWave.enemies)
            {
                totalEnemiesInCurrentWave += enemyWave.enemyCount; // Sumar el total de enemigos de la oleada
            }

            StartCoroutine(SpawnWaveEnemies(currentWave)); // Empezar a spawnear los enemigos de la oleada

            currentWaveIndex++; // Aumentar el índice de oleada
        }
        else
        {
            Debug.Log("¡Has completado todas las rondas!");
            UIManager.instance.ShowWinGameUI();  // Mostrar la UI de victoria
        }
    }

    // Coroutine para spawnear enemigos en la oleada
    IEnumerator SpawnWaveEnemies(Wave currentWave)
    {
        foreach (EnemyWave enemyWave in currentWave.enemies)
        {
            for (int i = 0; i < enemyWave.enemyCount; i++)
            {
                SpawnEnemy(enemyWave.enemyPrefab); // Spawnear el enemigo
                yield return new WaitForSeconds(enemyWave.spawnDelay); // Esperar el tiempo entre enemigos
            }
        }

        // Esperar hasta que todos los enemigos de la oleada sean eliminados
        yield return new WaitUntil(() => totalEnemiesAlive == 0);

        // Luego de que todos los enemigos mueren, iniciar la siguiente oleada
        yield return new WaitForSeconds(timeBeforeNextWave);
        StartNextWave(); // Iniciar la siguiente oleada
    }

    // Método para spawnear un enemigo
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

        // Registrar al enemigo en el EnemyManager
        EnemyManager.instance.RegisterEnemy();
    }

    // Método que se llama cuando un enemigo muere
    public void OnEnemyDied()
    {
        totalEnemiesAlive--; // Disminuir los enemigos vivos

        if (totalEnemiesAlive <= 0 && currentWaveIndex >= waves.Length)
        {
            Debug.Log("¡Ganaste!");
            UIManager.instance.ShowWinGameUI();  // Mostrar la UI de victoria
        }
    }
}
