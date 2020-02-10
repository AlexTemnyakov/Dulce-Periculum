using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHitRegistrator : MonoBehaviour
{
    private BuildingHealth buildingHealth;

    void Start()
    {
        buildingHealth = GetComponentInParent<BuildingHealth>();
    }


    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        Weapon w = other.GetComponent<Weapon>();
        if (w && !w.hit)
        {
            //HEALTH -= w.DAMAGE;
            w.hit   = true;

            buildingHealth.Hit(this, w.DAMAGE);
        }
    }
}
