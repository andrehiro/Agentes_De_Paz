using UnityEngine;
using UnityEngine.UI;

public class TowerSlideUI : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float slideSpeed = 5f;
    [SerializeField] private RectTransform panelRect;
    [SerializeField] private Button toggleButton;

    [Header("Posiciones")]
    [SerializeField] private Vector2 expandedPosition;
    [SerializeField] private Vector2 collapsedPosition;

    private bool isExpanded = false;

    void Start()
    {
        toggleButton.onClick.AddListener(ToggleTowerSlideUI);
        panelRect.anchoredPosition = collapsedPosition; // Estado inicial
    }

    void ToggleTowerSlideUI()
    {
        isExpanded = !isExpanded;
        StopAllCoroutines();
        StartCoroutine(TowerSlideUICoroutine());
    }

    private System.Collections.IEnumerator TowerSlideUICoroutine()
    {
        Vector2 targetPosition = isExpanded ? expandedPosition : collapsedPosition;
        
        while(Vector2.Distance(panelRect.anchoredPosition, targetPosition) > 0.1f)
        {
            panelRect.anchoredPosition = Vector2.Lerp(
                panelRect.anchoredPosition,
                targetPosition,
                slideSpeed * Time.deltaTime
            );
            yield return null;
        }
        
        panelRect.anchoredPosition = targetPosition; // Asegurar posición final
    }
}