using UnityEngine;

public class ColliderScaler : MonoBehaviour
{
    public Transform initialTransform;
    private Vector3 initialPosition;
    private Vector3 initialScale;
    private Vector3 newPosition;
    private Vector3 newScale;
    private int lastScreenWidth;
    private int lastScreenHeight;

    void Start()
    {
        if (initialTransform != null)
        {
            initialPosition = initialTransform.position; 
        }
        else
        {
            initialPosition = transform.position;
        }
        initialScale = transform.localScale;
        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
        HandleResolution(Screen.width, Screen.height);
    }

    void Update()
    {
        if (Screen.width != lastScreenWidth || Screen.height != lastScreenHeight)
        {
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
            HandleResolution(Screen.width, Screen.height);
        }
    }

    void HandleResolution(int width, int height)
    {
        Debug.Log("Resolución cambiada a: " + width + "x" + height);

        if (width == 1920 && height == 1080)
        {
            transform.position = initialPosition;
            transform.localScale = initialScale;
        }
         if (width == 1680 && height == 1050)
        {
            Debug.Log("Modo 1680x1050 activado.");
            Debug.Log("posicion inicial en x: " + initialPosition.x);
            if (initialPosition.x >= -9f && initialPosition.x < -7f)
            {
                Debug.Log("dentro del rango");
                newPosition = initialPosition;
                newPosition.x += 0.8f;
                transform.position = newPosition;
                newPosition.x -= 0.8f;
                newScale = initialScale;
                newScale.x *= 0.8993f;
                transform.localScale = newScale;
                newScale.x /= 0.8993f;
            }
            else if (initialPosition.x >= -7f && initialPosition.x < -6f)
            {
                newPosition = initialPosition;
                newPosition.x += 0.775f;
                transform.position = newPosition;
                newPosition.x -= 0.775f;
                newScale = initialScale;
                newScale.x *= 0.8993f;
                transform.localScale = newScale;
                newScale.x /= 0.8993f;
            }
            else if (initialPosition.x >= -6f && initialPosition.x < -5f)
            {
                newPosition = initialPosition;
                newPosition.x += 0.65f;
                transform.position = newPosition;
                newPosition.x -= 0.65f;
                newScale = initialScale;
                newScale.x *= 0.8993f;
                transform.localScale = newScale;
                newScale.x /= 0.8993f;
            }
            else if (initialPosition.x >= -5f && initialPosition.x < -4f)
            {
                newPosition = initialPosition;
                newPosition.x += 0.425f;
                transform.position = newPosition;
                newPosition.x -= 0.425f;
                newScale = initialScale;
                newScale.x *= 0.8993f;
                transform.localScale = newScale;
                newScale.x /= 0.8993f;
            }
            else if (initialPosition.x >= -4f && initialPosition.x < -3f)
            {
                newPosition = initialPosition;
                newPosition.x += 0.4f;
                transform.position = newPosition;
                newPosition.x -= 0.4f;
                newScale = initialScale;
                newScale.x *= 0.8993f;
                transform.localScale = newScale;
                newScale.x /= 0.8993f;
            }
            else if (initialPosition.x >= -3f && initialPosition.x < -2f)
            {
                newPosition = initialPosition;
                newPosition.x += 0.375f;
                transform.position = newPosition;
                newPosition.x -= 0.375f;
                newScale = initialScale;
                newScale.x *= 0.8993f;
                transform.localScale = newScale;
                newScale.x /= 0.8993f;
            }
            else if (initialPosition.x >= -2f && initialPosition.x < -1f)
            {
                newPosition = initialPosition;
                newPosition.x += 0.25f;
                transform.position = newPosition;
                newPosition.x -= 0.25f;
                newScale = initialScale;
                newScale.x *= 0.8993f;
                transform.localScale = newScale;
                newScale.x /= 0.8993f;
            }
            else if (initialPosition.x >= -1f && initialPosition.x < 0f)
            {
                newPosition = initialPosition;
                newPosition.x += 0.125f;
                transform.position = newPosition;
                newPosition.x -= 0.125f;
                newScale = initialScale;
                newScale.x *= 0.8993f;
                transform.localScale = newScale;
                newScale.x /= 0.8993f;
            }
            else if (initialPosition.x >= 0f && initialPosition.x < 1f)
            {
                newPosition = initialPosition;
                newPosition.x -= 0.125f;
                transform.position = newPosition;
                newPosition.x += 0.125f;
                newScale = initialScale;
                newScale.x *= 0.8993f;
                transform.localScale = newScale;
                newScale.x /= 0.8993f;
            }
            else if (initialPosition.x >= 1f && initialPosition.x < 2f)
            {
                newPosition = initialPosition;
                newPosition.x -= 0.25f;
                transform.position = newPosition;
                newPosition.x += 0.25f;
                newScale = initialScale;
                newScale.x *= 0.8993f;
                transform.localScale = newScale;
                newScale.x /= 0.8993f;
            }
            else if (initialPosition.x >= 2f && initialPosition.x < 3f)
            {
                newPosition = initialPosition;
                newPosition.x -= 0.375f;
                transform.position = newPosition;
                newPosition.x += 0.375f;
                newScale = initialScale;
                newScale.x *= 0.8993f;
                transform.localScale = newScale;
                newScale.x /= 0.8993f;
            }
            else if (initialPosition.x >= 3f && initialPosition.x < 4f)
            {
                newPosition = initialPosition;
                newPosition.x -= 0.4f;
                transform.position = newPosition;
                newPosition.x += 0.4f;
                newScale = initialScale;
                newScale.x *= 0.8993f;
                transform.localScale = newScale;
                newScale.x /= 0.8993f;
            }
            else if (initialPosition.x >= 4f && initialPosition.x < 5f)
            {
                newPosition = initialPosition;
                newPosition.x -= 0.425f;
                transform.position = newPosition;
                newPosition.x += 0.425f;
                newScale = initialScale;
                newScale.x *= 0.8993f;
                transform.localScale = newScale;
                newScale.x /= 0.8993f;
            }
            else if (initialPosition.x >= 5f && initialPosition.x < 6f)
            {
                newPosition = initialPosition;
                newPosition.x -= 0.55f;
                transform.position = newPosition;
                newPosition.x += 0.55f;
                newScale = initialScale;
                newScale.x *= 0.8993f;
                transform.localScale = newScale;
                newScale.x /= 0.8993f;
            }
            else if (initialPosition.x >= 6f && initialPosition.x < 7f)
            {
                newPosition = initialPosition;
                newPosition.x -= 0.675f;
                transform.position = newPosition;
                newPosition.x += 0.675f;
                newScale = initialScale;
                newScale.x *= 0.8993f;
                transform.localScale = newScale;
                newScale.x /= 0.8993f;
            }
            else if (initialPosition.x >= 7f && initialPosition.x < 9f)
            {
                newPosition = initialPosition;
                newPosition.x -= 0.8f;
                transform.position = newPosition;
                newPosition.x += 0.8f;
                newScale = initialScale;
                newScale.x *= 0.8993f;
                transform.localScale = newScale;
                newScale.x /= 0.8993f;
            }

        }
        else if (width == 1280 && height == 720)
        {
            Debug.Log("Modo HD activado.");
            // Lógica específica para 1280x720
        }
        else if (width == 1366 && height == 768)
        {
            Debug.Log("Modo WXGA activado.");
            // Lógica específica para 1366x768
        }
        else
        {
            Debug.Log("Resolución no reconocida, aplicando configuración por defecto.");
            // Lógica genérica para otras resoluciones
        }
    }
}