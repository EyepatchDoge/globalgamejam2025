using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public VolumeSettings musicVolumeSettings;
    public VolumeSettings SFXVolumeSettings;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        musicVolumeSettings.LoadVolume();
        musicVolumeSettings.SetMusicVolume();
        musicVolumeSettings.SetSFXVolume();

        SFXVolumeSettings.LoadVolume();
        SFXVolumeSettings.SetMusicVolume();
        SFXVolumeSettings.SetSFXVolume();
    }
}
