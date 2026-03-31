using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class VolumeControl : MonoBehaviour
{
    public static VolumeControl instance;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    [Header("Music")]
    public AudioSource musicSource;

    [Header("SFX")]
    public AudioClip clickSound;
    [Range(0f, 1f)] public float clickVolume = 1f;
    private AudioSource sfxSource;

    [Header("Sliders")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("Value Labels")]
    public TextMeshProUGUI masterValueText;
    public TextMeshProUGUI musicValueText;
    public TextMeshProUGUI sfxValueText;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.playOnAwake = false;

        AudioMixerGroup[] sfxGroup = audioMixer.FindMatchingGroups("SFX");
        if (sfxGroup.Length > 0)
            sfxSource.outputAudioMixerGroup = sfxGroup[0];

        AudioMixerGroup[] musicGroup = audioMixer.FindMatchingGroups("Music");
        if (musicGroup.Length > 0 && musicSource != null)
            musicSource.outputAudioMixerGroup = musicGroup[0];
    }

    void Start()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        musicVolumeSlider.value  = PlayerPrefs.GetFloat("MusicVolume", 0.2f);
        sfxVolumeSlider.value    = PlayerPrefs.GetFloat("SFXVolume", 1f);

        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
        UpdateLabels();

        masterVolumeSlider.onValueChanged.AddListener(_ => SetMasterVolume());
        musicVolumeSlider.onValueChanged.AddListener(_  => SetMusicVolume());
        sfxVolumeSlider.onValueChanged.AddListener(_    => SetSFXVolume());
    }

    void UpdateLabels()
    {
        if (masterValueText) masterValueText.text = Mathf.RoundToInt(masterVolumeSlider.value * 100).ToString();
        if (musicValueText)  musicValueText.text  = Mathf.RoundToInt(musicVolumeSlider.value * 100).ToString();
        if (sfxValueText)    sfxValueText.text    = Mathf.RoundToInt(sfxVolumeSlider.value * 100).ToString();
    }

    float ToDecibels(float value) =>
        value > 0.0001f ? Mathf.Log10(value) * 20f : -80f;

    public void SetMasterVolume()
    {
        audioMixer.SetFloat("MasterVolume", ToDecibels(masterVolumeSlider.value));
        PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
        PlayerPrefs.Save();
        if (masterValueText) masterValueText.text = Mathf.RoundToInt(masterVolumeSlider.value * 100).ToString();
    }

    public void SetMusicVolume()
    {
        audioMixer.SetFloat("MusicVolume", ToDecibels(musicVolumeSlider.value));
        PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        PlayerPrefs.Save();
        if (musicValueText) musicValueText.text = Mathf.RoundToInt(musicVolumeSlider.value * 100).ToString();
    }

    public void SetSFXVolume()
    {
        audioMixer.SetFloat("SFXVolume", ToDecibels(sfxVolumeSlider.value));
        PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.Save();
        if (sfxValueText) sfxValueText.text = Mathf.RoundToInt(sfxVolumeSlider.value * 100).ToString();
    }

    public void PlayClick()
    {
        if (clickSound != null)
            sfxSource.PlayOneShot(clickSound, clickVolume);
    }
}