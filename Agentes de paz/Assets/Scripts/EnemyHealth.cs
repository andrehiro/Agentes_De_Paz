using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f;
    private float currentHealth;
    public delegate void DeathEventHandler();
    public event DeathEventHandler onDeath;  // Evento cuando el enemigo muere

    void Start()
    {
        currentHealth = maxHealth;  // Inicializar la salud
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (onDeath != null)
        {
            onDeath();  // Llamar el evento de muerte
        }
        Destroy(gameObject);  // Eliminar al enemigo de la escena
    }
}
