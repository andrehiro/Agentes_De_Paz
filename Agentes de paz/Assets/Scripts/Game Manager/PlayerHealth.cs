using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;
    public float maxHealth = 100f;
    private float currentHealth;
    public event Action OnPlayerDeath;

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
            PlayerDie();
        }
    }

    // Método para manejar la muerte del jugador
    private void PlayerDie()
    {
        OnPlayerDeath?.Invoke();
    }
}