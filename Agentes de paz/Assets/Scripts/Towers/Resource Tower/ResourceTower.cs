using UnityEngine;

public class ResourceTower : Tower
{
    public int resourcesPerWave = 50;

    protected override void Start() 
    {
        towerUpgrades = GetComponent<TowerUpgrades>();
        towerUpgrades.UpdateSellValueText();
        EnemySpawner.OnWaveCompleted += HandleWaveCompleted;
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
    }
}