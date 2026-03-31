using UnityEngine;

public class TowerPlacer : MonoBehaviour
{
    public static TowerPlacer instance;
    private GameObject currentTower;
    private GameObject selectedTowerPrefab;
    public LayerMask restrictedLayer;

    [Header("Range Indicator Colors")]
    [SerializeField] private Color validColor = new Color(1f, 1f, 1f, 0.1f);
    [SerializeField] private Color invalidColor = new Color(1f, 0f, 0f, 0.3f);

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void SelectTowerPrefab(GameObject towerPrefab)
    {
        if (currentTower != null) return;

        selectedTowerPrefab = towerPrefab;
        currentTower = Instantiate(selectedTowerPrefab);
        SetTowerPosition();
        DisableTowerFunctionality(currentTower);
        SetTowerRangeIndicator(currentTower);
        ShowTowerRange(true);

        // Ajustar escala de la torre al instanciarla segun resolucion actual
        AdjustTowerScaleToCurrentResolution(currentTower);
    }

    void AdjustTowerScaleToCurrentResolution(GameObject tower)
    {
        ResolutionController rc = FindAnyObjectByType<ResolutionController>();
        if (rc == null) return;

        float camHeight = Camera.main.orthographicSize * 2f;
        float baseOrthoHeight = rc.baseOrthographicSize * 2f;
        float scaleRatio = camHeight / baseOrthoHeight;

        Vector3 originalScale = tower.transform.localScale;
        tower.transform.localScale = new Vector3(
            originalScale.x * scaleRatio,
            originalScale.y * scaleRatio,
            originalScale.z
        );
    }

    void Update()
    {
        if (currentTower != null)
        {
            SetTowerPosition();

            Vector3 mousePos = Input.mousePosition;
            if (mousePos.x < 0 || mousePos.x > Screen.width ||
                mousePos.y < 0 || mousePos.y > Screen.height)
            {
                CancelTowerPlacement();
                return;
            }

            if (!IsPlacementValid())
                ChangeChildSpriteColor(currentTower, "RangeIndicator", invalidColor);
            else
                ChangeChildSpriteColor(currentTower, "RangeIndicator", validColor);

            if (Input.GetMouseButtonDown(0))
                PlaceTower();
            else if (Input.GetMouseButtonDown(1))
                CancelTowerPlacement();
        }
    }

    void SetTowerPosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        currentTower.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
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

                TowerData towerData = currentTower.GetComponent<TowerData>();
                if (towerData != null)
                {
                    Camera cam = Camera.main;
                    float camHeight = cam.orthographicSize * 2f;
                    float camWidth = camHeight * ((float)Screen.width / Screen.height);

                    Vector3 pos = currentTower.transform.position;
                    towerData.normalizedPosition = new Vector2(
                        pos.x / camWidth,
                        pos.y / camHeight
                    );

                    // Guardar escala base dividida por ratio actual
                    ResolutionController rc = FindAnyObjectByType<ResolutionController>();
                    float baseOrthoHeight = rc != null ? rc.baseOrthographicSize * 2f : camHeight;
                    float scaleRatio = camHeight / baseOrthoHeight;

                    towerData.baseScale = new Vector3(
                        currentTower.transform.localScale.x / scaleRatio,
                        currentTower.transform.localScale.y / scaleRatio,
                        currentTower.transform.localScale.z
                    );
                }

                currentTower = null;
            }
        }
    }

    bool IsPlacementValid()
    {
        if (currentTower == null) return false;

        Collider2D towerCollider = currentTower.GetComponent<Collider2D>();
        if (towerCollider == null) return false;

        Physics2D.SyncTransforms();

        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(restrictedLayer);
        filter.useTriggers = false;

        return towerCollider.Overlap(filter, new Collider2D[1]) == 0;
    }

    public void SetTowerRangeIndicator(GameObject tower)
    {
        Transform rangeIndicator = tower.transform.Find("RangeIndicator");
        if (rangeIndicator != null)
        {
            Tower towerScript = tower.GetComponent<Tower>();
            if (towerScript != null)
            {
                float spriteDiameter = rangeIndicator.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
                float scaleFactor = (towerScript.range * towerScript.rangeAdjustment) / spriteDiameter;
                rangeIndicator.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
            }
        }
    }

    void ShowTowerRange(bool show)
    {
        if (currentTower != null)
        {
            Transform rangeIndicator = currentTower.transform.Find("RangeIndicator");
            if (rangeIndicator != null)
                rangeIndicator.gameObject.SetActive(show);
        }
    }

    void ChangeChildSpriteColor(GameObject parent, string childName, Color newColor)
    {
        Transform childTransform = parent.transform.Find(childName);
        if (childTransform != null)
        {
            SpriteRenderer spriteRenderer = childTransform.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
                spriteRenderer.color = newColor;
        }
    }

    void DisableTowerFunctionality(GameObject tower)
    {
        Collider2D col = tower.GetComponent<Collider2D>();
        if (col != null) col.isTrigger = true;

        MonoBehaviour[] scripts = tower.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
            if (script != this) script.enabled = false;
    }

    void EnableTowerFunctionality(GameObject tower)
    {
        Collider2D col = tower.GetComponent<Collider2D>();
        if (col != null) col.isTrigger = false;

        MonoBehaviour[] scripts = tower.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
            script.enabled = true;
    }

    public void CancelTowerPlacement()
    {
        if (currentTower != null)
        {
            Destroy(currentTower);
            ShowTowerRange(false);
            currentTower = null;
        }
    }
}