using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuMusicManager : MonoBehaviour
{
    private Music       music;
    private AudioSource source;

    void Start()
    {
        music       = GetComponent<Music>();
        source      = GetComponent<AudioSource>();
        source.loop = false;
    }

    void Update()
    {
        if (!source.isPlaying)
        {
            source.clip = music.Song;
            source.Play();
        }
    }
}
