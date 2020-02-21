using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public  float       MUSIC_DIST;

    private bool        enemyNearPlayer = false;
    private GameManager gameManager;
    private AudioSource audioSource;
    private float       maxVolume;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        audioSource = Camera.main.GetComponent<AudioSource>();
        maxVolume   = audioSource.volume;
    }

    void Update()
    {
        StartCoroutine(FindEnemiesNearPlayer());

        if (enemyNearPlayer)
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume, 0, Time.deltaTime);
        }
        else
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume, maxVolume, Time.deltaTime);
        }
    }

    private IEnumerator FindEnemiesNearPlayer()
    {
        if (!gameManager.Player || gameManager.Enemies.Count == 0)
        {
            enemyNearPlayer = false;
            yield break;
        }

        for (int i = 0; i < gameManager.Enemies.Count; i++)
        {
            if (!gameManager.Enemies[i] || !gameManager.Enemies[i].activeInHierarchy)
                continue;

            if (Vector3.Distance(gameManager.Player.transform.position, gameManager.Enemies[i].transform.position) < MUSIC_DIST)
            {
                enemyNearPlayer = true;
                break;
            }
            else
            {
                enemyNearPlayer = false;
            }

            yield return null;
        }
    }
}
