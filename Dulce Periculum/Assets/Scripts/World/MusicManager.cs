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
        GameObject player  = GameObject.FindGameObjectWithTag("Player");

        foreach (GameObject o in gameManager.Enemies)
        {
            if (!o || !o.activeInHierarchy)
                continue;

            if (Vector3.Distance(player.transform.position, o.transform.position) < MUSIC_DIST)
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
