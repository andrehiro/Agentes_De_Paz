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
            instance = this;
        else 
            Destroy(gameObject);
    }

    void Start()
    {
        UpdateLevelButtons();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.DeleteKey("UnlockedLevels");
            PlayerPrefs.Save();
            Debug.Log("Progreso borrado.");
            UpdateLevelButtons();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            // Desbloquea todos los niveles guardando el ultimo
            PlayerPrefs.SetString("UnlockedLevels", levelNames[levelNames.Length - 1]);
            PlayerPrefs.Save();
            Debug.Log("Todos los niveles desbloqueados.");
            UpdateLevelButtons();
        }
    }

    void UpdateLevelButtons()
    {
        string unlockedLevelName = PlayerPrefs.GetString("UnlockedLevels", "WindLevel1");
        int unlockedIndex = System.Array.IndexOf(levelNames, unlockedLevelName);

        for (int i = 0; i < levelButtons.Length; i++)
            levelButtons[i].interactable = i <= unlockedIndex;
    }
}