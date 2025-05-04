using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public Slider backgorundMusicVolumeSlider;

    void Start()
    {
        backgorundMusicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.2f);
        SetVolume();
    }

    public void SetVolume()
    {
        if (backgroundMusic != null)
        {
            backgroundMusic.volume = backgorundMusicVolumeSlider.value;
        }
        
        PlayerPrefs.SetFloat("MusicVolume", backgorundMusicVolumeSlider.value);
        PlayerPrefs.Save();
    }
}
