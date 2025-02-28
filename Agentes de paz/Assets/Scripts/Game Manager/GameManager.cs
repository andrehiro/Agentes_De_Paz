using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private PlayerHealth playerHealth;
    public int currentResources = 350;
    private bool gameOver = false;

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
        UIManager.instance.UpdateResourcesText(currentResources);
        playerHealth = PlayerHealth.instance;

        // Suscribirse a eventos
        EnemySpawner.OnAllWavesCompleted += CheckVictory;
        playerHealth.OnPlayerDeath += HandleDefeat;
    }

    void OnDestroy()
    {
        EnemySpawner.OnAllWavesCompleted -= CheckVictory;
        playerHealth.OnPlayerDeath -= HandleDefeat;
    }

    public bool CanAfford(int amount)
    {
        return currentResources >= amount;
    }
    
    public void SpendResources(int amount)
    {
        currentResources -= amount;
        UIManager.instance.UpdateResourcesText(currentResources);
    }

    public void GainResources(int amount)
    {
        currentResources += amount;
        UIManager.instance.UpdateResourcesText(currentResources);
    }

    private void CheckVictory()
    {
        StartCoroutine(WaitForEnemiesToDie());
    }

    private IEnumerator WaitForEnemiesToDie()
    {
        yield return new WaitUntil(() => EnemyManager.instance.enemiesAlive <= 0);

        if (gameOver) yield break;

        gameOver = true;
        UIManager.instance.ShowWinGameUI();
        TowerPlacer.instance.CancelTowerPlacement();
        yield return new WaitForSeconds(1f);
        Time.timeScale = 0f;
    }

    private void HandleDefeat()
    {
        if (gameOver) return;

        gameOver = true; 
        UIManager.instance.ShowLoseGameUI();
        TowerPlacer.instance.CancelTowerPlacement();
        Time.timeScale = 0f;
    }
}
