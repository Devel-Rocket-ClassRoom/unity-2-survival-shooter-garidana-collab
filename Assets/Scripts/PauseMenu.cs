using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("Pause Menu")]
    public GameObject pauseMenu;
    public Slider musicVolumeSlider;
    public Slider effectVolumeSlider;
    public Toggle muteToggle;

    [Header("Data")]
    public AudioMixer audioMixer;
    private bool isPaused = false;

    const string k_PrefsMusicVolumeKey = "MusicVolume";
    const string k_PrefsEffectVolumeKey = "EffectVolume";
    const string k_PrefsMuteKey = "Mute";

    const string k_AudioMixerMusicParameter = "MusicVolume";
    const string k_AudioMixerEffectParameter = "EffectVolume";

    public void Start()
    {
        musicVolumeSlider.value = PlayerPrefs.GetFloat(k_PrefsMusicVolumeKey, 1f);
        effectVolumeSlider.value = PlayerPrefs.GetFloat(k_PrefsEffectVolumeKey, 1f);
        muteToggle.isOn = PlayerPrefs.GetInt(k_PrefsMuteKey, 1) == 1;
        ApplyVolume();
    }

    public void Pause()
    {
        Time.timeScale = 0f;

        musicVolumeSlider.value = PlayerPrefs.GetFloat(k_PrefsMusicVolumeKey, 1f);
        effectVolumeSlider.value = PlayerPrefs.GetFloat(k_PrefsEffectVolumeKey, 1f);
        muteToggle.isOn = PlayerPrefs.GetInt(k_PrefsMuteKey, 1) == 1;

        pauseMenu.SetActive(true);
        isPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1f;

        PlayerPrefs.SetFloat(k_PrefsMusicVolumeKey, musicVolumeSlider.value);
        PlayerPrefs.SetFloat(k_PrefsEffectVolumeKey, effectVolumeSlider.value);
        PlayerPrefs.SetInt(k_PrefsMuteKey, muteToggle.isOn ? 1 : 0);

        pauseMenu.SetActive(false);
        isPaused = false;
    }

    public void TogglePause()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnMusicVolumeSliderChanged (float value)
    {
        ApplyVolume();
    }

    public void OnEffectVolumeSliderChanged (float value)
    {
        ApplyVolume();
    }

    public void OnMuteToggleChanged (bool soundOn)
    {
        ApplyVolume();
    }

    public void ApplyVolume()
    {
        if (muteToggle.isOn)
        {
            float dBValue = Mathf.Log10(Mathf.Max(effectVolumeSlider.value, 0.0001f)) * 20;
            audioMixer.SetFloat(k_AudioMixerEffectParameter, dBValue);

            dBValue = Mathf.Log10(Mathf.Max(musicVolumeSlider.value, 0.0001f)) * 20;
            audioMixer.SetFloat(k_AudioMixerMusicParameter, dBValue);
        }
        else
        {
            audioMixer.SetFloat(k_AudioMixerEffectParameter, -80f);
            audioMixer.SetFloat(k_AudioMixerMusicParameter, -80f);
        }
    }
}
