using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureHealth : MonoBehaviour
{
    public MaterialSound THIS_MATERIAL;
    public float         HEALTH;

    void Start()
    {
        
    }

    void Update()
    {
        if (!IsAlive())
            Die();
    }

    virtual public void OnTriggerEnter(Collider other)
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

    public bool IsAlive()
    {
        return HEALTH > 0;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
