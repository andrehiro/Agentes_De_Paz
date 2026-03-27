using UnityEngine;
using TMPro;
using System.Collections;

public class ResolutionController : MonoBehaviour
{
    [Header("Dropdowns")]
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown displayModeDropdown;

    [Header("Resoluciones disponibles")]
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

    [Header("Camara")]
    public float baseOrthographicSize = 5f;
    public float baseAspect = 16f / 9f;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;

        PopulateResolutionDropdown();
        PopulateDisplayModeDropdown();

        LoadSavedResolution();
        LoadSavedDisplayMode();
    }

    void AdjustCamera(int width, int height)
    {
        float currentAspect = (float)width / height;

        if (currentAspect >= baseAspect)
            mainCamera.orthographicSize = baseOrthographicSize;
        else
            mainCamera.orthographicSize = baseOrthographicSize * (baseAspect / currentAspect);
    }

    // ------------------------------
    //  RESOLUCIONES
    // ------------------------------
    void PopulateResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();

        foreach (Vector2Int res in resolutions)
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData($"{res.x} x {res.y}"));

        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        int savedIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        resolutionDropdown.value = savedIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutionIndex < 0 || resolutionIndex >= resolutions.Length) return;

        Vector2Int newRes = resolutions[resolutionIndex];
        bool isFullscreen = Screen.fullScreenMode == FullScreenMode.FullScreenWindow;

        Screen.SetResolution(newRes.x, newRes.y, isFullscreen);
        AdjustCamera(newRes.x, newRes.y);

        FindAnyObjectByType<MapScaler>()?.AdjustMap();

        foreach (ColliderScaler cs in FindObjectsByType<ColliderScaler>())
            cs.AdjustToResolution();

        PlayerPrefs.SetInt("ScreenWidth", newRes.x);
        PlayerPrefs.SetInt("ScreenHeight", newRes.y);
        PlayerPrefs.SetInt("ResolutionIndex", resolutionIndex);
        PlayerPrefs.Save();
    }

    // ------------------------------
    //  MODO DE PANTALLA
    // ------------------------------
    void PopulateDisplayModeDropdown()
    {
        displayModeDropdown.ClearOptions();
        displayModeDropdown.options.Add(new TMP_Dropdown.OptionData("Pantalla Completa"));
        displayModeDropdown.options.Add(new TMP_Dropdown.OptionData("Modo Ventana"));
        displayModeDropdown.onValueChanged.AddListener(SetDisplayMode);
    }

    public void SetDisplayMode(int index)
    {
        switch (index)
        {
            case 0: SetFullscreenMode(); break;
            case 1: SetWindowedMode(); break;
        }
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

        int width = PlayerPrefs.GetInt("ScreenWidth", 1280);
        int height = PlayerPrefs.GetInt("ScreenHeight", 720);
        Screen.SetResolution(width, height, false);
    }

    // ------------------------------
    //  CARGAR CONFIGURACIONES
    // ------------------------------
    private void LoadSavedResolution()
    {
        int width = PlayerPrefs.GetInt("ScreenWidth", Screen.currentResolution.width);
        int height = PlayerPrefs.GetInt("ScreenHeight", Screen.currentResolution.height);
        FullScreenMode savedMode = (FullScreenMode)PlayerPrefs.GetInt("FullscreenMode", (int)FullScreenMode.Windowed);

        Screen.SetResolution(width, height, savedMode);
        AdjustCamera(width, height);
        StartCoroutine(AdjustAfterFrame());
    }

    private IEnumerator AdjustAfterFrame()
    {
        yield return null;
        
        FindAnyObjectByType<MapScaler>()?.AdjustMap();

        foreach (ColliderScaler cs in FindObjectsByType<ColliderScaler>())
            cs.AdjustToResolution();
    }

    private void LoadSavedDisplayMode()
    {
        FullScreenMode savedMode = (FullScreenMode)PlayerPrefs.GetInt("FullscreenMode", (int)FullScreenMode.Windowed);
        displayModeDropdown.value = savedMode == FullScreenMode.FullScreenWindow ? 0 : 1;
        displayModeDropdown.RefreshShownValue();
    }
}