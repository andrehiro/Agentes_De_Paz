using UnityEngine;

public class TowerData : MonoBehaviour
{
    public int towerLevel;
    public string targetingMode;
    public Vector3 position;

    void Start()
    {
        towerLevel = 0;
        targetingMode = "Primero";
        Debug.Log("Hola");
    }

    void Awake()
    {

    }

    public void UpdateTargetingMode()
    {
        if (targetingMode == "Primero")
        {
            targetingMode = "Ultimo";
        }
        else if (targetingMode == "Ultimo")
        {
            targetingMode = "Fuerte";
        }
        else if (targetingMode == "Fuerte")
        {
            targetingMode = "Primero";
        }
    }

    public void SaveTowerData(Tower tower)
    {

    }
}