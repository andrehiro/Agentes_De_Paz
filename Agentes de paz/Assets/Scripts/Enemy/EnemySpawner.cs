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

[System.Serializable]
public class PathBifurcation
{
    public int bifurcationWaypointIndex;
    public List<Transform> pathA;
    public List<Transform> pathB;
}

public class EnemySpawner : MonoBehaviour
{
    public Wave[] waves;
    public Transform spawnPoint;
    public List<Transform> defaultWaypoints;
    public List<PathBifurcation> pathBifurcations;

    public List<int> stealthWaypoints;
    public List<int> stealthExitWaypoints;

    public static event System.Action<int> OnWaveCompleted;
    public static event System.Action OnAllWavesCompleted;

    [SerializeField] private int currentWaveIndex = 0;
    private int currentGroupIndex = 0;
    private bool isWaveInProgress = false;
    private int enemyCounter = 0;

    void Start()
    {
        UIManager.instance.UpdateWaveText(currentWaveIndex + 1);
    }

    void Update()
    {
        // Solo para oleadas posteriores si el toggle está activado
        if (!isWaveInProgress &&
            currentWaveIndex > 0 &&
            currentWaveIndex < waves.Length &&
            GameStateManager.instance.autoModeToggle.isOn)
        {
            StartNextWave();
        }
    }

    public void StartNextWave()
    {
        if (!isWaveInProgress && currentWaveIndex < waves.Length)
        {
            StartCoroutine(SpawnWave());
        }
    }

    IEnumerator SpawnWave()
    {
        isWaveInProgress = true;
        Wave currentWave = waves[currentWaveIndex];
        enemyCounter = 0;

        // Esperar delay inicial solo para oleadas posteriores
        if (currentWaveIndex > 0 && currentWave.timeBeforeNextWave > 0)
        {
            yield return new WaitForSeconds(currentWave.timeBeforeNextWave);
        }

        // Spawn de grupos de enemigos
        for (currentGroupIndex = 0; currentGroupIndex < currentWave.enemyGroups.Length; currentGroupIndex++)
        {
            EnemyWave currentGroup = currentWave.enemyGroups[currentGroupIndex];

            for (int i = 0; i < currentGroup.enemyCount; i++)
            {
                SpawnEnemy(currentGroup.enemyPrefab, SelectPath(enemyCounter));
                enemyCounter++;

                if (i < currentGroup.enemyCount - 1 && currentGroup.spawnDelay > 0)
                    yield return new WaitForSeconds(currentGroup.spawnDelay);
            }

            if (currentGroup.delayAfterWave > 0)
                yield return new WaitForSeconds(currentGroup.delayAfterWave);
        }

        // Esperar a que todos los enemigos mueran
        yield return new WaitUntil(() => EnemyManager.instance.enemiesAlive == 0);

        // Notificar finalización de oleada
        OnWaveCompleted?.Invoke(currentWaveIndex + 1);

        // Preparar siguiente oleada
        currentWaveIndex++;
        UIManager.instance.UpdateWaveText(currentWaveIndex + 1);
        isWaveInProgress = false;

        // Finalizar si es la última oleada
        if (currentWaveIndex >= waves.Length)
        {
            OnAllWavesCompleted?.Invoke();
        }
    }

    void SpawnEnemy(GameObject enemyPrefab, List<Transform> path)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();

        if (enemyMovement != null)
        {
            enemyMovement.waypoints = path;
            enemyMovement.spawnPoint = spawnPoint; // Asigna el spawnPoint desde el EnemySpawner
            enemyMovement.SetStealthWaypoints(stealthWaypoints, stealthExitWaypoints);
        }

        EnemyManager.instance.RegisterEnemy();
    }

    List<Transform> SelectPath(int enemyIndex)
    {
        foreach (PathBifurcation bifurcation in pathBifurcations)
        {
            if (bifurcation.bifurcationWaypointIndex < defaultWaypoints.Count)
            {
                List<Transform> combinedPath = new List<Transform>();

                // Base path hasta bifurcación
                for (int i = 0; i <= bifurcation.bifurcationWaypointIndex; i++)
                {
                    if (i < defaultWaypoints.Count)
                        combinedPath.Add(defaultWaypoints[i]);
                }

                // Rama seleccionada
                List<Transform> selectedBranch = (enemyIndex % 2 == 0) ? bifurcation.pathA : bifurcation.pathB;
                combinedPath.AddRange(selectedBranch);

                return combinedPath;
            }
        }
        return new List<Transform>(defaultWaypoints);
    }
}