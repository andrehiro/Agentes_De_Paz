using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class TowerTargeting : MonoBehaviour
{
    private GameObject targetEnemy;

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
            if (!IsValidTarget(enemy)) continue;

            EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
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
            if (!IsValidTarget(enemy)) continue;

            EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
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
            if (!IsValidTarget(enemy)) continue;

            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

            if (enemyHealth.maxHealth > highestMaxHealth)
            {
                highestMaxHealth = enemyHealth.maxHealth;
                bestTarget = enemy;
            }
        }
        return bestTarget;
    }

    private bool IsValidTarget(GameObject enemy)
    {
        if (!IsWithinRange(enemy)) return false;

        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
        if (enemyMovement == null || enemyMovement.IsInvulnerable()) return false; // Ignorar si está en stealth

        return true;
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
        // Verifica si hay una torre seleccionada
        if (TowerSelectionManager.instance.selectedTower == null) return;

        // Obtiene el TowerData de la torre seleccionada
        TowerData selectedTowerData = TowerSelectionManager.instance.selectedTower.GetComponent<TowerData>();
        if (selectedTowerData == null) return;

        // Cambia el modo de targeting
        selectedTowerData.UpdateTargetingMode();

        // Actualiza el texto del botón en la UI
        UIManager.instance.targetingButtonText.text = selectedTowerData.targetingMode;
    }

}