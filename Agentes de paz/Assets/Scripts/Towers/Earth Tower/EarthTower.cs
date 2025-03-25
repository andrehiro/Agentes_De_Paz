using UnityEngine;

public class EarthTower : Tower
{
    protected override void Start()
    {
        base.Start();
        TowerPlacer.instance.SetTowerRangeIndicator(gameObject);
    }

    protected override void ShootAtEnemy(GameObject enemy)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Vector3 direction = (enemy.transform.position - firePoint.position).normalized;

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }

        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.SetTower(this); // Pasamos la referencia completa de la torre
        }
    }
}