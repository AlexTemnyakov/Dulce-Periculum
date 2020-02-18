using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public  float      DIST_FROM_PLAYER;
    public  float      ANGLE;
    public  float      HEIGHT;
    private const
            float      CHANGE_POS_SPEED     = 0.1f;

    private GameObject player;
    private Vector3    newPos;
    private float      currentAngle;
    private float      minAngle;
    private float      maxAngle;

    void Start()
    {
        player       = GameObject.FindGameObjectWithTag("Player");
        currentAngle = ANGLE;
        minAngle     = ANGLE - 5;
        maxAngle     = ANGLE + 5;
        ChangePosition();
    }

    void FixedUpdate()
    {
        //if (Input.GetMouseButton(1))
        //    ChangeAngle();
        ChangePosition();
    }

    private void ChangeAngle()
    {
        currentAngle = Mathf.Lerp(currentAngle, currentAngle - Input.GetAxis("Mouse Y"), 0.2f);
        if (currentAngle < minAngle)
            currentAngle = minAngle;
        else if (currentAngle > maxAngle)
            currentAngle = maxAngle;
    }

    private void ChangePosition()
    {
        Vector3    dir, 
                   newPos,
                   tmp;
        float      _dist    = DIST_FROM_PLAYER,
                   _height  = HEIGHT,
                   _angle   = currentAngle;

        if (IsInsideObject()/* || IsCameraInsideObject()*/)
        {
            _dist   /= 2;
            _height /= 1.5f;
            _angle  /= 1.5f;
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
            if (Physics.Raycast(player.transform.position + Vector3.up * Utils.PLAYER_HEIGHT_OFFSET, dir, out hit, dist, LayerMask.GetMask("Buildings", "Terrain")))
            {
                newPos = hit.point - dir.normalized * 2;
                break;
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

    private bool IsCameraInsideObject()
    {
        Vector3[] directions = { Quaternion.Euler(-20, 0, 0) * transform.up,
                                  transform.up,
                                  -transform.up };
        RaycastHit hit;
        int count = 0;

        foreach (Vector3 d in directions)
        {
            if (Physics.Raycast(transform.position, d, out hit, DIST_FROM_PLAYER * 10f, LayerMask.GetMask("Buildings")))
            {
                count += 1;
            }
        }

        return count >= directions.Length ? true : false;
    }
}
