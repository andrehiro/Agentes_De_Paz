using UnityEngine;

public class WindTowerUpgrades : TowerUpgrades
{
    void Start()
    {
        TowerUpgrades selectedTower = TowerSelectionManager.instance.selectedTower.GetComponent<TowerUpgrades>();
        
        selectedTower.upgradeText = "Aumentos menores en todas sus estadisticas";
        UIManager.instance.upgradeText.text = selectedTower.upgradeText;
    }

    public override void UpgradeTower()
    {
        if (maxUpgrade) return; 

        int currentUpgradeCost = !firstUpgrade ? upgradeCost1 : (!secondUpgrade ? upgradeCost2 : upgradeCost3);

        if (!CheckAndSpendResources(currentUpgradeCost)) return;

        Tower tower = GetComponent<Tower>();
        WindTower windtower = GetComponent<WindTower>();
        TowerUpgrades selectedTower = TowerSelectionManager.instance.selectedTower.GetComponent<TowerUpgrades>();
        TowerData selectedTowerData = TowerSelectionManager.instance.selectedTower.GetComponent<TowerData>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (!firstUpgrade)
        {
            // Primera mejora
            tower.fireRate = 0.3f;
            windtower.knockbackForce = 4f;
            tower.range = 6f;
            tower.rangeAdjustment = 8.5f;
            TowerPlacer.instance.SetTowerRangeIndicator(gameObject);
            spriteRenderer.sprite = towerUpgradeSprite1;
            firstUpgrade = true;

            selectedTower.upgradeText = "Aumenta la velocidad de disparo y el retroceso";
        }
        else if (!secondUpgrade)
        {
            // Segunda mejora
            tower.fireRate = 0.5f;
            windtower.knockbackForce = 5f;
            spriteRenderer.sprite = towerUpgradeSprite2;
            secondUpgrade = true;

            selectedTower.upgradeText = "Duplica su penetracion y retroceso";
        }
        else
        {
            // Tercera mejora
            tower.damage = 5f;
            tower.pierce = 8;
            windtower.knockbackForce = 10f;
            TowerPlacer.instance.SetTowerRangeIndicator(gameObject);
            spriteRenderer.sprite = towerUpgradeSprite3;
            maxUpgrade = true;

            selectedTower.upgradeText = "";
        }

        selectedTowerData.towerLevel++;
    }
}
