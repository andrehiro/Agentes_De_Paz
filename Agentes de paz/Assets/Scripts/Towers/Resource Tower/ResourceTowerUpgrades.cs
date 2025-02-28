using UnityEngine;

public class ResourceTowerUpgrade : TowerUpgrades
{
    public override void UpgradeTower()
    {
        if (maxUpgrade) return;  // No permitir mejoras adicionales

        int currentUpgradeCost = firstUpgrade ? upgradeCost2 : upgradeCost1;

        if (!CheckAndSpendResources(currentUpgradeCost)) return;

        ResourceTower tower = GetComponent<ResourceTower>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (!firstUpgrade)
        {
            tower.resourcesPerWave = 100;
            spriteRenderer.sprite = towerUpgradeSprite1;
            firstUpgrade = true;
        }
        else
        {
            tower.resourcesPerWave = 200;
            spriteRenderer.sprite = towerUpgradeSprite2;
            maxUpgrade = true;
        }
    }
}
