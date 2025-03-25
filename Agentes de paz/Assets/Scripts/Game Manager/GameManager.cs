using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int currentResources = 350;
    private PlayerHealth playerHealth;
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
        CompleteLevel();
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

    public void CompleteLevel()
    {
        string currentLevelName = SceneManager.GetActiveScene().name;
        string nextLevel = "";

        // Determinar el siguiente nivel en la secuencia
        switch (currentLevelName)
        {
            case "WindLevel1":
                nextLevel = "WindLevel2";
                break;
            case "WindLevel2":
                nextLevel = "FireLevel1";
                break;
            case "FireLevel1":
                nextLevel = "FireLevel2";
                break;
            case "FireLevel2":
                nextLevel = "EarthLevel1";
                break;
            case "EarthLevel1":
                nextLevel = "EarthLevel2";
                break;
            case "EarthLevel2":
                nextLevel = "WaterLevel2";
                break;
            case "WaterLevel1":
                nextLevel = "WaterLevel2";
                break;
        }

        // Si hay un siguiente nivel, lo desbloqueamos
        if (!string.IsNullOrEmpty(nextLevel))
        {
            PlayerPrefs.SetString("UnlockedLevels", nextLevel);
            PlayerPrefs.Save();
        }
    }
}
