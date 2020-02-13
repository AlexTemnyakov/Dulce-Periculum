using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    public GameObject SHEATHED;
    public GameObject UNSHEATHED;
    public AudioClip  MOVING_SOUND;
    public AudioClip  SHEATHE_SOUND;
    public AudioClip  UNSHEATHE_SOUND;

    void Start()
    {
        SHEATHED.SetActive(true);
        UNSHEATHED.SetActive(false);
    }

    public void UnsheatheSword()
    {
        SHEATHED.SetActive(false);
        UNSHEATHED.SetActive(true);
        UNSHEATHED.GetComponent<AudioSource>().PlayOneShot(UNSHEATHE_SOUND);
    }

    public void SheatheSword()
    {
        SHEATHED.SetActive(true);
        UNSHEATHED.SetActive(false);
        SHEATHED.GetComponent<AudioSource>().PlayOneShot(SHEATHE_SOUND);
    }

    public void SwordSwingSound()
    {
        UNSHEATHED.GetComponent<AudioSource>().PlayOneShot(MOVING_SOUND);
    }

    public void SwordEnable()
    {
        hit = true;
    }

    public void SwordDisable()
    {
        hit = false;
    }
}
