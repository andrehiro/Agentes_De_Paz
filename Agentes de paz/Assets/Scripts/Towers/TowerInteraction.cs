using UnityEngine;
using UnityEngine.EventSystems;

public class TowerInteraction : MonoBehaviour
{
    public GameObject towerUpgradesUI;
    private bool isUIActive = false;
    private bool isPlaced = false; 

    void OnMouseDown()
    {
        // Si el clic fue en la UI, no hacer nada
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (IsInRestrictedArea()) return;

        if (!isPlaced)
        {
            isPlaced = true;
        }
        else
        {
            towerUpgradesUI.SetActive(true);
            transform.Find("RangeIndicator")?.gameObject.SetActive(true);
            isUIActive = true;
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
                towerUpgradesUI.SetActive(false);
                transform.Find("RangeIndicator")?.gameObject.SetActive(false);
                isUIActive = false;
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
    
}