using UnityEngine;
using UnityEngine.SceneManagement; // Importar SceneManager

public class ChangeScene : MonoBehaviour
{
    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;
    }
}