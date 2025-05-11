using UnityEngine;

public class ResourceTower : Tower
{
    public Canvas canvas;
    public int resourcesPerWave = 50;

    private FloatingText floatingText;

    protected override void Start() 
    {
        towerUpgrades = GetComponent<TowerUpgrades>();
        towerUpgrades.UpdateSellValueText();
        EnemySpawner.OnWaveCompleted += HandleWaveCompleted;

        floatingText = canvas.GetComponentInChildren<FloatingText>(true);
    }

    void OnDestroy()
    {
        EnemySpawner.OnWaveCompleted -= HandleWaveCompleted;
    }

    protected override void ShootAtEnemy(GameObject enemy) 
    {  
    }

    void HandleWaveCompleted(int waveNumber)
    {
        GameManager.instance.GainResources(resourcesPerWave);

        if (floatingText != null)
        {
            floatingText.gameObject.SetActive(true);
            floatingText.SetText("+" + resourcesPerWave);
        }
    }
}