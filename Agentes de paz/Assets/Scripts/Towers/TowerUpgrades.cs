using UnityEngine;
using UnityEngine.UI;
using TMPro;

// INTERFAZ
public interface ITowerUpgrades
{
    int GetUpgradeCost(int level);
    void UpgradeTower();
}

// CLASE BASE
public abstract class TowerUpgrades : MonoBehaviour, ITowerUpgrades
{
    public int upgradeCost1 = 120;
    public int upgradeCost2 = 200;
    public int upgradeCost3 = 350;

    public bool firstUpgrade = false;
    public bool secondUpgrade = false;
    public bool maxUpgrade = false;

    public Sprite towerUpgradeSprite1;
    public Sprite towerUpgradeSprite2;
    public Sprite towerUpgradeSprite3;

    public string upgradeText;

    void Update()
    {
        if (TowerSelectionManager.instance.selectedTower == null)
        {
            UIManager.instance.upgradeTowerButton.interactable = false;
            return;
        }

        Tower selected = TowerSelectionManager.instance.selectedTower;
        TowerData selectedTowerData = selected.GetComponent<TowerData>();
        ITowerUpgrades upgrades = selected.GetComponent<ITowerUpgrades>();

        if (selectedTowerData.towerLevel >= 3)
        {
            UIManager.instance.upgradeTowerButton.interactable = false;
            UIManager.instance.upgradeCostText.text = "Maximo";
        }
        else
        {
            int cost = upgrades.GetUpgradeCost(selectedTowerData.towerLevel);
            UIManager.instance.upgradeTowerButton.interactable = GameManager.instance.currentResources >= cost;
            UIManager.instance.upgradeCostText.text = "Mejorar " + cost;
        }
    }

    public int GetUpgradeCost(int level)
    {
        return level switch
        {
            0 => upgradeCost1,
            1 => upgradeCost2,
            2 => upgradeCost3,
            _ => 0
        };
    }

    public bool CheckAndSpendResources(int price)
    {
        if (GameManager.instance.CanAfford(price))
        {
            GameManager.instance.SpendResources(price);
            GetComponent<Tower>().cost += price;
            UpdateSellValueText();
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateSellValueText()
    {
        UIManager.instance.sellValueText.text = "Venta " + Mathf.RoundToInt(GetComponent<Tower>().cost * GetComponent<Tower>().sellValueReturn).ToString();
    }

    public void SellTower()
    {
        Tower selectedTower = TowerSelectionManager.instance.selectedTower.GetComponent<Tower>();
        TowerInteraction towerInteraction = TowerSelectionManager.instance.selectedTower.GetComponent<TowerInteraction>();

        GameManager.instance.GainResources(Mathf.RoundToInt(selectedTower.cost * selectedTower.sellValueReturn));
        towerInteraction.CloseTowerUI();
        Destroy(selectedTower.gameObject);
    }

    public void UpgradeTowerGeneral()
    {
        Tower selected = TowerSelectionManager.instance.selectedTower;

        ITowerUpgrades upgrades = selected.GetComponent<ITowerUpgrades>();
        upgrades?.UpgradeTower();

        UIManager.instance.towerUpgradeUIImage.sprite = selected.GetComponent<SpriteRenderer>().sprite;
        UIManager.instance.upgradeText.text = selected.GetComponent<TowerUpgrades>().upgradeText;
    }

    public abstract void UpgradeTower();
}
