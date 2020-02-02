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
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (gameObject.layer == other.gameObject.layer)
            return;

        Damage d = other.GetComponentInParent<Damage>();
        HEALTH  -= d.DAMAGE;

        print("Hit, damage=" + d.DAMAGE + ", health=" + HEALTH);

        if (HEALTH <= 0)
        {
            Destroy(gameObject);
        }
    }
}
