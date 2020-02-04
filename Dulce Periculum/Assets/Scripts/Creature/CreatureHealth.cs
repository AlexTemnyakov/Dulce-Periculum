using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureHealth : MonoBehaviour
{
    public float HEALTH;

    void Start()
    {
        
    }

    void Update()
    {
        if (!IsAlive())
            Die();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (gameObject.layer == other.gameObject.layer)
            return;

        Damage d = other.GetComponent<Damage>();
        if (d)
        {
            HEALTH -= d.DAMAGE;
            print("Hit, damage=" + d.DAMAGE + ", health=" + HEALTH);
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
