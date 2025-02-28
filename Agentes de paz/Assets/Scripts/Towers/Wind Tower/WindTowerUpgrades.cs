using UnityEngine;

public class WindTowerUpgrades : TowerUpgrades
{
    public override void UpgradeTower()
    {
        if (maxUpgrade) return;  // No permitir mejoras adicionales

        int currentUpgradeCost = firstUpgrade ? upgradeCost2 : upgradeCost1;

        if (!CheckAndSpendResources(currentUpgradeCost)) return;

        Tower tower = GetComponent<Tower>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (!firstUpgrade)
        {
            tower.fireRate = 1.7f;
            tower.damage = 5f;
            spriteRenderer.sprite = towerUpgradeSprite1;
            firstUpgrade = true;
        }
        else
        {
            tower.range *= 1.2f;
            tower.fireRate = 1.8f;
            tower.damage = 10f;
            TowerPlacer.instance.SetTowerRangeIndicator(gameObject);
            spriteRenderer.sprite = towerUpgradeSprite2;
            maxUpgrade = true;
        }
    }
}
