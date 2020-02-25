using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHitRegistrator : MonoBehaviour
{
    private BuildingHealth buildingHealth;

    void Start()
    {
        buildingHealth = GetComponentInParent<BuildingHealth>();
        if (!buildingHealth)
        {
            buildingHealth = GetComponent<BuildingHealth>();
            if (!buildingHealth)
                Debug.LogError("The building can't be destroyed.");
        }

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

            buildingHealth.Hit(this, w.Damage);
        }
    }
}
