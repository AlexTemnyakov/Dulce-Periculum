using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
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

    IEnumerator RotateMe(Vector3 byAngles, float inTime)
    {
        var fromAngle = transform.rotation;
        var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
        for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
        {
            transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }
    }
}
