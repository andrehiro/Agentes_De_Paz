using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator transitionAnimator; // Asigna el Animator del Canvas
    public GameObject transitionCanvas; // Asigna el Canvas de la transición
    public float transitionTime = 1f; // Duración de la animación

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LoadSceneWithTransition(sceneName));
    }

    IEnumerator LoadSceneWithTransition(string sceneName)
    {
        // 1️⃣ Activa el Canvas y lanza la animación de cierre
        transitionCanvas.SetActive(true);
        transitionAnimator.SetTrigger("Cerrar");

        // 2️⃣ Espera el tiempo de la animación
        yield return new WaitForSeconds(transitionTime);

        // 3️⃣ Carga la nueva escena
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1f;

        // 4️⃣ Espera un frame para asegurarse de que la escena ha cargado
        yield return null;

        // 5️⃣ Lanza la animación de apertura
        transitionAnimator.SetTrigger("Abrir");

        // 6️⃣ Espera que la animación termine y desactiva el Canvas
        yield return new WaitForSeconds(transitionTime);
        transitionCanvas.SetActive(false);
    }
}