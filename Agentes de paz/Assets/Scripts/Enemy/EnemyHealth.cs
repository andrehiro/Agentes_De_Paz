using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f;
    public int resources = 100;
    private bool isCounted = false;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;  
    }

    // Método para recibir daño
    public void TakeDamage(float damage)
    {
        if(currentHealth <= 0) return;

        currentHealth -= damage;

        if(currentHealth <= 0 && !isCounted)
        {
            isCounted = true; // Marcar como contabilizado
            EnemyKilled();
        }
    }

    // Método para destruir al enemigo
    public void EnemyKilled()
    {
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