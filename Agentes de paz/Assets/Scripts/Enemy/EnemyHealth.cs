using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f;
    public int resources = 100;
    private bool isCounted = false;
    private float currentHealth;

    private EnemyMovement enemyMovement;

    void Start()
    {
        currentHealth = maxHealth;
        enemyMovement = GetComponent<EnemyMovement>(); // Obtener referencia al movimiento
    }

    // Método para recibir daño
    public void TakeDamage(float damage)
    {
        // ❌ Si el enemigo está en stealth, no recibe daño
        if (enemyMovement != null && enemyMovement.IsInvulnerable())
        {
            return;
        }

        if (currentHealth <= 0) return;

        currentHealth -= damage;

        if (currentHealth <= 0 && !isCounted)
        {
            isCounted = true; // Marcar como contabilizado
            EnemyKilled();
        }
    }

    // Método para destruir al enemigo
    public void EnemyKilled()
    {
        // Agregar un efecto visual opcional antes de destruir (si lo deseas)
        // GetComponent<SpriteRenderer>().color = Color.red;

        Destroy(gameObject, 0.1f);
        EnemyManager.instance.UnregisterEnemy();
        GameManager.instance.GainResources(resources);
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
        EnemyManager.instance.UnregisterEnemy();
    }
}