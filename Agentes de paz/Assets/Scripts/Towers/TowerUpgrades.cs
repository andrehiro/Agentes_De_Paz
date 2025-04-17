using UnityEngine;
using UnityEngine.UI;
using TMPro;

public abstract class TowerUpgrades : MonoBehaviour
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
        // Verificar si hay una torre seleccionada
        if (TowerSelectionManager.instance.selectedTower == null)
        {
            UIManager.instance.upgradeTowerButton.interactable = false;
            return;
        }
        
        TowerData selectedTower = TowerSelectionManager.instance.selectedTower.GetComponent<TowerData>();
        
        if (selectedTower.towerLevel >= 3)
        {
            UIManager.instance.upgradeTowerButton.interactable = false;
            UIManager.instance.upgradeCostText.text = "Máx alcanzado";
        }
        else if(selectedTower.towerLevel == 0)
        {
            UIManager.instance.upgradeTowerButton.interactable = GameManager.instance.currentResources >= upgradeCost1;
            UIManager.instance.upgradeCostText.text = "Mejorar = " + upgradeCost1.ToString();
        }
        else if(selectedTower.towerLevel == 1)
        {
            UIManager.instance.upgradeTowerButton.interactable = GameManager.instance.currentResources >= upgradeCost2;
            UIManager.instance.upgradeCostText.text = "Mejorar = " + upgradeCost2.ToString();
        }
        else if(selectedTower.towerLevel == 2)
        {
            UIManager.instance.upgradeTowerButton.interactable = GameManager.instance.currentResources >= upgradeCost3;
            UIManager.instance.upgradeCostText.text = "Mejorar = " + upgradeCost3.ToString();
        }
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
        UIManager.instance.sellValueText.text = "Venta = " + Mathf.RoundToInt(GetComponent<Tower>().cost * GetComponent<Tower>().sellValueReturn).ToString();
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
        Tower selectedTower = TowerSelectionManager.instance.selectedTower;

        if (selectedTower.GetComponent<WaterTowerUpgrades>() != null)
        {
            selectedTower.GetComponent<WaterTowerUpgrades>().UpgradeTower();
        }
        else if (selectedTower.GetComponent<WindTowerUpgrades>() != null)
        {
            selectedTower.GetComponent<WindTowerUpgrades>().UpgradeTower();
        }
        else if (selectedTower.GetComponent<FireTowerUpgrades>() != null)
        {
            selectedTower.GetComponent<FireTowerUpgrades>().UpgradeTower();
        }
        else if (selectedTower.GetComponent<EarthTowerUpgrades>() != null)
        {
            selectedTower.GetComponent<EarthTowerUpgrades>().UpgradeTower();
        }
        else if (selectedTower.GetComponent<ResourceTowerUpgrades>() != null)
        {
            selectedTower.GetComponent<ResourceTowerUpgrades>().UpgradeTower();
        }
        
        UIManager.instance.towerUpgradeUIImage.sprite = selectedTower.GetComponent<SpriteRenderer>().sprite;
        UIManager.instance.upgradeText.text = selectedTower.GetComponent<TowerUpgrades>().upgradeText;
    }

    public abstract void UpgradeTower();
}