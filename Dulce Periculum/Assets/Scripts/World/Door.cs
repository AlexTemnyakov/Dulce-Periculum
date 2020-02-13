using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    public  float      HEALTH;

    private bool       closed;
    private bool       moving;
    private Quaternion targetRotation;
    private Vector3    openingAngles = new Vector3(0, -90, 0);
    private Vector3    closingAngles = new Vector3(0, 90, 0);

    void Start()
    {
        closed = true;
        moving = false;
    }

    void Update()
    {
        if (HEALTH <= 0)
            Destroy(gameObject);

        if (moving)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.05f);
            if (transform.rotation == targetRotation)
                moving = false;
        }
    }

    public override void Interact()
    {
        if (moving)
            return;

        if (closed)
        {
            targetRotation = Quaternion.Euler(transform.eulerAngles + openingAngles);
        }
        else
        {
            targetRotation = Quaternion.Euler(transform.eulerAngles + closingAngles);
        }

        closed = !closed;
        moving = true;
    }

    public void OnTriggerEnter(Collider other)
    {
        Weapon w = other.GetComponent<Weapon>();
        if (w && w.hit)
        {
            HEALTH -= w.DAMAGE;
            w.hit   = false;
            //other.GetComponent<HitSound>().PlayHitSound(THIS_MATERIAL);
            print("Hit, damage=" + w.DAMAGE + ", health=" + HEALTH);
        }
    }


}
