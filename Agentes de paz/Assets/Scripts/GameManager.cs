using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject endGameCanvas;
    public PlayerHealth playerHealth;

    void OnEnable()
    {
        // Suscribirse al evento OnEnemyDeath
        EnemyMovement.OnEnemyReachedEnd += ApplyDamageToPlayer;
    }

    void OnDisable()
    {
        // Desuscribirse del evento cuando el objeto se desactive
        EnemyMovement.OnEnemyReachedEnd -= ApplyDamageToPlayer;
    }

    // Método que aplica daño al jugador
    void ApplyDamageToPlayer(float damage)
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);  // Aplicar el daño al jugador
        }
    }

    public void ShowEndGameUI()
    {
        endGameCanvas.SetActive(true);
        Time.timeScale = 0f; 
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void QuitGame()
    {
        Application.Quit(); 
        Debug.Log("Juego cerrado"); 
    }
}