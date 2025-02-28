using UnityEngine;

public class WindProjectile : MonoBehaviour
{
    private WindTower tower;
    private int enemiesHit = 0;
    private float lifeTime = 5f;
    private bool isActive = true;
    
    // Variables cacheadas
    private int cachedPierce;
    private float cachedDamage;
    private float cachedKnockbackForce;
    private float cachedKnockbackDuration;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public void SetTower(WindTower sourceTower)
    {
        tower = sourceTower;
        cachedDamage = tower.damage;
        cachedPierce = tower.pierce;
        cachedKnockbackForce = tower.knockbackForce;
        cachedKnockbackDuration = tower.knockbackDuration;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive || tower == null) return;

        if (collision.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
            EnemyMovement enemyMovement = collision.GetComponent<EnemyMovement>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(cachedDamage);
            }

            if (enemyMovement != null)
            {
                Vector2 knockbackDirection = -enemyMovement.GetCurrentDirection();
                enemyMovement.ApplyKnockback(knockbackDirection, cachedKnockbackForce, cachedKnockbackDuration);
            }

            enemiesHit++;
            if (enemiesHit >= cachedPierce)
            {
                isActive = false;
                GetComponent<Collider2D>().enabled = false;
                Destroy(gameObject);
            }
        }
    }
}