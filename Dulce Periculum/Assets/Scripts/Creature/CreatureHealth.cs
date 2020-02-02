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
        Damage d = other.GetComponentInParent<Damage>();
        print("Hit, damage=" + d.GetDamage() + ", health=" + HEALTH);

        HEALTH -= d.GetDamage();

        if (HEALTH <= 0)
        {
            print("qqq");
            Destroy(gameObject);
        }
    }
}
