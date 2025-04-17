using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public Animator transitionAnimatorRight;
    public Animator transitionAnimatorLeft; 
    public GameObject transitionCanvas; 

    private void Start()
    {
        StartCoroutine(OpenAnimationTransition());
    }

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LoadSceneWithTransition(sceneName));
    }

    IEnumerator LoadSceneWithTransition(string sceneName)
    {
        Time.timeScale = 1f;
        transitionCanvas.SetActive(true);
        transitionAnimatorRight.SetBool("Closed", true);
        transitionAnimatorLeft.SetBool("Closed", true);

        yield return new WaitForSeconds(1f);

        SceneManager.LoadScene(sceneName);
        
    }

    IEnumerator OpenAnimationTransition()
    {
        transitionCanvas.SetActive(true);
        transitionAnimatorRight.SetBool("Closed", false);
        transitionAnimatorLeft.SetBool("Closed", false);

        yield return new WaitForSeconds(1f);

        transitionCanvas.SetActive(false);
    }
}