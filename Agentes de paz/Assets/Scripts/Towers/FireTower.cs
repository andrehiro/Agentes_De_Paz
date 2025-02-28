using UnityEngine;

public class FireTower : Tower
{
    [Header("Multi Shot Stats")]
    public int projectileCount = 3;       // Número de proyectiles por disparo
    public float angleBetweenShots = 15f; // Ángulo entre cada proyectil

    protected override void ShootAtEnemy(GameObject enemy)
    {
        Vector3 baseDirection = (enemy.transform.position - firePoint.position).normalized;
        
        // Calcular ángulos para el patrón de disparo
        float startAngle = -(projectileCount - 1) * angleBetweenShots / 2f;

        for (int i = 0; i < projectileCount; i++)
        {
            // Calcular rotación para cada proyectil
            float currentAngle = startAngle + (i * angleBetweenShots);
            Quaternion rotation = Quaternion.Euler(0, 0, currentAngle);
            Vector3 shotDirection = rotation * baseDirection;

            // Crear y configurar proyectil
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            
            // Aplicar velocidad
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = shotDirection * projectileSpeed;
            }

            // Configurar parámetros del proyectil
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.SetTower(this);
            }
        }
    }
}