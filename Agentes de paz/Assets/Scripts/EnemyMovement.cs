using UnityEngine;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour
{
    public List<Transform> waypoints; 
    private int currentWaypointIndex = 0;
    public float speed = 3f;
    public float damage = 10f; 
    public delegate void EnemyReachedEnd(float damage);  // Delegado que incluye el daño
    public static event EnemyReachedEnd OnEnemyReachedEnd;  // Evento de muerte del enemigo
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
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex++;

            // Si el enemigo ha llegado al final de la ruta, notificar la muerte
            if (currentWaypointIndex >= waypoints.Count)
            {
                Destroy(gameObject);
                NotifyEnemyReachedEnd();
            }
        }
    }

    // Método que indica que el enemigo ha llegado al final de su ruta
    void NotifyEnemyReachedEnd()
    {
        // Llamar al evento para notificar la muerte del enemigo y enviar el daño
        if (OnEnemyReachedEnd != null)
        {
            OnEnemyReachedEnd.Invoke(damage);
        }
        Destroy(gameObject);
    }

    // Método para calcular el progreso del enemigo hacia el final
    public float GetProgress()
    {
        if (waypoints == null || waypoints.Count == 0)
        {
            return float.MaxValue; // Si no hay waypoints, el progreso es infinito (enemigo no válido)
        }

        float progress = 0f;

        // Suma las distancias desde los waypoints anteriores
        for (int i = 0; i < currentWaypointIndex; i++)
        {
            progress += Vector3.Distance(waypoints[i].position, waypoints[i + 1].position);
        }

        // Suma la distancia restante al waypoint actual
        progress += Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position);

        return progress;
    }
}