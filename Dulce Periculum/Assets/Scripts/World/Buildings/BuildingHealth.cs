using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingHealth : MonoBehaviour
{
    public float HEALTH;
    public float HIT_DAMAGE;

    void Start()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        int         start    = children.Length > 1 ? 1 : 0;
        for (int i = start; i < children.Length; i++)
        {
            if (children[i].GetComponent<Collider>())
            {
                children[i].gameObject.AddComponent<BuildingHitRegistrator>();
            }
        }
    }

    void LateUpdate()
    {
        if (HEALTH <= 0)
            Destroy(gameObject);
    }

    public void Hit(BuildingHitRegistrator source, float damage)
    {
        HEALTH -= HIT_DAMAGE;
        print("Health=" + HEALTH);
    }
}
