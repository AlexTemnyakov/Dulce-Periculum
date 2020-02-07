using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public  float       MUSIC_DIST;

    private bool        enemyNearPlayer = false;
    private AudioSource audioSource;
    private float       maxVolume;

    void Start()
    {
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
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Enemy") as GameObject[]; 
        GameObject   player  = GameObject.FindGameObjectWithTag("Player");

        for (int i = 0; i < objects.Length; i++)
        {
            if (Vector3.Distance(player.transform.position, objects[i].transform.position) < MUSIC_DIST)
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
