using UnityEngine;

public class GameManager : MonoBehaviour
{

    #region variables

    public GameObject pausePanel;
    public GameObject controlsTab;
    public VolumeSettings volumeSettings;
    public bool seenControlPanel;


    #endregion
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        volumeSettings.LoadVolume();
        volumeSettings.SetMusicVolume();
        volumeSettings.SetSFXVolume();


        controlsTab.SetActive(true);

        Time.timeScale = 0;
        CursorOn();

        seenControlPanel = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && seenControlPanel== true)
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
        seenControlPanel = true;
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

    public void PauseGame()
    {
        Time.timeScale = 0;
    }
}
