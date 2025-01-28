using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10f;
    public int penetration = 1;

    private int enemiesHit = 0;
    private float lifeTime = 5f; 

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            // Aplicar daño al enemigo
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }
            enemiesHit++;

            // Si el proyectil ha alcanzado su límite de penetración, destruirlo
            if (enemiesHit >= penetration)
            {
                Destroy(gameObject);
            }
        }
    }
}