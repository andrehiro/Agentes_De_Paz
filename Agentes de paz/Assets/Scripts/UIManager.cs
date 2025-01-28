using UnityEngine;
using TMPro; 

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject winGameUI;
    public GameObject lossGameUI;
    public TextMeshProUGUI health;
    public TextMeshProUGUI waves;

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
        health.text = "Health: " + currentHealth.ToString();
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