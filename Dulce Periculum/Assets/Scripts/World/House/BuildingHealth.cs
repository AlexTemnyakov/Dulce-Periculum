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
        {
            StartCoroutine(Destruct());
        }
    }

    private IEnumerator Destruct()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        int start = children.Length > 1 ? 1 : 0; 
        if (children.Length < 1)
        {
            Destroy(gameObject);
        }
        else
        {
            for (int i = 1; i < children.Length; i++)
            {
                if (!children[i].gameObject.GetComponent<Rigidbody>())
                {
                    children[i].gameObject.AddComponent<Rigidbody>();
                    children[i].gameObject.GetComponent<Rigidbody>().mass = 500;
                    //children[i].gameObject.GetComponent<Rigidbody>().useGravity = true;
                    //children[i].gameObject.GetComponent<Rigidbody>().AddForce(Vector3.down * 0.01f + Vector3.right * 0.01f);
                }

                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForSeconds(5.0f);

            Destroy(gameObject);
        }
    }

    public void Hit(BuildingHitRegistrator source, float damage)
    {
        HEALTH -= HIT_DAMAGE;
        print("Health=" + HEALTH);
    }
}
