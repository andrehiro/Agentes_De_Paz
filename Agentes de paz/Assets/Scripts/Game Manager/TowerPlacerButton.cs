using UnityEngine;
using UnityEngine.UI;

public class TowerPlacerButton : MonoBehaviour
{
    public GameObject towerPrefab;
    private TowerPlacer towerPlacer;

    void Start()
    {
        towerPlacer = Object.FindFirstObjectByType<TowerPlacer>();
        GetComponent<Button>().onClick.AddListener(SelectTower);
    }

    void Update()
    {
        if (towerPrefab.GetComponent<Tower>().cost > GameManager.instance.currentResources)
        {
            GetComponent<Button>().interactable = false;
        }
        else
        {
            GetComponent<Button>().interactable = true;
        }

    }

    void SelectTower()
    {
        towerPlacer.SelectTowerPrefab(towerPrefab);
    }
}
