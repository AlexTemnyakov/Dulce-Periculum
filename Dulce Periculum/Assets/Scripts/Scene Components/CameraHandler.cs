using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public  float      DIST_FROM_PLAYER;
    public  float      ANGLE;
    public  float      HEIGHT;
    private const
            float      PLAYER_HEIGHT_OFFSET = 4.0f;
    private const
            float      CHANGE_POS_SPEED     = 0.8f;

    private GameObject player;

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
        Vector3    dir, 
                   newPos,
                   tmp;
        float      _dist    = DIST_FROM_PLAYER,
                   _height  = HEIGHT;

        dir                 = player.transform.forward.normalized;
        newPos              = player.transform.position + (-dir * _dist);
        newPos.y            = player.transform.position.y + _height;
        transform.position  = Vector3.Slerp(CorrectPosition(newPos), transform.position, CHANGE_POS_SPEED);

        // Set the desired angle.
        transform.forward          = player.transform.position - transform.position;
        tmp                        = transform.localEulerAngles;
        tmp.x                      = ANGLE;
        transform.localEulerAngles = tmp;
    }

    private Vector3 CorrectPosition(Vector3 position)
    {
        Vector3[]  pointsToCheck = {
                                            Vector3.zero,
                                        new Vector3(15, -14, 0),
                                        new Vector3(15, 14, 0)
                                    };
        Vector3    newPos = position;
        Vector3    dir;
        RaycastHit hit;

        foreach (Vector3 v in pointsToCheck)
        {
            dir = Quaternion.Euler(v) * (position - (player.transform.position + Vector3.up * PLAYER_HEIGHT_OFFSET));
            if (Physics.Raycast(player.transform.position + Vector3.up * PLAYER_HEIGHT_OFFSET, dir, out hit, DIST_FROM_PLAYER))
            {
                newPos = hit.point - dir.normalized * 2;
                break;
            }
        }

        return newPos;
    }
}
