using UnityEngine;

public class WindTower : Tower
{
    public float knockbackForce = 5f;

    protected override void ShootAtEnemy(GameObject enemy)
    {
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Vector3 direction = (enemy.transform.position - firePoint.position).normalized;

        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }

        WindProjectile windProjectile = projectile.GetComponent<WindProjectile>();
        if (windProjectile != null)
        {
            windProjectile.damage = damage;
            windProjectile.pierce = pierce;
            windProjectile.knockbackForce = knockbackForce;
        }
    }
}