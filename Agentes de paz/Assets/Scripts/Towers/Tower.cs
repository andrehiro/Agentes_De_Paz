using UnityEngine;

public class Tower : MonoBehaviour
{
    public int cost = 100;
    public float range = 10f;
    public float fireRate = 1f;
    public float projectileSpeed = 10f;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public GameObject rangeIndicator;
    private GameObject targetEnemy;
    private float fireCooldown = 0f;
    


    void Start()
    {
        TowerPlacer.instance.SetTowerRangeIndicator(gameObject);
        GameObject targetEnemy = FindMostAdvancedEnemy();
    }

    void Update()
    {
        fireCooldown = Mathf.Max(0, fireCooldown - Time.deltaTime);
        
        if(GetComponent<TowerUpgrades>().targetingButtonText.text == "Primero")
        {
            targetEnemy = FindMostAdvancedEnemy();
        }
        else
        {
            targetEnemy = FindLeastAdvancedEnemy();
        }
    
        if (targetEnemy != null && fireCooldown <= 0f)
        {
            ShootAtEnemy(targetEnemy);
            fireCooldown = 1f / fireRate;
        }
    }

    GameObject FindMostAdvancedEnemy()
    {
        GameObject bestTarget = null;
        int highestWaypointIndex = -1;
        float smallestDistanceToWaypoint = float.MaxValue;

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            // Verificar distancia a la torre
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy > range) continue;

            EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
            if (enemyMovement == null || enemyMovement.waypoints == null) continue;

            // Obtener información del progreso del enemigo
            int currentIndex = enemyMovement.currentWaypointIndex;
            Transform currentWaypoint = GetCurrentWaypoint(enemyMovement);

            if (currentWaypoint == null) continue;

            // Calcular distancia al waypoint actual
            float distanceToWaypoint = Vector3.Distance(enemy.transform.position, currentWaypoint.position);

            // Priorizar enemigos más adelantados en el camino
            bool isBetterTarget = false;
            
            if (currentIndex > highestWaypointIndex)
            {
                isBetterTarget = true;
            }
            else if (currentIndex == highestWaypointIndex)
            {
                if (distanceToWaypoint < smallestDistanceToWaypoint)
                {
                    isBetterTarget = true;
                }
            }

            // Actualizar mejor objetivo
            if (isBetterTarget)
            {
                highestWaypointIndex = currentIndex;
                smallestDistanceToWaypoint = distanceToWaypoint;
                bestTarget = enemy;
            }
        }

        return bestTarget;
    }

    GameObject FindLeastAdvancedEnemy()
    {
        GameObject bestTarget = null;
        int lowestWaypointIndex = int.MaxValue;
        float largestDistanceToWaypoint = -1f;

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            // Verificar distancia a la torre
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy > range) continue;

            EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
            if (enemyMovement == null || enemyMovement.waypoints == null) continue;

            // Obtener información del progreso del enemigo
            int currentIndex = enemyMovement.currentWaypointIndex;
            Transform currentWaypoint = GetCurrentWaypoint(enemyMovement);

            if (currentWaypoint == null) continue;

            // Calcular distancia al waypoint actual
            float distanceToWaypoint = Vector3.Distance(enemy.transform.position, currentWaypoint.position);

            // Priorizar enemigos menos avanzados en el camino
            bool isBetterTarget = false;
            
            if (currentIndex < lowestWaypointIndex)
            {
                isBetterTarget = true;
            }
            else if (currentIndex == lowestWaypointIndex)
            {
                if (distanceToWaypoint > largestDistanceToWaypoint)
                {
                    isBetterTarget = true;
                }
            }

            // Actualizar mejor objetivo
            if (isBetterTarget)
            {
                lowestWaypointIndex = currentIndex;
                largestDistanceToWaypoint = distanceToWaypoint;
                bestTarget = enemy;
            }
        }

        return bestTarget;
    }

    Transform GetCurrentWaypoint(EnemyMovement enemyMovement)
    {
        if (enemyMovement.waypoints.Count == 0) return null;
        int index = Mathf.Clamp(enemyMovement.currentWaypointIndex, 0, enemyMovement.waypoints.Count - 1);
        return enemyMovement.waypoints[index];
    }

    void ShootAtEnemy(GameObject enemy)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Vector3 direction = (enemy.transform.position - firePoint.position).normalized;

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, range);
    }
}