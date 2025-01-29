using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public int enemiesAlive = 0;

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

    // Método para registrar un enemigo
    public void RegisterEnemy()
    {
        enemiesAlive++;
    }

    // Método para desregistrar un enemigo
    public void UnregisterEnemy()
    {
        enemiesAlive--;
    }
}
