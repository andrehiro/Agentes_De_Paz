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
                if (currentWaypointIndex >= waypoints.Count)
                {
                    PlayerHealth.instance.TakeDamage(damage);
                    GetComponent<EnemyHealth>().DestroyEnemy();
                }
            }
        }
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