using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject settingsUI;
    public GameObject blockerPanel;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                if (settingsUI.activeSelf)
                {
                    CloseSettings();
                }
                else
                {
                    Resume();
                }
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        isPaused = true;
        
        // Activar menú y panel bloqueador
        pauseMenuUI.SetActive(true);
        blockerPanel.SetActive(true);
        
        // Asegurar que el EventSystem funcione correctamente
        EventSystem.current.SetSelectedGameObject(pauseMenuUI.GetComponentInChildren<Button>().gameObject);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;
        
        // Desactivar todos los menús y el panel
        pauseMenuUI.SetActive(false);
        settingsUI.SetActive(false);
        blockerPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        // Ocultar menú de pausa y mostrar configuración
        pauseMenuUI.SetActive(false);
        settingsUI.SetActive(true);
        
        // Enfocar el primer botón de la configuración
        EventSystem.current.SetSelectedGameObject(settingsUI.GetComponentInChildren<Button>().gameObject);
    }

    public void CloseSettings()
    {
        // Ocultar configuración y mostrar menú de pausa
        settingsUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        
        // Re-enfocar el botón del menú de pausa
        EventSystem.current.SetSelectedGameObject(pauseMenuUI.GetComponentInChildren<Button>().gameObject);
    }

    public void CloseMenu()
    {
        Resume();
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;  // Importante reestablecer el tiempo
        SceneManager.LoadScene("MainMenu");  // Nombre de tu escena de menú principal
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo del juego...");
        Application.Quit();
        
        // En el Editor de Unity esto no funciona, solo en builds
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}