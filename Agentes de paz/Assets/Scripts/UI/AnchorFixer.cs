using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AnchorFixer : MonoBehaviour
{
    void Awake()
    {
        RectTransform canvasRect = GetComponent<RectTransform>();
        string sceneName = SceneManager.GetActiveScene().name;

        foreach (Button button in GetComponentsInChildren<Button>())
        {
            RectTransform child = button.GetComponent<RectTransform>();
            string key = sceneName + "_" + button.gameObject.name;

            if (Screen.width == 1920 && Screen.height == 1080)
            {
                // Guarda anchorMin correcto en 1920x1080
                Vector2 anchorMin = new Vector2(
                    child.anchorMin.x + child.anchoredPosition.x / canvasRect.rect.width,
                    child.anchorMin.y + child.anchoredPosition.y / canvasRect.rect.height
                );

                PlayerPrefs.SetFloat(key + "_ax", anchorMin.x);
                PlayerPrefs.SetFloat(key + "_ay", anchorMin.y);
                PlayerPrefs.Save();

                child.anchorMin = anchorMin;
                child.anchorMax = anchorMin;
                child.anchoredPosition = Vector2.zero;
            }
            else if (PlayerPrefs.HasKey(key + "_ax"))
            {
                Vector2 anchorMin = new Vector2(
                    PlayerPrefs.GetFloat(key + "_ax"),
                    PlayerPrefs.GetFloat(key + "_ay")
                );

                child.anchorMin = anchorMin;
                child.anchorMax = anchorMin;
                child.anchoredPosition = Vector2.zero;
            }
        }
    }
}