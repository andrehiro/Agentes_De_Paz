using UnityEngine;
using UnityEngine.UI;

public class TowerPlacerButton : MonoBehaviour
{
    public GameObject towerPrefab;
    private TowerPlacer towerPlacer;
    private Button button;
    private Tower tower;

    void Start()
    {
        towerPlacer = Object.FindAnyObjectByType<TowerPlacer>();
        button = GetComponent<Button>();
        tower = towerPrefab.GetComponent<Tower>();
        button.onClick.AddListener(SelectTower);
    }

    void Update()
    {
        button.interactable = tower.cost <= GameManager.instance.currentResources;
    }

    void SelectTower()
    {
        towerPlacer.SelectTowerPrefab(towerPrefab);
    }
}