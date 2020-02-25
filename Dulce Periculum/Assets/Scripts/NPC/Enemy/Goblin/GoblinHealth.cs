using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinHealth : CreatureHealth
{
    void Start()
    {
        Settings s    = GameObject.FindGameObjectWithTag("Settings").GetComponent<Settings>();
        maxHealth     = s.GOBLIN_HEALTH;
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!IsAlive)
        {
            Animator a = GetComponent<Animator>();
            a.SetTrigger("Death");
        }
    }

    override public void OnTriggerEnter(Collider other)
    {
        if (gameObject.layer == other.gameObject.layer)
            return;

        Weapon w = other.GetComponent<Weapon>();
        if (w && w.hit)
        {
            currentHealth -= w.Damage;
            w.hit          = false;
            other.GetComponent<HitSound>().PlayHitSound(THIS_MATERIAL);
            print("Hit, damage=" + w.Damage + ", health=" + currentHealth);
        }
    }
}
