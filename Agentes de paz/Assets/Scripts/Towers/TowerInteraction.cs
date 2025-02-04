using UnityEngine;

public class TowerInteraction : MonoBehaviour
{
    void OnMouseDown()
    {
        // Mostrar/ocultar UI de mejoras a través del UIManager
        if (UIManager.instance.towerUpgradesUI.activeSelf)
        {
            UIManager.instance.towerUpgradesUI.SetActive(false);
        }
        else
        {
            // Ocultar UI de otras torres y mostrar esta
            UIManager.instance.towerUpgradesUI.SetActive(true);
        }
    }

    void Update()
    {
        // Cerrar el UI al hacer clic fuera
        if (UIManager.instance.towerUpgradesUI.activeSelf && Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
            if (hit.collider == null || hit.collider.gameObject != gameObject)
            {
                UIManager.instance.towerUpgradesUI.SetActive(false);
            }
        }
    }
}