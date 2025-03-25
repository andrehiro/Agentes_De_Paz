using UnityEngine;

public class TowerSelectionManager : MonoBehaviour
{
    public static TowerSelectionManager instance;
    public Tower selectedTower;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void SelectTower(Tower tower)
    {
        selectedTower = tower;
    }
}