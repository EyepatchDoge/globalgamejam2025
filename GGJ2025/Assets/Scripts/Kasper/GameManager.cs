using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region variables

    public GameObject pausePanel;
    public VolumeSettings musicVolumeSettings;
    public VolumeSettings SFXVolumeSettings;

    #endregion
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            CursorOn();
            pausePanel.SetActive(true);

        } 
    }


    public void CursorOn()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void CursorOff() 
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
