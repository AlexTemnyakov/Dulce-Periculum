using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    [Range(0, 1)]
    public  float   SPEED;
    public  float   LIFETIME;
    public  float   DAMAGE;

    private Vector3 direction      = Vector3.zero;
    private float   timeAfterStart = 0;
    private bool    initialized    = false;

    void Update()
    {
        if (!initialized)
            return;

        timeAfterStart += Time.deltaTime;

        if (timeAfterStart >= LIFETIME)
            Destroy(gameObject);
        else
            transform.Translate(direction * SPEED);
    }

    public void Initialize(Vector3 __direction)
    {
        direction   = __direction;
        initialized = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        int otherLayer = collision.gameObject.layer;

        if (otherLayer == LayerMask.NameToLayer("Player"))
            return;
        else
            Destroy(gameObject);
    }
}
