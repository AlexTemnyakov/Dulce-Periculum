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
    public  MaterialSound THIS_MATERIAL;
    public  AudioClip     METAL_METAL;
    public  AudioClip     METAL_CREATURE;

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
        switch (THIS_MATERIAL)
        {
            case MaterialSound.METAL:
            {
                switch (other)
                {
                    case MaterialSound.METAL:
                        audioSource.PlayOneShot(METAL_METAL);
                        break;
                    case MaterialSound.CREATURE:
                        audioSource.PlayOneShot(METAL_CREATURE);
                        break;
                }
            } break;

            case MaterialSound.CREATURE:
            {
                switch (other)
                {
                    case MaterialSound.METAL:
                        audioSource.PlayOneShot(METAL_CREATURE);
                        break;
                    case MaterialSound.CREATURE:
                        audioSource.PlayOneShot(METAL_METAL);
                        break;
                }
            } break;
        }
    }
}
