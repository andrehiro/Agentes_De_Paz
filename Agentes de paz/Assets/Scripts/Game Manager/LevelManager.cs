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
        UpdateLevelButtons();
    }

    void Update()
    {
        // Si se presiona la tecla P, borra el progreso
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerPrefs.DeleteKey("UnlockedLevels");
            PlayerPrefs.Save();
            Debug.Log("Progreso borrado.");
            UpdateLevelButtons();
        }
    }

    void UpdateLevelButtons()
    {
        string unlockedLevelName = PlayerPrefs.GetString("UnlockedLevels", "WindLevel1");
        int unlockedIndex = System.Array.IndexOf(levelNames, unlockedLevelName);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].interactable = i <= unlockedIndex;
        }
    }
}
