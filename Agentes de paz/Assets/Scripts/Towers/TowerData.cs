using UnityEngine;

public class TowerData : MonoBehaviour
{
    public int towerLevel;
    public string targetingMode;
    public Vector2 normalizedPosition;
    public Vector3 baseScale; // escala original al colocar

    void Start()
    {
        towerLevel = 0;
        targetingMode = "Primero";
    }

    public void UpdateTargetingMode()
    {
        if (targetingMode == "Primero") targetingMode = "Ultimo";
        else if (targetingMode == "Ultimo") targetingMode = "Fuerte";
        else if (targetingMode == "Fuerte") targetingMode = "Primero";
    }
}