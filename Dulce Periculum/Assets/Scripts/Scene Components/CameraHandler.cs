using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private GameObject player;
    public  float      distFromPlayer;
    public  float      angle;
    public  float      height;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ChangePosition();
    }

    void LateUpdate()
    {
        ChangePosition();
    }

    private void ChangePosition()
    {
        Vector3 dir, newPos;

        dir                = player.transform.forward.normalized;
        newPos             = player.transform.position + (-dir * distFromPlayer);
        newPos.y           = player.transform.position.y + height;
        transform.position = newPos;
        transform.forward  = dir;

        transform.Rotate(angle, 0, 0);
    }
}
