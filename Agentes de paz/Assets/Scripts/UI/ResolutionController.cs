using UnityEngine;

public class ResolutionController : MonoBehaviour
{
    public Vector2Int[] resolutions = {
        new Vector2Int(1920, 1080),
        new Vector2Int(1280, 720),
        new Vector2Int(1200, 800)
    };

    void Start()
    {
        LoadSavedResolution();
    }

    // Cambia a pantalla completa
    public void SetFullscreen()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        PlayerPrefs.SetInt("FullscreenMode", (int)Screen.fullScreenMode);
        PlayerPrefs.Save();
    }

    // Cambia a modo ventana
    public void SetWindowed()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
        PlayerPrefs.SetInt("FullscreenMode", (int)Screen.fullScreenMode);
        PlayerPrefs.Save();
        
        // Aplica la resolución guardada
        int width = PlayerPrefs.GetInt("ScreenWidth", 1280);
        int height = PlayerPrefs.GetInt("ScreenHeight", 720);
        Screen.SetResolution(width, height, false);
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutionIndex < 0 || resolutionIndex >= resolutions.Length) return;
        
        Vector2Int newRes = resolutions[resolutionIndex];
        Screen.SetResolution(newRes.x, newRes.y, Screen.fullScreenMode);
        SaveResolution(newRes);
    }

    private void SaveResolution(Vector2Int resolution)
    {
        PlayerPrefs.SetInt("ScreenWidth", resolution.x);
        PlayerPrefs.SetInt("ScreenHeight", resolution.y);
        PlayerPrefs.Save();
    }

    private void LoadSavedResolution()
    {
        // Cargar configuración guardada
        int width = PlayerPrefs.GetInt("ScreenWidth", Screen.currentResolution.width);
        int height = PlayerPrefs.GetInt("ScreenHeight", Screen.currentResolution.height);
        FullScreenMode savedMode = (FullScreenMode)PlayerPrefs.GetInt("FullscreenMode", (int)FullScreenMode.Windowed);
        
        // Aplicar configuración
        Screen.SetResolution(width, height, savedMode);
    }
}