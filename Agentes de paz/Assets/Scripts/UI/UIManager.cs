using UnityEngine;
using TMPro; 
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject towerUpgradesUI;
    public Animator animatorTowerUpgradesUI;
    public GameObject winGameUI;
    public GameObject lossGameUI;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI resourcesText;
    public TextMeshProUGUI upgradeCostText;
    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI sellValueText;
    public TextMeshProUGUI targetingButtonText;
    public Button upgradeTowerButton;
    public Image towerUpgradeUIImage;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateHealthText(float currentHealth)
    {
        healthText.text = currentHealth.ToString();
    }

    public void UpdateWaveText(float currentWave)
    {
        waveText.text = "Ronda: " + currentWave.ToString();
    }

    public void UpdateResourcesText(int currentResources)
    {
        resourcesText.text = currentResources.ToString();
    }

    public void ShowWinGameUI()
    {
        winGameUI.SetActive(true);
    }

    public void ShowLoseGameUI()
    {
        lossGameUI.SetActive(true);
    }
}