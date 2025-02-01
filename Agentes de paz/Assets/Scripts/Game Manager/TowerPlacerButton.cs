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

    void SelectTower()
    {
        towerPlacer.SelectTowerPrefab(towerPrefab);
    }
}
