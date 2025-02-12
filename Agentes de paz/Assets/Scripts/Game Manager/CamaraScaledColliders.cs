using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ScaleColliderWithCamera : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private Camera mainCamera;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        mainCamera = Camera.main;
        AdjustCollider();
    }

    void Update()
    {
        AdjustCollider(); // Llamamos esto en Update en caso de que la resolución cambie
    }

    void AdjustCollider()
    {
        if (mainCamera == null) return;

        float camHeight = mainCamera.orthographicSize * 2;
        float camWidth = camHeight * mainCamera.aspect;

        // Ajustamos el tamaño del collider
        boxCollider.size = new Vector2(camWidth, camHeight);
    }
}