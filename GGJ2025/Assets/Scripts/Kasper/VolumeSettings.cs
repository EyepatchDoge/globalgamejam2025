using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer musicMixer;
    [SerializeField] private AudioMixer SFXMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;

    private void Start()
    {
        // Check if first-time launch and initialize defaults
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 0.5f);
        }
        if (!PlayerPrefs.HasKey("SFXVolume"))
        {
            PlayerPrefs.SetFloat("SFXVolume", 0.5f);
        }

        // Ensure PlayerPrefs are saved
        PlayerPrefs.Save();

        // Load and apply saved volumes
        LoadVolume();
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        musicMixer.SetFloat("music", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20); // Prevent log(0)
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume()
    {
        float volume = SFXSlider.value;
        SFXMixer.SetFloat("sfx", Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
        PlayerPrefs.Save();
    }

    public void LoadVolume()
    {
        // Get saved values
        float musicVolume = PlayerPrefs.GetFloat("musicVolume");
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume");

        // Update UI sliders
        musicSlider.value = musicVolume;
        SFXSlider.value = sfxVolume;

        // Apply values to AudioMixers
        musicMixer.SetFloat("music", Mathf.Log10(Mathf.Max(musicVolume, 0.0001f)) * 20);
        SFXMixer.SetFloat("sfx", Mathf.Log10(Mathf.Max(sfxVolume, 0.0001f)) * 20);
    }
}
