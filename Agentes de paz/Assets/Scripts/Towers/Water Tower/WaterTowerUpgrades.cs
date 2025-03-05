using UnityEngine;

public class WaterTowerUpgrades : TowerUpgrades
{
    public override void UpgradeTower()
    {
        if (maxUpgrade) return; 

        int currentUpgradeCost = !firstUpgrade ? upgradeCost1 : (!secondUpgrade ? upgradeCost2 : upgradeCost3);

        if (!CheckAndSpendResources(currentUpgradeCost)) return;

        Tower tower = GetComponent<Tower>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (!firstUpgrade)
        {
            // Primera mejora
            tower.fireRate = 1.7f;
            tower.damage = 5f;
            spriteRenderer.sprite = towerUpgradeSprite1;
            firstUpgrade = true;
        }
        else if (!secondUpgrade)
        {
            // Segunda mejora
            tower.fireRate = 3.5f;
            tower.damage = 5f;
            spriteRenderer.sprite = towerUpgradeSprite2;
            secondUpgrade = true;
        }
        else
        {
            // Tercera mejora
            tower.fireRate = 9f;
            tower.damage = 5f;
            spriteRenderer.sprite = towerUpgradeSprite3;
            maxUpgrade = true;
        }
    }
}
