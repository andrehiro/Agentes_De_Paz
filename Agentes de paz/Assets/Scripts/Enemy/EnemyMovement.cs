using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour
{
    public static int enemyCounter = 0;
    public Transform spawnPoint;
    public List<List<Transform>> pathOptions;
    public List<Transform> waypoints;
    public int currentWaypointIndex = 0;
    public float speed = 3f;
    public float damage = 10f;
    public float knockbackResistance = 0f;

    private bool isKnockedBack = false;
    private Vector2 currentMovementDirection;
    private SpriteRenderer spriteRenderer;
    private EnemyHealth enemyHealth;

    private List<int> stealthWaypoints = new List<int>();
    private List<int> stealthExitWaypoints = new List<int>();
    
    private bool isInvulnerable = false;
    private Vector3 originalScale;

    [Header("Stealth Animation Settings")]
    public float stealthDuration = 0.5f;
    public float stealthScaleSpeed = 1.5f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyHealth = GetComponent<EnemyHealth>();
        originalScale = transform.localScale;

        AssignPath();
        enemyCounter++;
    }

    void Update()
    {
        if (!isKnockedBack && waypoints != null && waypoints.Count > 0)
            MoveToWaypoint();
    }

    private IEnumerator AdjustAllAfterResolution(float oldRatioX, float oldRatioY)
    {
        FindAnyObjectByType<MapScaler>()?.AdjustMap();

        foreach (ColliderScaler cs in FindObjectsByType<ColliderScaler>())
            cs.AdjustToResolution();

        yield return null;
        yield return null;

        EnemyMovement[] enemies = FindObjectsByType<EnemyMovement>();
        Debug.Log($"Enemigos encontrados: {enemies.Length}");
        
        foreach (EnemyMovement enemy in enemies)
            enemy.SnapToCurrentProgress();
    }

    public void SnapToCurrentProgress()
    {
        if (waypoints == null || waypoints.Count == 0) return;

        // Si aun no llego al primer waypoint, queda entre spawn y waypoint 0
        if (currentWaypointIndex == 0)
        {
            Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : waypoints[0].position;
            Vector3 firstWaypoint = waypoints[0].position;

            float totalDistance = Vector3.Distance(spawnPos, firstWaypoint);
            float distanceToNext = Vector3.Distance(transform.position, firstWaypoint);

            float progress = totalDistance > 0 ? 1f - (distanceToNext / totalDistance) : 0f;
            progress = Mathf.Clamp01(progress);

            transform.position = Vector3.Lerp(spawnPos, firstWaypoint, progress);
            return;
        }

        int prevIndex = currentWaypointIndex - 1;
        int nextIndex = Mathf.Min(currentWaypointIndex, waypoints.Count - 1);

        Vector3 prevWaypoint = waypoints[prevIndex].position;
        Vector3 nextWaypoint = waypoints[nextIndex].position;

        float total = Vector3.Distance(prevWaypoint, nextWaypoint);
        float dist = Vector3.Distance(transform.position, nextWaypoint);

        float prog = total > 0 ? 1f - (dist / total) : 0f;
        prog = Mathf.Clamp01(prog);

        transform.position = Vector3.Lerp(prevWaypoint, nextWaypoint, prog);
    }

    void MoveToWaypoint()
    {
        if (currentWaypointIndex < waypoints.Count)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            currentMovementDirection = (targetWaypoint.position - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
            {
                currentWaypointIndex++;

                if (stealthWaypoints.Contains(currentWaypointIndex))
                    StartCoroutine(SetStealthMode(true));
                else if (stealthExitWaypoints.Contains(currentWaypointIndex))
                    StartCoroutine(SetStealthMode(false));

                if (currentWaypointIndex >= waypoints.Count)
                {
                    PlayerHealth.instance.TakeDamage(damage);
                    enemyHealth.DestroyEnemy();
                }
            }
        }
    }

    void AssignPath()
    {
        if (pathOptions != null && pathOptions.Count > 0)
        {
            int pathIndex = enemyCounter % pathOptions.Count;
            waypoints = new List<Transform>(pathOptions[pathIndex]);
        }
    }

    public void SetPaths(List<List<Transform>> paths)
    {
        pathOptions = paths;
    }

    public void SetStealthWaypoints(List<int> stealthPoints, List<int> exitPoints)
    {
        stealthWaypoints = stealthPoints ?? new List<int>();
        stealthExitWaypoints = exitPoints ?? new List<int>();
    }

    IEnumerator SetStealthMode(bool active)
    {
        isInvulnerable = active;
        float elapsedTime = 0f;
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = active ? Vector3.zero : originalScale;
        float adjustedDuration = stealthDuration / stealthScaleSpeed;

        while (elapsedTime < adjustedDuration)
        {
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / adjustedDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = targetScale;
    }

    public bool IsInvulnerable()
    {
        return isInvulnerable;
    }

    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        if (!isKnockedBack)
        {
            float adjustedForce = force * (1f - knockbackResistance);
            StartCoroutine(KnockbackRoutine(direction, adjustedForce, duration));
        }
    }

    private IEnumerator KnockbackRoutine(Vector2 direction, float force, float duration)
    {
        isKnockedBack = true;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            if (currentWaypointIndex > 0)
            {
                Transform previousWaypoint = waypoints[currentWaypointIndex - 1];
                Vector2 moveDirection = (previousWaypoint.position - transform.position).normalized;
                transform.position += (Vector3)(moveDirection * force * Time.deltaTime);

                if (Vector3.Distance(transform.position, previousWaypoint.position) < 0.1f)
                    currentWaypointIndex--;
            }
            else
            {
                if (spawnPoint != null)
                {
                    Vector2 moveDirection = (spawnPoint.position - transform.position).normalized;
                    transform.position += (Vector3)(moveDirection * force * Time.deltaTime);
                }
                else
                {
                    transform.position -= (Vector3)(currentMovementDirection * force * Time.deltaTime);
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        isKnockedBack = false;
    }

    public Vector2 GetCurrentDirection()
    {
        return currentMovementDirection;
    }
}