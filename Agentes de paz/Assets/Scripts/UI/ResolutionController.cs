using UnityEngine;
using TMPro;

public class ResolutionController : MonoBehaviour
{
    public TMP_Dropdown resolutionDropdown;

    public Vector2Int[] resolutions = {
        new Vector2Int(1920, 1080),
        new Vector2Int(1680, 1050),
        new Vector2Int(1600, 900),
        new Vector2Int(1440, 900),
        new Vector2Int(1366, 768),
        new Vector2Int(1280, 800),
        new Vector2Int(1280, 720),
        new Vector2Int(1024, 768),
        new Vector2Int(800, 600),
        new Vector2Int(640, 480),
    };

    void Start()
    {
        PopulateDropdown();
        LoadSavedResolution();
    }

    void PopulateDropdown()
    {
        resolutionDropdown.ClearOptions();
        
        // Crear opciones para el TMP_Dropdown
        foreach (Vector2Int res in resolutions)
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData($"{res.x} x {res.y}"));
        }

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        
        // Cargar resolución guardada
        int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        resolutionDropdown.value = savedIndex;
        resolutionDropdown.RefreshShownValue(); // Refrescar el texto mostrado en el Dropdown
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutionIndex < 0 || resolutionIndex >= resolutions.Length) return;
        
        Vector2Int newRes = resolutions[resolutionIndex];
        Screen.SetResolution(newRes.x, newRes.y, Screen.fullScreenMode);

        // Guardar configuración
        PlayerPrefs.SetInt("ScreenWidth", newRes.x);
        PlayerPrefs.SetInt("ScreenHeight", newRes.y);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.Save();
    }

    public void SetFullscreenMode()
    {
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
        PlayerPrefs.SetInt("FullscreenMode", (int)FullScreenMode.FullScreenWindow);
        PlayerPrefs.Save();
    }

    public void SetWindowedMode()
    {
        Screen.fullScreenMode = FullScreenMode.Windowed;
        PlayerPrefs.SetInt("FullscreenMode", (int)FullScreenMode.Windowed);
        PlayerPrefs.Save();

        // Aplicar la última resolución guardada
        int width = PlayerPrefs.GetInt("ScreenWidth", 1280);
        int height = PlayerPrefs.GetInt("ScreenHeight", 720);
        Screen.SetResolution(width, height, false);
    }

    private void LoadSavedResolution()
    {
        int width = PlayerPrefs.GetInt("ScreenWidth", Screen.currentResolution.width);
        int height = PlayerPrefs.GetInt("ScreenHeight", Screen.currentResolution.height);
        FullScreenMode savedMode = (FullScreenMode)PlayerPrefs.GetInt("FullscreenMode", (int)FullScreenMode.Windowed);

        Screen.SetResolution(width, height, savedMode);
    }
}