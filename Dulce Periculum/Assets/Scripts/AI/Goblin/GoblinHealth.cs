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

    override public void OnTriggerEnter(Collider other)
    {
        if (gameObject.layer == other.gameObject.layer)
            return;

        Weapon w = other.GetComponent<Weapon>();
        if (w && !w.hit)
        {
            HEALTH -= w.DAMAGE;
            w.hit   = true;
            other.GetComponent<HitSound>().PlayHitSound(THIS_MATERIAL);
            print("Hit, damage=" + w.DAMAGE + ", health=" + HEALTH);
        }
    }
}
