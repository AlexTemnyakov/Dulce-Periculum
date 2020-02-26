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
    private float          fireStart;
    private float          fireCooldownStart;

    void Start()
    {
        Sheathed.SetActive(true);
        Unsheathed.SetActive(false);

        damageDefault = Damage;
        particle      = Unsheathed.GetComponent<ParticleSystem>();

        fireStart         = 0;
        fireCooldownStart = 0;

        ParticleSystem.EmissionModule em;
        em         = particle.emission;
        em.enabled = false;
    }

    void Update()
    {
        if (fireStart > 0)
            fireStart -= Time.deltaTime;
        else
            SwitchFireOff();

        if (fireCooldownStart > 0)
            fireCooldownStart -= Time.deltaTime;
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
        if (fireCooldownStart <= 0)
        {
            ParticleSystem.EmissionModule em;
            em                = particle.emission;
            em.enabled        = true;
            Damage           *= 2;
            fireStart         = FireOnMaxTime;
            fireCooldownStart = FireOnCooldown;
        }
    }

    public void SwitchFireOff()
    {
        ParticleSystem.EmissionModule em;
        em                = particle.emission;
        em.enabled        = false;
        Damage            = damageDefault;
        fireStart         = 0;
    }

    public bool isFireOn
    {
        get
        {
            return particle.emission.enabled;
        }
    }
}
