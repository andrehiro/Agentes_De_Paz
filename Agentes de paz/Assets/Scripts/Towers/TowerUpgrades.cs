using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerUpgrades : MonoBehaviour
{
    public int upgradeCost1 = 120;
    public int upgradeCost2 = 200;
    private bool firstUpgrade = false;
    private bool maxUpgrade = false;

    public Button upgradeButton;
    public TextMeshProUGUI sellValueText;
    public TextMeshProUGUI upgradeCostText;
    public Sprite towerUpgradeSprite1;
    public Sprite towerUpgradeSprite2;

    void Update()
    {
        if (maxUpgrade)
        {
            upgradeButton.interactable = false;
            upgradeCostText.text = "Máx alcanzado";
        }
        else
        {
            int currentUpgradeCost = firstUpgrade ? upgradeCost2 : upgradeCost1;
            upgradeButton.interactable = GameManager.instance.currentResources >= currentUpgradeCost;
            upgradeCostText.text = "Mejorar = " + currentUpgradeCost.ToString();
        }
    }

    public void UpgradeTower()
    {
        if (maxUpgrade) return;  // No permitir mejoras adicionales

        int currentUpgradeCost = firstUpgrade ? upgradeCost2 : upgradeCost1;

        if (!CheckAndSpendResources(currentUpgradeCost)) return;

        Tower tower = GetComponent<Tower>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        tower.range *= 1.2f;
        tower.fireRate *= 1.2f;
        TowerPlacer.instance.SetTowerRangeIndicator(gameObject);

        if (!firstUpgrade)
        {
            spriteRenderer.sprite = towerUpgradeSprite1;
            firstUpgrade = true;
        }
        else
        {
            spriteRenderer.sprite = towerUpgradeSprite2;
            maxUpgrade = true;
        }
    }

    public bool CheckAndSpendResources(int price)
    {
        if (GameManager.instance.CanAfford(price))
        {
            GameManager.instance.SpendResources(price);
            GetComponent<Tower>().cost += price;
            UpdateSellValueText(Mathf.RoundToInt(GetComponent<Tower>().cost * GetComponent<Tower>().sellValueReturn));
            return true;
        }
        else
        {
            Debug.Log("Not enough resources to upgrade tower");
            return false;
        }
    }

    public void UpdateSellValueText(int currentSellValue)
    {
        sellValueText.text = "Venta = " + currentSellValue.ToString();
    }

    public void SellTower()
    {
        GameManager.instance.GainResources(Mathf.RoundToInt(GetComponent<Tower>().cost * GetComponent<Tower>().sellValueReturn));
        Destroy(gameObject);
    }
}
