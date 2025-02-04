using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    private GameObject currentTower;
    private GameObject selectedTowerPrefab;

    [SerializeField] private LayerMask restrictedLayer; // Capa de zonas prohibidas
    public void SelectTowerPrefab(GameObject towerPrefab)
    {
        if (currentTower != null) return;

        selectedTowerPrefab = towerPrefab;
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

            // Cambiar color si la torre está en una zona prohibida
            if (!IsPlacementValid())
            {
                ChangeChildSpriteColor(currentTower, "RangeIndicator", new Color(1f, 0f, 0f, 0.3f)); // Rojo semi-transparente
            }
            else
            {
                ChangeChildSpriteColor(currentTower, "RangeIndicator", new Color(1f, 1f, 1f, 0.1f)); // Blanco con 30% de transparencia
            }

            if (Input.GetMouseButtonDown(0))
            {
                PlaceTower();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                CancelTowerPlacement();
            }
        }
    }

    void SetTowerPosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentTower.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        
        // Añadir esto para actualización precisa de colisiones
        Physics2D.SyncTransforms();
    }   

    void PlaceTower()
    {
        if (currentTower != null && GameManager.instance.CanAfford(selectedTowerPrefab.GetComponent<Tower>().cost))
        {
            if (IsPlacementValid())
            {
                GameManager.instance.SpendResources(selectedTowerPrefab.GetComponent<Tower>().cost);
                EnableTowerFunctionality(currentTower);
                ShowTowerRange(false);
                currentTower = null;
            }
            else
            {
                Debug.Log("No se puede colocar la torre aquí");
            }
        }
    }

    bool IsPlacementValid()
    {
        if (currentTower == null) return false;

        Collider2D towerCollider = currentTower.GetComponent<Collider2D>();
        if (towerCollider == null) return false;

        // Fuerza la actualización inmediata de la posición física
        Physics2D.SyncTransforms();

        // Configurar filtro de capa
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(restrictedLayer);
        filter.useTriggers = false;

        // Verificar colisiones con la forma EXACTA del collider
        return towerCollider.Overlap(filter, new Collider2D[1]) == 0;
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

    void ChangeChildSpriteColor(GameObject parent, string childName, Color newColor)
    {
        Transform childTransform = parent.transform.Find(childName);
        if (childTransform != null)
        {
            SpriteRenderer spriteRenderer = childTransform.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = newColor;
            }
        }
    }

    void DisableTowerFunctionality(GameObject tower)
    {
        // Mantener el collider habilitado pero como trigger
        Collider2D col = tower.GetComponent<Collider2D>();
        if (col != null) {
            col.isTrigger = true; // Permite que el collider detecte pero no cause físicas
        }

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
        if (col != null) {
            col.isTrigger = false; // Restaurar comportamiento normal
        }

        MonoBehaviour[] scripts = tower.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = true;
        }
    }

    void CancelTowerPlacement()
{
    if (currentTower != null)
    {
        // Destruir la torre preview
        Destroy(currentTower);
        ShowTowerRange(false);
        currentTower = null;
        Debug.Log("Colocación cancelada");
    }
}
}
