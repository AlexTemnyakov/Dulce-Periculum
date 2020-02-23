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

    public IEnumerator ShowWinMenu()
    {
        Image img      = winMenuObject.GetComponent<Image>();
        Text  text     = winMenuObject.GetComponentInChildren<Text>();
        Color tmp;
        float alphaMax = img.color.a;
        float time;

        tmp       = img.color;
        tmp.a     = 0;
        img.color = tmp;

        tmp        = text.color;
        tmp.a      = 0;
        text.color = tmp;

        winMenuObject.SetActive(true);

        time = 0;
        while (time < 1)
        {
            tmp       = img.color;
            tmp.a     = Mathf.Lerp(0, alphaMax, time);
            img.color = tmp;

            tmp        = text.color;
            tmp.a      = Mathf.Lerp(0, 1, time);
            text.color = tmp;

            time += Time.deltaTime / 5;

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(5);

        time = 0;
        while (time < 1)
        {
            tmp       = img.color;
            tmp.a     = Mathf.Lerp(alphaMax, 0, time);
            img.color = tmp;

            tmp        = text.color;
            tmp.a      = Mathf.Lerp(1, 0, time);
            text.color = tmp;

            time += Time.deltaTime / 5;

            yield return new WaitForEndOfFrame();
        }

        winMenuObject.SetActive(false);

        tmp       = img.color;
        tmp.a     = alphaMax;
        img.color = tmp;

        tmp        = text.color;
        tmp.a      = 1;
        text.color = tmp;
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
