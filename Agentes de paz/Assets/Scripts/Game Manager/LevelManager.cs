using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public Button[] levelButtons;
    public string[] levelNames;

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
        string unlockedLevelName = PlayerPrefs.GetString("UnlockedLevels", "WindLevel1");
        int unlockedIndex = System.Array.IndexOf(levelNames, unlockedLevelName);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = i <= unlockedIndex;
        }
    }

    void Update()
    {
        // Presionar Espacio para reiniciar el progreso
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ResetProgress();
        }
    }

    void ResetProgress()
    {
        PlayerPrefs.DeleteAll(); // Elimina todo
        PlayerPrefs.SetString("UnlockedLevels", "WindLevel1"); // Restaura el primer nivel desbloqueado
        PlayerPrefs.Save();

        Debug.Log("Progreso reiniciado. Solo WindLevel1 está desbloqueado.");

        // Reiniciar los botones
        for (int i = 0; i < levelButtons.Length; i++)
        {
            string levelName = levelNames[i];

            if (levelName == "WindLevel1")
            {
                levelButtons[i].interactable = true;
            }
            else
            {
                levelButtons[i].interactable = false;
            }
        }
    }
}
