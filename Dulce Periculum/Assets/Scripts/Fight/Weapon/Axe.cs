using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Weapon
{
    public GameObject AXE;
    public AudioClip  SWING_SOUND;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AxeSwingSound()
    {
        AXE.GetComponent<AudioSource>().PlayOneShot(SWING_SOUND);
    }

    public void AxeEnable()
    {
        hit = true;
    }

    public void AxeDisable()
    {
        hit = false;
    }
}
