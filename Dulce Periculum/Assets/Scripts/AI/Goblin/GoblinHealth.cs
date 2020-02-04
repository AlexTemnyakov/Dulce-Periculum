using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinHealth : CreatureHealth
{
    void Update()
    {
        if (!IsAlive())
        {
            Animator a = GetComponent<Animator>();
            a.SetTrigger("Death");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (gameObject.layer == other.gameObject.layer)
            return;

        Damage d = other.GetComponent<Damage>();
        if (d)
        {
            HEALTH -= d.DAMAGE;
            other.GetComponent<HitSound>().PlayHitSound(MaterialSound.CREATURE);
            print("Hit, damage=" + d.DAMAGE + ", health=" + HEALTH);
        }
    }
}
