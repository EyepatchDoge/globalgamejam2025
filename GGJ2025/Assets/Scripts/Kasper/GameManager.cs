using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region variables

    public GameObject pausePanel;
    public GameObject controlsTab;
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

        controlsTab.SetActive(true);

        Time.timeScale = 0;
        CursorOn();
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

    public void StartTheGame()
    {
        controlsTab.SetActive(false);
        CursorOff();
        ResumeGame();
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
