using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float maxHealth = 50f; 
    private float currentHealth;

    void Start()
    {
        // Inicializar la salud actual al valor máximo
        currentHealth = maxHealth;
    }

    // Método para recibir daño
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Método para manejar la muerte del enemigo
    private void Die()
    {
        Destroy(gameObject);
    }
}