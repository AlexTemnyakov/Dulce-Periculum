using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuObject;

    private bool paused = false;

    void Start()
    {
        pauseMenuObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            PauseGame();
    }

    public void PauseGame()
    {
        if (paused)
            return;

        pauseMenuObject.SetActive(true);
        Time.timeScale = 0f;
        paused         = true;
    }

    public void ResumeGame()
    {
        if (!paused)
            return;

        pauseMenuObject.SetActive(false);
        Time.timeScale = 1f;
        paused         = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
