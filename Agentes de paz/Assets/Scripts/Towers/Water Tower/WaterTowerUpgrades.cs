using UnityEngine;

public class WaterTowerUpgrades : TowerUpgrades
{
    void Start()
    {
        TowerUpgrades selectedTower = TowerSelectionManager.instance.selectedTower.GetComponent<TowerUpgrades>();
        
        selectedTower.upgradeText = "Mejora 1";
        UIManager.instance.upgradeText.text = selectedTower.upgradeText;
    }

    public override void UpgradeTower()
    {
        if (maxUpgrade) return; 

        int currentUpgradeCost = !firstUpgrade ? upgradeCost1 : (!secondUpgrade ? upgradeCost2 : upgradeCost3);

        if (!CheckAndSpendResources(currentUpgradeCost)) return;

        Tower tower = GetComponent<Tower>();
        TowerUpgrades selectedTower = TowerSelectionManager.instance.selectedTower.GetComponent<TowerUpgrades>();
        TowerData selectedTowerData = TowerSelectionManager.instance.selectedTower.GetComponent<TowerData>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (!firstUpgrade)
        {
            // Primera mejora
            tower.fireRate = 1.7f;
            tower.damage = 5f;
            spriteRenderer.sprite = towerUpgradeSprite1;
            firstUpgrade = true;

            selectedTower.upgradeText = "Mejora 2";
        }
        else if (!secondUpgrade)
        {
            // Segunda mejora
            tower.fireRate = 3.5f;
            tower.damage = 5f;
            spriteRenderer.sprite = towerUpgradeSprite2;
            secondUpgrade = true;

            selectedTower.upgradeText = "Mejora 3";
        }
        else
        {
            // Tercera mejora
            tower.fireRate = 9f;
            tower.damage = 5f;
            tower.range = 6f;
            tower.rangeAdjustment = 8.5f;
            TowerPlacer.instance.SetTowerRangeIndicator(gameObject);
            spriteRenderer.sprite = towerUpgradeSprite3;
            maxUpgrade = true;

            selectedTower.upgradeText = "";
        }

        selectedTowerData.towerLevel++;
    }
}