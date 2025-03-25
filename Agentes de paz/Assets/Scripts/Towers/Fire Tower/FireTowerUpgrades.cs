using UnityEngine;

public class FireTowerUpgrades : TowerUpgrades
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
        FireTower firetower = GetComponent<FireTower>();
        TowerUpgrades selectedTower = TowerSelectionManager.instance.selectedTower.GetComponent<TowerUpgrades>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (!firstUpgrade)
        {
            // Primera mejora
            tower.damage = 4f;
            tower.fireRate = 1.2f;
            firetower.projectileCount = 5;
            tower.range = 2.8f;
            TowerPlacer.instance.SetTowerRangeIndicator(gameObject);
            spriteRenderer.sprite = towerUpgradeSprite1;
            firstUpgrade = true;

            selectedTower.upgradeText = "Mejora 2";
        }
        else if (!secondUpgrade)
        {
            // Segunda mejora
            tower.fireRate = 2f;
            tower.damage = 6f;
            spriteRenderer.sprite = towerUpgradeSprite2;
            secondUpgrade = true;

            selectedTower.upgradeText = "Mejora 3";
        }
        else
        {
            // Tercera mejora
            tower.fireRate = 2.5f;
            tower.damage = 10f;
            firetower.projectileCount = 10;
            firetower.angleBetweenShots = 8f;
            tower.range = 3f;
            TowerPlacer.instance.SetTowerRangeIndicator(gameObject);
            spriteRenderer.sprite = towerUpgradeSprite3;
            maxUpgrade = true;

            selectedTower.upgradeText = "";
        }

        towerLevel++;
    }
}
