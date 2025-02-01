using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    private GameObject currentTower;
    private GameObject selectedTowerPrefab;

    public void SelectTowerPrefab(GameObject towerPrefab)
    {
        // Si ya hay una torre en proceso de colocación, no hacer nada
        if (currentTower != null)
        {
            return;
        }

        // Actualiza el prefab seleccionado
        selectedTowerPrefab = towerPrefab;

        // Crear la torre en la posición del mouse
        currentTower = Instantiate(selectedTowerPrefab);
        SetTowerPosition();
        DisableTowerFunctionality(currentTower);
        SetTowerRangeIndicator(currentTower);
        ShowTowerRange(true);
    }

    void Update()
    {
        if (currentTower != null)
        {
            SetTowerPosition();

            // Colocar la torre al hacer clic izquierdo
            if (Input.GetMouseButtonDown(0))
            {
                PlaceTower();
            }
        }
    }

    void SetTowerPosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentTower.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }

    void PlaceTower()
    {
        // Asegurarnos de que la torre se coloca en la posición actual
        if (currentTower != null)
        {
            EnableTowerFunctionality(currentTower);
            ShowTowerRange(false); // Ocultar el rango de la torre
            currentTower = null; // Liberar la torre para permitir colocar otra
        }
    }

    void SetTowerRangeIndicator(GameObject tower)
    {
        Transform rangeIndicator = tower.transform.Find("RangeIndicator");
        if (rangeIndicator != null)
        {
            Tower towerScript = tower.GetComponent<Tower>();
            if (towerScript != null)
            {
                float spriteDiameter = rangeIndicator.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
                float scaleFactor = (towerScript.range * 2f) / spriteDiameter;
                rangeIndicator.localScale = new Vector3(scaleFactor, scaleFactor, 1);
            }
        }
    }

    void ShowTowerRange(bool show)
    {
        if (currentTower != null)
        {
            Transform rangeIndicator = currentTower.transform.Find("RangeIndicator");
            if (rangeIndicator != null)
            {
                rangeIndicator.gameObject.SetActive(show);
            }
        }
    }

    void DisableTowerFunctionality(GameObject tower)
    {
        Collider2D col = tower.GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        MonoBehaviour[] scripts = tower.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            if (script != this)
                script.enabled = false;
        }
    }

    void EnableTowerFunctionality(GameObject tower)
    {
        Collider2D col = tower.GetComponent<Collider2D>();
        if (col != null) col.enabled = true;

        MonoBehaviour[] scripts = tower.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = true;
        }
    }
}
