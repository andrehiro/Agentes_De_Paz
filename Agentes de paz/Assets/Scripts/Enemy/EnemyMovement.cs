using UnityEngine;
using System.Collections.Generic;

public class EnemyMovement : MonoBehaviour
{
    public List<Transform> waypoints;
    public int currentWaypointIndex = 0;
    public float speed = 3f;
    public float damage = 10f;

    void Update()
    {
        if (waypoints != null && waypoints.Count > 0)
        {
            MoveToWaypoint();
        }
    }

    void MoveToWaypoint()
    {
        if (currentWaypointIndex < waypoints.Count)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];
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
}