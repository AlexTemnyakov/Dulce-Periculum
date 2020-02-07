using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitSound : HitSound
{
    public AudioClip METAL_METAL;
    public AudioClip METAL_CREATURE;

    override public void PlayHitSound(MaterialSound other)
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
    }
}
