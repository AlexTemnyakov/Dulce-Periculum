using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public  float       MUSIC_DIST;

    private bool        enemyNearPlayer = false;
    private bool        villageNearPlayer = false;
    private GameManager gameManager;
    private AudioSource source;
    private Music       music;
    private float       maxVolume;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        source      = Camera.main.GetComponent<AudioSource>();
        source.loop = false;
        music       = GetComponent<Music>();
        maxVolume   = source.volume;
    }

    void Update()
    {
        if (!source.isPlaying)
        {
            source.clip = music.Song;
            source.Play();
        }

        StartCoroutine(FindEnemiesNearPlayer());

        if (enemyNearPlayer)
        {
            source.volume = Mathf.Lerp(source.volume, 0, Time.deltaTime);
        }
        else
        {
            source.volume = Mathf.Lerp(source.volume, maxVolume, Time.deltaTime);
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
                yield break; 
            }

            yield return null;
        }

        enemyNearPlayer = false;
    }
}
