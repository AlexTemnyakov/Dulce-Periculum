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
        print("Hit, damage=" + d.GetDamage() + ", health=" + HEALTH);

        HEALTH -= d.GetDamage();

        if (HEALTH <= 0)
        {
            Destroy(gameObject);
        }
    }
}
