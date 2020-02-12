using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureHealth : MonoBehaviour
{
    public    MaterialSound THIS_MATERIAL;

    protected float         maxHealth;
    protected float         currentHealth;

    void Start()
    {
        maxHealth = currentHealth = 0;
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
            currentHealth -= w.DAMAGE;
            w.hit   = true;
            other.GetComponent<HitSound>().PlayHitSound(THIS_MATERIAL);
            print("Hit, damage=" + w.DAMAGE + ", health=" + currentHealth);
        }
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
