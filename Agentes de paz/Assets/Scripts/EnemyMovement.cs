using UnityEngine;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour
{
    public List<Transform> waypoints; 
    private int currentWaypointIndex = 0;
    public float speed = 3f;
    public float damage = 10f;

    void Update()
    {
        if (waypoints != null && waypoints.Count > 0)
        {
            MoveToWaypoint();
        }
    }

    // Método para mover el enemigo hacia el siguiente waypoint
    void MoveToWaypoint()
    {
        if (currentWaypointIndex < waypoints.Count)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];
            if (targetWaypoint != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

                if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
                {
                    currentWaypointIndex++;

                    if (currentWaypointIndex >= waypoints.Count)
                    {
                        EnemyManager.instance.UnregisterEnemy();
                        PlayerHealth.instance.TakeDamage(damage);
                        // Llamar a Die() en el enemigo actual, no en la instancia estática
                        GetComponent<EnemyHealth>().Die();
                    }
                }
            }
        }
    }

    // Método para calcular el progreso del enemigo hacia el final
    public float GetProgress()
    {
        if (waypoints == null || waypoints.Count == 0)
        {
            return float.MaxValue; // Si no hay waypoints, el progreso es infinito (enemigo no válido)
        }
        // Calcula el progreso basado en la distancia recorrida y la distancia total
        float totalDistance = 0f;
        float distanceCovered = 0f;

        for (int i = 0; i < waypoints.Count - 1; i++)
        {
            totalDistance += Vector3.Distance(waypoints[i].position, waypoints[i + 1].position);
            if (i < currentWaypointIndex)
            {
                distanceCovered += Vector3.Distance(waypoints[i].position, waypoints[i + 1].position);
            }
            else if (i == currentWaypointIndex)
            {
                distanceCovered += Vector3.Distance(transform.position, waypoints[i + 1].position);
            }
        }

        return distanceCovered / totalDistance;
    }
}