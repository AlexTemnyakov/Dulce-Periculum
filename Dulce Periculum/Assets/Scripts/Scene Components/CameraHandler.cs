using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public  float      DIST_FROM_PLAYER;
    public  float      ANGLE;
    public  float      HEIGHT;
    private const
            float      PLAYER_HEIGHT_OFFSET = 3f;
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
            dir = Quaternion.Euler(v) * (position - (player.transform.position + Vector3.up * PLAYER_HEIGHT_OFFSET));
            if (Physics.Raycast(player.transform.position + Vector3.up * PLAYER_HEIGHT_OFFSET, dir, out hit, dist))
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
        Vector3[] directions = {
                                    player.transform.forward,
                                    Quaternion.Euler(0, 72, 0) * player.transform.forward,
                                    Quaternion.Euler(0, 144, 0) * player.transform.forward,
                                    Quaternion.Euler(0, 216, 0) * player.transform.forward,
                                    Quaternion.Euler(0, 288, 0) * player.transform.forward,
                                    /*player.transform.forward,
                                    player.transform.right,
                                    -player.transform.forward,
                                    -player.transform.right,*/
                               };
        RaycastHit hit;
        int        count = 0;

        //Debug.DrawRay(player.transform.position + Vector3.up * PLAYER_HEIGHT_OFFSET, player.transform.forward * DIST_FROM_PLAYER * 2);
        //Debug.DrawRay(player.transform.position + Vector3.up * PLAYER_HEIGHT_OFFSET, Quaternion.Euler(0, 72, 0) * player.transform.forward * DIST_FROM_PLAYER * 2);
        //Debug.DrawRay(player.transform.position + Vector3.up * PLAYER_HEIGHT_OFFSET, Quaternion.Euler(0, 144, 0) * player.transform.forward * DIST_FROM_PLAYER * 2);
        //Debug.DrawRay(player.transform.position + Vector3.up * PLAYER_HEIGHT_OFFSET, Quaternion.Euler(0, 216, 0) * player.transform.forward * DIST_FROM_PLAYER * 2);
        //Debug.DrawRay(player.transform.position + Vector3.up * PLAYER_HEIGHT_OFFSET, Quaternion.Euler(0, 288, 0) * player.transform.forward * DIST_FROM_PLAYER * 2);

        foreach (Vector3 d in directions)
        {
            if (Physics.Raycast(player.transform.position + Vector3.up * PLAYER_HEIGHT_OFFSET, d, out hit, DIST_FROM_PLAYER * 2))
            {
                if (hit.transform.gameObject.tag.Equals("Player"))
                    continue;
                count += 1;
            }
        }

        return count >= 4 ? true : false;
    }
}
