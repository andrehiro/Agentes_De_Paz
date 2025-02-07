using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;       // Canvas del menú de pausa
    public GameObject blockerPanel;      // Panel transparente que bloquea interacciones
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
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
        
        // Asegurar que el EventSystem solo interactúe con el menú
        EventSystem.current.SetSelectedGameObject(pauseMenuUI.GetComponentInChildren<Button>().gameObject);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;
        
        // Desactivar menú y panel bloqueador
        pauseMenuUI.SetActive(false);
        blockerPanel.SetActive(false);
    }

    public void OpenSettings()
    {
        Debug.Log("Abrir configuración");
        // Aquí cargarías tu menú de configuración
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