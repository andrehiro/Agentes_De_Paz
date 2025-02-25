using UnityEngine;

public class WindProjectile : MonoBehaviour
{
    public float damage = 10f;
    public int pierce = 1;
    public float knockbackForce = 5f;
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
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
            Rigidbody2D enemyRb = collision.GetComponent<Rigidbody2D>();
            EnemyMovement enemyMovement = collision.GetComponent<EnemyMovement>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            if (enemyRb != null && enemyMovement != null)
            {
                // Intentar calcular la dirección inversa del movimiento del enemigo

                // Aplicar la fuerza de knockback

            }

            enemiesHit++;

            if (enemiesHit >= pierce)
            {
                Destroy(gameObject);
            }
        }
    }
}
