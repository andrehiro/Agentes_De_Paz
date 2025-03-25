using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TowerInteraction : MonoBehaviour
{
    private bool isUIActive = false;
    private bool isPlaced = false;

    // Variable estática para hacer referencia a la torre cuya UI está activa
    private static TowerInteraction currentActiveTower = null;

    void OnMouseDown()
    {
        TowerData towerData = GetComponent<TowerData>();
        Tower selectedTower = TowerSelectionManager.instance.selectedTower;

        // Si el clic fue en la UI, no hacer nada
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (IsInRestrictedArea()) return;

        if (!isPlaced)
        {
            isPlaced = true;
        }
        else
        {
            // Si ya hay una torre activa, cerramos su UI
            if (currentActiveTower != null && currentActiveTower != this)
            {
                currentActiveTower.CloseTowerUI();
            }

            // Actualizar la UI de la torre
            UIManager.instance.towerUpgradeUIImage.sprite = selectedTower.GetComponent<SpriteRenderer>().sprite;
            UIManager.instance.targetingButtonText.text = towerData.targetingMode;
            UIManager.instance.upgradeText.text = selectedTower.GetComponent<TowerUpgrades>().upgradeText;
            selectedTower.GetComponent<TowerUpgrades>().UpdateSellValueText();

            // Mostrar la UI de la torre
            UIManager.instance.towerUpgradesUI.SetActive(true);
            UIManager.instance.animatorTowerUpgradesUI.SetBool("Closed", false);
            transform.Find("RangeIndicator")?.gameObject.SetActive(true);
            isUIActive = true;

            // Actualizar la referencia a la torre activa
            currentActiveTower = this;
        }
    }

    void Update()
    {
        // Si la UI está activa y el usuario hace clic en otro lado
        if (Input.GetMouseButtonDown(0) && isUIActive)
        {
            // Si el clic fue en un elemento UI, no hacer nada
            if (EventSystem.current.IsPointerOverGameObject()) return;

            // Verificar si el clic fue en la torre con un Raycast
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider == null || hit.collider.gameObject != gameObject)
            {
                // Ocultar el UI si el clic no fue en la torre
                CloseTowerUI();

                // Limpiar la referencia de la torre activa
                currentActiveTower = null;
            }
        }
    }

    private bool IsInRestrictedArea()
    {
        if (TowerPlacer.instance == null) return false;

        Collider2D towerCollider = GetComponent<Collider2D>();
        if (towerCollider == null) return false;

        // Usar la misma capa restringida de TowerPlacer
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(TowerPlacer.instance.restrictedLayer);
        filter.useTriggers = false;

        Physics2D.SyncTransforms(); 

        return towerCollider.Overlap(filter, new Collider2D[1]) > 0;
    }

    // Método para cerrar la UI de la torre
    public void CloseTowerUI()
    {
        if (isUIActive)
        {
            UIManager.instance.animatorTowerUpgradesUI.SetBool("Closed", true);
            transform.Find("RangeIndicator")?.gameObject.SetActive(false);
            isUIActive = false;
        }
    }
}