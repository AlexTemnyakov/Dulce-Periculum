using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameGUI : MonoBehaviour
{
    public GameObject pauseMenuObject;
    public GameObject winMenuObject;
    public GameObject loseMenuObject;

    private bool paused = false;

    void Start()
    {
        pauseMenuObject.SetActive(false);
        winMenuObject.SetActive(false);
        loseMenuObject.SetActive(false);
    }

    void Update()
    {

    }

    public void PauseGame()
    {
        if (paused)
            return;

        Time.timeScale = 0f;
        paused         = true;
    }

    public void ResumeGame()
    {
        if (!paused)
            return;

        Time.timeScale = 1f;
        paused         = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToPauseMenu()
    {
        PauseGame();
        pauseMenuObject.SetActive(true);
    }

    public void LeavePauseMenu()
    {
        ResumeGame();
        pauseMenuObject.SetActive(false);
    }

    public void GoToWinMenu()
    {
        PauseGame();
        winMenuObject.SetActive(true);
    }

    public void GoToLoseMenu()
    {
        PauseGame();
        loseMenuObject.SetActive(true);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
