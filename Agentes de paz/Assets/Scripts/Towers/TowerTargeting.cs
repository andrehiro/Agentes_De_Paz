using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TowerTargeting : MonoBehaviour
{
    private GameObject targetEnemy;
    public TextMeshProUGUI targetingButtonText;

    public GameObject GetTarget(string targetingMode)
    {
        if (targetingMode == "Primero") return FindMostAdvancedEnemy();
        if (targetingMode == "Ultimo") return FindLeastAdvancedEnemy();
        if (targetingMode == "Fuerte") return FindHighestHealthEnemy();
        return null;
    }

    private GameObject FindMostAdvancedEnemy()
    {
        GameObject bestTarget = null;
        int highestWaypointIndex = -1;
        float smallestDistanceToWaypoint = float.MaxValue;

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (!IsWithinRange(enemy)) continue;

            EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
            if (enemyMovement == null) continue;

            int currentIndex = enemyMovement.currentWaypointIndex;
            float distanceToWaypoint = GetDistanceToWaypoint(enemyMovement, enemy);

            if (currentIndex > highestWaypointIndex || 
                (currentIndex == highestWaypointIndex && distanceToWaypoint < smallestDistanceToWaypoint))
            {
                highestWaypointIndex = currentIndex;
                smallestDistanceToWaypoint = distanceToWaypoint;
                bestTarget = enemy;
            }
        }
        return bestTarget;
    }

    private GameObject FindLeastAdvancedEnemy()
    {
        GameObject bestTarget = null;
        int lowestWaypointIndex = int.MaxValue;
        float largestDistanceToWaypoint = -1f;

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (!IsWithinRange(enemy)) continue;

            EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
            if (enemyMovement == null) continue;

            int currentIndex = enemyMovement.currentWaypointIndex;
            float distanceToWaypoint = GetDistanceToWaypoint(enemyMovement, enemy);

            if (currentIndex < lowestWaypointIndex || 
                (currentIndex == lowestWaypointIndex && distanceToWaypoint > largestDistanceToWaypoint))
            {
                lowestWaypointIndex = currentIndex;
                largestDistanceToWaypoint = distanceToWaypoint;
                bestTarget = enemy;
            }
        }
        return bestTarget;
    }

    private GameObject FindHighestHealthEnemy()
    {
        GameObject bestTarget = null;
        float highestMaxHealth = -1f;

        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (!IsWithinRange(enemy)) continue;

            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth == null) continue;

            if (enemyHealth.maxHealth > highestMaxHealth)
            {
                highestMaxHealth = enemyHealth.maxHealth;
                bestTarget = enemy;
            }
        }
        return bestTarget;
    }

    private bool IsWithinRange(GameObject enemy)
    {
        Tower tower = GetComponent<Tower>();
        return Vector3.Distance(transform.position, enemy.transform.position) <= tower.range;
    }

    private float GetDistanceToWaypoint(EnemyMovement enemyMovement, GameObject enemy)
    {
        if (enemyMovement.waypoints.Count == 0) return float.MaxValue;
        Transform currentWaypoint = enemyMovement.waypoints[Mathf.Clamp(enemyMovement.currentWaypointIndex, 0, enemyMovement.waypoints.Count - 1)];
        return Vector3.Distance(enemy.transform.position, currentWaypoint.position);
    }

    public void UpdateTargetingButtonText()
    {
        if (targetingButtonText.text == "Primero")
        {
            targetingButtonText.text = "Ultimo";
        }
        else if (targetingButtonText.text == "Ultimo")
        {
            targetingButtonText.text = "Fuerte";
        }
        else if (targetingButtonText.text == "Fuerte")
        {
            targetingButtonText.text = "Primero";
        }
    }
}
