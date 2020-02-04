using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MaterialSound
{
    METAL,
    CREATURE
}

public class HitSound : MonoBehaviour
{
    //public  HitSoundHandler HIT_SOUND_HANDLER;
    public  MaterialSound THIS_MATERIAL;
    public  AudioClip     METAL_METAL;

    private AudioSource   audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public void PlayHitSound(MaterialSound other)
    {
        audioSource.PlayOneShot(METAL_METAL);
    }
}
