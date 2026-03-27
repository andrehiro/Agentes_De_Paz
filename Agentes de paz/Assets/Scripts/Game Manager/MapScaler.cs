using UnityEngine;
using System.Collections;

public class MapScaler : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(AdjustNextFrame());
    }

    public void AdjustMap()
    {
        StartCoroutine(AdjustNextFrame());
    }

    private IEnumerator AdjustNextFrame()
    {
        yield return null;

        float cameraHeight = mainCamera.orthographicSize * 2f;
        float cameraWidth = cameraHeight * ((float)Screen.width / Screen.height);

        float spriteHeight = spriteRenderer.sprite.bounds.size.y;
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;

        transform.localScale = new Vector3(
            cameraWidth / spriteWidth,
            cameraHeight / spriteHeight,
            1f
        );
    }
}