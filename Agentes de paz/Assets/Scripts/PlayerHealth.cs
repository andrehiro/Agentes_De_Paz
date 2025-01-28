using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public GameManager gameManager;

    [Header("Configuración de Vida")]
    public float maxHealth = 100f; 
    private float currentHealth; 

    [Header("UI de Vida")]
    public TextMeshProUGUI healthText; 

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    // Método para aplicar daño al jugador
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Limitar la vida entre 0 y maxHealth
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Método para curar al jugador
    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Limitar la vida entre 0 y maxHealth
        UpdateHealthUI();
    }

    // Método para actualizar la UI de vida
    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Vida: " + currentHealth.ToString("0"); 
        }
    }

    // Método para manejar la muerte del jugador
    private void Die()
    {
        gameManager.ShowEndGameUI();
    }
}
