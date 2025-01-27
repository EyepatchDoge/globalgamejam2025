using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public VolumeSettings volumeSettings;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        volumeSettings.LoadVolume();
        volumeSettings.SetMusicVolume();
        volumeSettings.SetSFXVolume();

    }
}
