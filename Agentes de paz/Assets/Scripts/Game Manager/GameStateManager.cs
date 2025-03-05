using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;
    public Toggle autoModeToggle;

    private const string ToggleKey = "AutoModeToggleState"; 

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadToggleState();
    }

    // Método para guardar el estado del Toggle
    public void SaveToggleState()
    {
        bool isAutoModeOn = autoModeToggle.isOn; 
        PlayerPrefs.SetInt(ToggleKey, isAutoModeOn ? 1 : 0); 
        PlayerPrefs.Save();
    }

    // Método para cargar el estado del Toggle
    void LoadToggleState()
    {
        if (PlayerPrefs.HasKey(ToggleKey))
        {
            int savedState = PlayerPrefs.GetInt(ToggleKey); 
            autoModeToggle.isOn = savedState == 1; 
        }
        else
        {
            autoModeToggle.isOn = false;
        }
    }
}