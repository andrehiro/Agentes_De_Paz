using UnityEngine;
using TMPro; 

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject winGameUI;
    public GameObject lossGameUI;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI resourcesText;

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
        healthText.text = "Health: " + currentHealth.ToString();
    }

    public void UpdateWaveText(float currentWave)
    {
        waveText.text = "Wave: " + currentWave.ToString();
    }

    public void UpdateResourcesText(int currentResources)
    {
        resourcesText.text = $"Resources: {currentResources}";
    }

    public void ShowWinGameUI()
    {
        winGameUI.SetActive(true);
    }

    public void ShowlossGameUI()
    {
        lossGameUI.SetActive(true);
    }
}