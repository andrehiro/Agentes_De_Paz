using UnityEngine;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour
{
    public List<Transform> waypoints;
    public float speed = 3f;
    private int currentWaypointIndex = 0;

    void Update()
    {
        // Si hay waypoints, mover al enemigo
        if (waypoints != null && waypoints.Count > 0)
        {
            MoveToWaypoint();
        }
    }

    // Mover al enemigo hacia el waypoint
    void MoveToWaypoint()
    {
        // Obtener el waypoint actual
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // Mover hacia el waypoint
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        // Rotar hacia el waypoint solo en el eje Z
        Vector3 direction = targetWaypoint.position - transform.position;
        direction.z = 0f;
        if (direction.magnitude > 0.1f)
        {
            // Calcular la rotación para el eje Z
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        // Verificar si el enemigo ha llegado al waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            // Pasar al siguiente waypoint
            currentWaypointIndex++;

            // Si se alcanzó el último waypoint, destruir el enemigo
            if (currentWaypointIndex >= waypoints.Count)
            {
                Destroy(gameObject);
            }
        }
    }
}
