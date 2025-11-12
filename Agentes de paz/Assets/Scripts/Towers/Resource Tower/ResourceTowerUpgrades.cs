using UnityEngine;

public class ResourceTowerUpgrades : TowerUpgrades
{
    void Start()
    {
        TowerUpgrades selectedTower = TowerSelectionManager.instance.selectedTower.GetComponent<TowerUpgrades>();
        
        selectedTower.upgradeText = "Aumento menor en los recursos generados";
        UIManager.instance.upgradeText.text = selectedTower.upgradeText;
    }

    public override void UpgradeTower()
    {
        if (maxUpgrade) return; 

        int currentUpgradeCost = !firstUpgrade ? upgradeCost1 : (!secondUpgrade ? upgradeCost2 : upgradeCost3);

        if (!CheckAndSpendResources(currentUpgradeCost)) return;

        ResourceTower tower = GetComponent<ResourceTower>();
        TowerUpgrades selectedTower = TowerSelectionManager.instance.selectedTower.GetComponent<TowerUpgrades>();
        TowerData selectedTowerData = TowerSelectionManager.instance.selectedTower.GetComponent<TowerData>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (!firstUpgrade)
        {
            // Primera mejora
            tower.resourcesPerWave = 130;
            spriteRenderer.sprite = towerUpgradeSprite1;
            firstUpgrade = true;

            selectedTower.upgradeText = "Aumento mayor en los recursos generados";
        }
        else if (!secondUpgrade)
        {
            // Segunda mejora
            tower.resourcesPerWave = 250;
            spriteRenderer.sprite = towerUpgradeSprite2;
            secondUpgrade = true;

            selectedTower.upgradeText = "Triplica la cantidad de recursos generados";
        }
        else
        {
            // Tercera mejora
            tower.resourcesPerWave = 750;
            spriteRenderer.sprite = towerUpgradeSprite3;
            maxUpgrade = true;

            selectedTower.upgradeText = "";
        }

        selectedTowerData.towerLevel++;
    }
}