using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : CreatureHealth
{
    public float HILL_START_TIME;
    [Range(0, 0.1f)]
    public float HILL_SPEED_LOW;
    [Range(0, 0.5f)]
    public float HILL_SPEED_HIGH;

    private float timeAfterHit = float.MaxValue; 

    void Start()
    {
        Settings s    = GameObject.FindGameObjectWithTag("Settings").GetComponent<Settings>();
        maxHealth     = s.PLAYER_HEALTH;
        currentHealth = maxHealth;
    }

    void Update()
    {
        if (!IsAlive)
            Die();
        else
            Hill();
    }

    /*protected override void Die()
    {

    }*/

    override public void OnTriggerEnter(Collider other)
    {
        if (gameObject.layer == other.gameObject.layer)
            return;

        Weapon w = other.GetComponent<Weapon>();
        if (w && w.hit)
        {
            currentHealth -= w.DAMAGE;
            w.hit          = false;
            timeAfterHit   = 0;
            other.GetComponent<HitSound>().PlayHitSound(THIS_MATERIAL);
            print("Hit, damage=" + w.DAMAGE + ", health=" + currentHealth);
        }
    }

    private void Hill()
    {
        if (timeAfterHit >= HILL_START_TIME)
        {
            float __hillSpeed = (maxHealth - currentHealth) < 10 ? HILL_SPEED_HIGH : HILL_SPEED_LOW;
            timeAfterHit      = float.MaxValue;
            currentHealth     = Mathf.Lerp(currentHealth, maxHealth, __hillSpeed);
        }
        else
        {
            timeAfterHit += Time.deltaTime;
        }
    }
}
