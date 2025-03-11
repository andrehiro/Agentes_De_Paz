using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour
{
    public List<Transform> waypoints;
    public int currentWaypointIndex = 0;
    public float speed = 3f;
    public float damage = 10f;

    private bool isKnockedBack = false;
    private Vector2 currentMovementDirection;
    private SpriteRenderer spriteRenderer;
    private EnemyHealth enemyHealth;

    private List<int> stealthWaypoints = new List<int>();
    private List<int> stealthExitWaypoints = new List<int>();

    private bool isInvulnerable = false;
    private Vector3 originalScale;
    
    [Header("Stealth Animation Settings")]
    public float stealthDuration = 0.5f;  // Duración de la animación completa
    public float stealthScaleSpeed = 1.5f; // Velocidad de escalado (1 = normal, >1 = más rápido)

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyHealth = GetComponent<EnemyHealth>();
        originalScale = transform.localScale;
    }

    void Update()
    {
        if (!isKnockedBack && waypoints != null && waypoints.Count > 0)
        {
            MoveToWaypoint();
        }
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
                {
                    StartCoroutine(SetStealthMode(true));
                }
                else if (stealthExitWaypoints.Contains(currentWaypointIndex))
                {
                    StartCoroutine(SetStealthMode(false));
                }

                if (currentWaypointIndex >= waypoints.Count)
                {
                    PlayerHealth.instance.TakeDamage(damage);
                    enemyHealth.DestroyEnemy();
                }
            }
        }
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
        
        float adjustedDuration = stealthDuration / stealthScaleSpeed; // Ajuste de velocidad

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
            StartCoroutine(KnockbackRoutine(direction, force, duration));
        }
    }

    private IEnumerator KnockbackRoutine(Vector2 direction, float force, float duration)
    {
        isKnockedBack = true;
        Vector2 originalPosition = transform.position;
        Vector2 targetPosition = originalPosition + direction * force;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            transform.position = Vector2.Lerp(originalPosition, targetPosition, elapsed / duration);
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
