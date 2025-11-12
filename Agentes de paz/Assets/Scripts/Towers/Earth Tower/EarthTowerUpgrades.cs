using UnityEngine;

public class EarthTowerUpgrades : TowerUpgrades
{
    void Start()
    {
        TowerUpgrades selectedTower = TowerSelectionManager.instance.selectedTower.GetComponent<TowerUpgrades>();
        
        selectedTower.upgradeText = "Aumenta la velocidad de disparo";
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
            tower.fireRate = 0.35f;
            spriteRenderer.sprite = towerUpgradeSprite1;
            firstUpgrade = true;

            selectedTower.upgradeText = "Duplica su poder y aumenta la velocidad de disparo";
        }
        else if (!secondUpgrade)
        {
            // Segunda mejora
            tower.fireRate = 0.4f;
            tower.damage = 65f;
            spriteRenderer.sprite = towerUpgradeSprite2;
            secondUpgrade = true;

            selectedTower.upgradeText = "Aumenta significativamente su poder";
        }
        else
        {
            // Tercera mejora
            tower.fireRate = 0.4f;
            tower.damage = 150f;
            spriteRenderer.sprite = towerUpgradeSprite3;
            maxUpgrade = true;

            selectedTower.upgradeText = "";
        }

        selectedTowerData.towerLevel++;
    }
}
