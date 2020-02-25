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

                songs.RemoveAt(retIdx);

                currentSong = ret;

                if (currentSong)
                    songs.Add(currentSong);

                return ret;
            }
        }
    }
}
