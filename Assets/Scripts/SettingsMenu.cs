using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [Header("Panel")]
    public GameObject settingsPanel;
    public GameObject pausePanel;

    [Header("Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider ambienceSlider;

    [Header("Back Button")]
    public Button backButton;
    public Button openSettingsButton;

    

    void Start()
    {
        settingsPanel.SetActive(false);

        if (backButton != null)
            backButton.onClick.AddListener(CloseSettings);

        if (openSettingsButton != null)
            openSettingsButton.onClick.AddListener(OpenSettings);

        // Set sliders to match current AudioManager values
        if (AudioManager.instance != null)
        {
            musicSlider.value = AudioManager.instance.musicVolume;
            sfxSlider.value = AudioManager.instance.sfxVolume;
            ambienceSlider.value = AudioManager.instance.ambienceVolume;
        }

        // Listen for slider changes
        musicSlider.onValueChanged.AddListener(OnMusicChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXChanged);
        ambienceSlider.onValueChanged.AddListener(OnAmbienceChanged);
    }

    public void OpenSettings()
    {
        pausePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        pausePanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    void OnMusicChanged(float value)
    {
        if (AudioManager.instance == null) return;
        AudioManager.instance.musicVolume = value;
        AudioManager.instance.musicSource.volume = value;
    }

    void OnSFXChanged(float value)
    {
        if (AudioManager.instance == null) return;
        AudioManager.instance.sfxVolume = value;
        AudioManager.instance.coinSource.volume = value * 0.4f;
    }

    void OnAmbienceChanged(float value)
    {
        if (AudioManager.instance == null) return;
        AudioManager.instance.ambienceVolume = value;
        AudioManager.instance.windSource.volume = value;
        AudioManager.instance.birdsSource.volume = value;
    }
}