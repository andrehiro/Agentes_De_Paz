using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class TowerUpgrades : MonoBehaviour
{
    public int upgradeCost = 120;
    public Button upgradeButton;
    public TextMeshProUGUI upgradeCostText;
    public TextMeshProUGUI sellValueText;
    public TextMeshProUGUI targetingButtonText;
    public Sprite towerUpgradeSprite1;
    public Sprite towerUpgradeSprite2;
    private bool maxUpgrade = false;

    void Update()
    {
        if (upgradeCost > GameManager.instance.currentResources || maxUpgrade)
        {
            upgradeButton.interactable = false;
        }
        else
        {
            upgradeButton.interactable = true;
        }
    }

    public void UpgradeTower()
    {
        if (!CheckAndSpendResources(upgradeCost)) return;

        Tower tower = GetComponent<Tower>();
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        tower.range *= 1.2f;
        tower.fireRate *= 1.2f;
        TowerPlacer.instance.SetTowerRangeIndicator(gameObject);

        if (spriteRenderer.sprite == towerUpgradeSprite1)
        {
            spriteRenderer.sprite = towerUpgradeSprite2;
            maxUpgrade = true;
        }
        else
        {
            spriteRenderer.sprite = towerUpgradeSprite1;
        }
    }

    public bool CheckAndSpendResources(int price)
    {
        if (GameManager.instance.CanAfford(price))
        {
            GameManager.instance.SpendResources(price);
            GetComponent<Tower>().cost += price;
            UpdateSellValueText(Mathf.RoundToInt(GetComponent<Tower>().cost * 0.7f));
            return true;
        }
        else
        {
            Debug.Log("Not enough resources to upgrade tower");
            return false;
        }
    }
    public void UpdateTargetingButtonText()
    {
        if (targetingButtonText.text == "Primero")
        {
            targetingButtonText.text = "Último";
        }
        else
        {
            targetingButtonText.text = "Primero";
        }
    }

    public void UpdateSellValueText(int currentSellValue)
    {
        sellValueText.text = "Venta = "+currentSellValue.ToString();
    }
    
    public void SellTower()
    {
        GameManager.instance.GainResources(Mathf.RoundToInt(GetComponent<Tower>().cost * 0.7f));
        Destroy(gameObject);
    }

}