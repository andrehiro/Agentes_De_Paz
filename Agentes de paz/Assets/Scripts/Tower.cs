using UnityEngine;
using System.Collections.Generic;

public class Tower : MonoBehaviour
{
    public float range = 10f;
    public float fireRate = 1f; 
    public float projectileSpeed = 10f;
    public GameObject projectilePrefab; 
    public Transform firePoint; 
    private float fireCooldown = 0f;

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        // Buscar el enemigo más cercano al final
        GameObject targetEnemy = FindFirstEnemy();

        if (targetEnemy != null && fireCooldown <= 0f)
        {
            // Disparar hacia el enemigo
            ShootAtEnemy(targetEnemy);
            fireCooldown = 1f / fireRate; // Resetear cooldown según la tasa de disparo
        }
    }

    GameObject FindFirstEnemy()
    {
        GameObject firstEnemy = null;
        float closestToEnd = float.MaxValue; // Una medida del "progreso" hacia el final

        // Buscar todos los enemigos en el rango de la torre
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);

            // Verificar si el enemigo está en rango
            if (distance <= range)
            {
                // Obtener el progreso del enemigo hacia el final
                EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();
                if (enemyMovement != null && enemyMovement.GetProgress() < closestToEnd)
                {
                    closestToEnd = enemyMovement.GetProgress();
                    firstEnemy = enemy;
                }
            }
        }

        return firstEnemy;
    }

    void ShootAtEnemy(GameObject enemy)
    {
        // Crear el proyectil
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Establecer la dirección hacia el enemigo
        Vector3 direction = (enemy.transform.position - firePoint.position).normalized;

        // Obtener el componente Rigidbody2D del proyectil y aplicar la velocidad
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }
    }

    // Dibujar un círculo que representa el rango de la torre en el editor
    void OnDrawGizmosSelected()
    {
        // Establecer el color del Gizmo a verde
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f); // Color verde con transparencia

        // Dibujar un círculo (o esfera en 3D) con el radio de rango
        Gizmos.DrawWireSphere(transform.position, range);
    }
}