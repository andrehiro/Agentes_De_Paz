using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;
    public float maxHealth = 100f;
    private float currentHealth;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Inicializar la salud actual al valor máximo
        currentHealth = maxHealth;
        UIManager.instance.UpdateHealthText(currentHealth);
    }

    // Método para recibir daño
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        UIManager.instance.UpdateHealthText(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Método para manejar la muerte del jugador
    private void Die()
    {
        UIManager.instance.ShowlossGameUI();
    }
}