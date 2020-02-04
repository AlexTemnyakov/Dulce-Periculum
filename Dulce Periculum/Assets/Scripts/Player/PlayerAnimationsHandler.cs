using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsHandler : MonoBehaviour
{
    public GameObject SWORD_UNSHEATHED;
    public GameObject SWORD_SHEATHED;

    void Start()
    {
        SheathSword();
    }

    public void UnsheathSword()
    {
        SWORD_SHEATHED.SetActive(false);
        SWORD_UNSHEATHED.SetActive(true);
    }

    public void SheathSword()
    {
        SWORD_SHEATHED.SetActive(true);
        SWORD_UNSHEATHED.SetActive(false);
    }

    public void WeaponHitSound()
    {
        if (SWORD_UNSHEATHED.activeInHierarchy)
        {
            SWORD_UNSHEATHED.GetComponent<AudioSource>().Play();
        }
    }
}
