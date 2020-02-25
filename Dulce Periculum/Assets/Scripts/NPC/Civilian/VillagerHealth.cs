using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagerHealth : CreatureHealth
{
    void Start()
    {
        Settings s    = GameObject.FindGameObjectWithTag("Settings").GetComponent<Settings>();
        maxHealth     = s.VILLAGER_HEALTH;
        currentHealth = maxHealth;
    }

    override public void OnTriggerEnter(Collider other)
    {
        if (gameObject.layer == other.gameObject.layer
            || other.gameObject.layer == LayerMask.NameToLayer("Player"))
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
