using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;
    public int enemiesAlive = 0;  // Número de enemigos vivos
    public EnemySpawner enemySpawner; // Referencia al EnemySpawner

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

    public void RegisterEnemy()
    {
        enemiesAlive++; // Incrementar cuando un nuevo enemigo es registrado
    }

    public void UnregisterEnemy()
    {
        enemiesAlive--; // Decrementar cuando un enemigo muere
        if (enemiesAlive <= 0)
        {
            enemySpawner.OnEnemyDied(); // Notificar al spawner cuando todos los enemigos han muerto
        }
    }
}
