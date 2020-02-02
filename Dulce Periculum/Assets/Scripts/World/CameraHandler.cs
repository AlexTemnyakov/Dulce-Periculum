using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public  float      DIST_FROM_PLAYER;
    public  float      ANGLE;
    public  float      HEIGHT;
    //private const
    //        float      PLAYER_HEIGHT_OFFSET = 3f;
    private const
            float      CHANGE_POS_SPEED     = 0.5f;

    private GameObject player;
    private Vector3    newPos;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        ChangePosition();
    }

    void Update()
    {
        ChangePosition();
    }

    private void ChangePosition()
    {
        Vector3    dir, 
                   newPos,
                   tmp;
        float      _dist    = DIST_FROM_PLAYER,
                   _height  = HEIGHT,
                   _angle   = ANGLE;

        if (IsInsideObject())
        {
            _dist   /= 2;
            _height /= 2;
            _angle  /= 2;
        }

        dir                 = player.transform.forward.normalized;
        newPos              = player.transform.position + (-dir * _dist);
        newPos.y            = player.transform.position.y + _height;
        transform.position  = Vector3.Slerp(transform.position, CorrectPosition(newPos, _dist), CHANGE_POS_SPEED);

        // Set the desired angle.
        transform.forward          = player.transform.position - transform.position;
        tmp                        = transform.localEulerAngles;
        tmp.x                      = _angle;
        transform.localEulerAngles = tmp;
    }

    private Vector3 CorrectPosition(Vector3 position, float dist)
    {
        Vector3[]  pointsToCheck = {
                                            Vector3.zero,
                                        new Vector3(0, -15, 0),
                                        new Vector3(0, 15, 0)
                                    };
        Vector3    newPos = position;
        Vector3    dir;
        RaycastHit hit;

        foreach (Vector3 v in pointsToCheck)
        {
            dir = Quaternion.Euler(v) * (position - (player.transform.position + Vector3.up * Utils.PLAYER_HEIGHT_OFFSET));
            if (Physics.Raycast(player.transform.position + Vector3.up * Utils.PLAYER_HEIGHT_OFFSET, dir, out hit, dist))
            {
                if (hit.transform.gameObject.tag.Equals("Player"))
                {
                    continue;
                }
                else
                {
                    newPos = hit.point - dir.normalized * 2;
                    break;
                }
            }
        }

        return newPos;
    }

    private bool IsInsideObject()
    {
        Vector3[]  directions = { Quaternion.Euler(-20, 0, 0) * player.transform.up,
                                  player.transform.up,
                                  -player.transform.up };
        RaycastHit hit;
        int        count = 0;

        foreach (Vector3 d in directions)
        {
            if (Physics.Raycast(player.transform.position + Vector3.up * Utils.PLAYER_HEIGHT_OFFSET, d, out hit, DIST_FROM_PLAYER * 10f, LayerMask.GetMask("Buildings")))
            {
                count += 1;
            }
        }

        return count >= directions.Length ? true : false;
    }
}
