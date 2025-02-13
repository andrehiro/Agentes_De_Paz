using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;
    public Toggle autoModeToggle;

    private const string ToggleKey = "AutoModeToggleState"; // Clave para guardar el estado

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
        bool isAutoModeOn = autoModeToggle.isOn; // Obtener el estado actual del Toggle
        PlayerPrefs.SetInt(ToggleKey, isAutoModeOn ? 1 : 0); // Guardar como 1 si está ON, 0 si está OFF
        PlayerPrefs.Save(); // Asegurarse de que los cambios se guarden
    }

    // Método para cargar el estado del Toggle
    void LoadToggleState()
    {
        if (PlayerPrefs.HasKey(ToggleKey)) // Verificar si existe una clave guardada
        {
            int savedState = PlayerPrefs.GetInt(ToggleKey); // Obtener el valor guardado
            autoModeToggle.isOn = savedState == 1; // Si es 1, activar el Toggle, si es 0, desactivarlo
        }
        else
        {
            autoModeToggle.isOn = false; // Predeterminado: OFF
        }
    }
}