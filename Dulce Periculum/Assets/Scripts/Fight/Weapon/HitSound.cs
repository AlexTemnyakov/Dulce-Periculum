using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Weapon))]
public class HitSound : MonoBehaviour
{
    protected AudioSource audioSource;

    void Start()
    {  
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    virtual public void PlayHitSound(MaterialSound other)
    {

    }
}
