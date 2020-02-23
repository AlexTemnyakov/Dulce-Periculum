using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public List<AudioClip> songs;
    
    private AudioClip      currentSong = null;

    public AudioClip Song
    {
        get
        {
            if (songs.Count == 0)
                return currentSong;
            else
            {
                int       retIdx = Random.Range(0, songs.Count);
                AudioClip ret = songs[retIdx];

                if (currentSong)
                    songs.Add(currentSong);

                currentSong = ret;

                songs.RemoveAt(retIdx);

                return ret;
            }
        }
    }
}
