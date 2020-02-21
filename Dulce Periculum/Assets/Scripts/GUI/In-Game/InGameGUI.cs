using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InGameGUI : MonoBehaviour
{
    public  GameObject  pauseMenuObject;
    public  GameObject  winMenuObject;
    public  GameObject  loseMenuObject;
    public  GameObject  playerHealthIndicator;

    private bool        paused                   = false;
    private GameManager gameManager              = null;
    private Image       playerHealthIndicatorImg = null;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        playerHealthIndicatorImg = playerHealthIndicator.GetComponent<Image>();

        pauseMenuObject.SetActive(false);
        winMenuObject.SetActive(false);
        loseMenuObject.SetActive(false);
        playerHealthIndicator.SetActive(true);
    }

    void Update()
    {
        HandlePlayerHealthIndicator();
    }

    private void HandlePlayerHealthIndicator()
    {
        float        tmpVal       = 0;
        Color        tmpColor     = playerHealthIndicatorImg.color;
        PlayerHealth playerHealth = gameManager.Player ? gameManager.Player.GetComponentInParent<PlayerHealth>() : null;

        if (playerHealth)
        {
            tmpVal = Mathf.Lerp(tmpColor.a, 1 - (playerHealth.CurrentHealth / playerHealth.MaximumHealth), Time.deltaTime);
        }

        tmpColor.a = tmpVal;
        playerHealthIndicatorImg.color = tmpColor;
    }

    public void PauseGame()
    {
        if (paused)
            return;

        Time.timeScale = 0f;
        paused         = true;

        playerHealthIndicator.SetActive(false);
    }

    public void ResumeGame()
    {
        if (!paused)
            return;

        Time.timeScale = 1f;
        paused         = false;

        playerHealthIndicator.SetActive(true);
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
