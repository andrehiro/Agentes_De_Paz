using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int currentResources = 500;
    
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

    void Start()
    {
        UIManager.instance.UpdateResourcesText(currentResources);
    }
    
    public bool CanAfford(int amount)
    {
        return currentResources >= amount;
    }
    
    public void SpendResources(int amount)
    {
        currentResources -= amount;
        UIManager.instance.UpdateResourcesText(currentResources);
    }

    public void GainResources(int amount)
    {
        currentResources += amount;
        UIManager.instance.UpdateResourcesText(currentResources);
    }
    
    
}