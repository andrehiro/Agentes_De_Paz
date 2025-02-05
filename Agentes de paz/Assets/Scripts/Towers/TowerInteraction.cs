using UnityEngine;
using UnityEngine.EventSystems;

public class TowerInteraction : MonoBehaviour
{
    public GameObject towerUpgradesUI;
    private bool isUIActive = false;
    private bool isPlaced = false;

    void OnMouseDown()
    {
        if (!isPlaced)
        {
            isPlaced = true;
        }
        else
        {
            // Si el clic fue en la UI, no hacer nada
            if (EventSystem.current.IsPointerOverGameObject()) return;

            // Activar UI solo si la torre ya fue colocada
            towerUpgradesUI.SetActive(true);
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
                // Si el clic no fue en la torre ni en la UI, ocultar el UI
                towerUpgradesUI.SetActive(false);
                isUIActive = false;
            }
        }
    }
}
