using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    private Tower tower; 
    private int enemiesHit = 0;
    private float lifeTime = 5f;
    private bool isActive = true;
    private Collider2D projectileCollider;

    private float cachedDamage;
    private int cachedPierce;

    public void SetTower(Tower sourceTower)
    {
        tower = sourceTower;
        cachedDamage = tower.damage;
        cachedPierce = tower.pierce;
        projectileCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive || tower == null) return;

        if (collision.CompareTag("Enemy"))
        {
            EnemyMovement enemyMovement = collision.GetComponent<EnemyMovement>();

            // Si el enemigo está en stealth, ignorar el impacto
            if (enemyMovement != null && enemyMovement.IsInvulnerable())
            {
                return;
            }

            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(cachedDamage);
            }

            enemiesHit++;

            if (enemiesHit >= cachedPierce)
            {
                isActive = false;
                if (projectileCollider != null)
                {
                    projectileCollider.enabled = false;
                }
                StartCoroutine(DestroyAfterDelay(0.03f));
            }
        }
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
