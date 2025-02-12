using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject settingsUI;
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
        
        pauseMenuUI.SetActive(true);
        
        EventSystem.current.SetSelectedGameObject(pauseMenuUI.GetComponentInChildren<Button>().gameObject);
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        isPaused = false;
        
        // Desactivar todos los menús y el panel
        pauseMenuUI.SetActive(false);
        settingsUI.SetActive(false);
    }

    public void OpenSettings()
    {
        pauseMenuUI.SetActive(false);
        settingsUI.SetActive(true);
        
        EventSystem.current.SetSelectedGameObject(settingsUI.GetComponentInChildren<Button>().gameObject);
    }

    public void CloseSettings()
    {

        settingsUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        
        EventSystem.current.SetSelectedGameObject(pauseMenuUI.GetComponentInChildren<Button>().gameObject);
    }

    public void CloseMenu()
    {
        Resume();
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); 
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