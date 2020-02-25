using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    public GameObject Sheathed;
    public GameObject Unsheathed;

    public AudioClip  MovingSound;
    public AudioClip  SheatheSound;
    public AudioClip  UnsheatheSound;

    public float      FireOnMaxTime;
    public float      FireOnCooldown;

    private int            damageDefault;
    private ParticleSystem particle;
    private bool           fireOn        = false;
    private float          fireStart     = float.MaxValue;

    void Start()
    {
        Sheathed.SetActive(true);
        Unsheathed.SetActive(false);

        damageDefault = Damage;
        particle      = Unsheathed.GetComponent<ParticleSystem>();

        ParticleSystem.EmissionModule em;
        em         = particle.emission;
        em.enabled = false;
    }

    void Update()
    {
        if (fireOn)
        {
            fireStart += Time.deltaTime;

            if (fireStart > FireOnMaxTime)
            {
                SwitchFireOff();
            }
        }
    }

    public void UnsheatheSword()
    {
        Sheathed.SetActive(false);
        Unsheathed.SetActive(true);
        Unsheathed.GetComponent<AudioSource>().PlayOneShot(UnsheatheSound);
    }

    public void SheatheSword()
    {
        Sheathed.SetActive(true);
        Unsheathed.SetActive(false);
        Sheathed.GetComponent<AudioSource>().PlayOneShot(SheatheSound);
    }

    public void SwordSwingSound()
    {
        Unsheathed.GetComponent<AudioSource>().PlayOneShot(MovingSound);
    }

    public void SwordEnable()
    {
        hit = true;
    }

    public void SwordDisable()
    {
        hit = false;
    }

    public void SwitchFireOn()
    {
        ParticleSystem.EmissionModule em;
        em         = Particle.emission;
        em.enabled = true;
        Damage    *= 2;
        fireOn     = true;
        fireStart  = 0;
    }

    public void SwitchFireOff()
    {
        ParticleSystem.EmissionModule em;
        em         = Particle.emission;
        em.enabled = false;
        Damage     = damageDefault;
        fireOn     = false;
        fireStart  = float.MaxValue;
    }

    public ParticleSystem Particle
    {
        get
        {
            return particle;
        }
    }
}
