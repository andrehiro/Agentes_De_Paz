using UnityEngine;
using UnityEngine.UI;

public class TowerUpgrades : MonoBehaviour
{
    public void UpgradeTower()
    {
        if(GameManager.instance.CanAfford(40))
        {
            GameManager.instance.SpendResources(40);
            GetComponent<Tower>().range *= 1.2f;
            GetComponent<Tower>().fireRate *= 1.2f;
            GetComponent<Tower>().cost += 40;
            TowerPlacer.instance.SetTowerRangeIndicator(gameObject);
        }
        else
        {
            Debug.Log("Not enough resources to upgrade tower");
        }

        
    }

    public void SellTower()
    {
        // Agregar los recursos al jugador
        GameManager.instance.GainResources(Mathf.RoundToInt(GetComponent<Tower>().cost * 0.7f));

        // Destruir la torre
        Destroy(gameObject);
    }
}
