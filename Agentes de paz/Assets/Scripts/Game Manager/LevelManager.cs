using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Button[] levelButtons;
    public string[] levelNames;

    void Start()
    {
        string unlockedLevelName = PlayerPrefs.GetString("UnlockedLevels", "WindLevel1"); 

        // Iterar a través de todos los botones de niveles
        for (int i = 0; i < levelButtons.Length; i++)
        {
            string levelName = levelNames[i];

            // Si el nivel actual es el siguiente a desbloquear o anterior
            if (string.Compare(levelName, unlockedLevelName) > 0)
            {
                levelButtons[i].interactable = false;
            }
            else
            {
                levelButtons[i].interactable = true;
            }
        }
    }
}