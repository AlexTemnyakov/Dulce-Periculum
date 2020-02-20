using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public  float      DIST_FROM_PLAYER;
    public  float      ANGLE;
    public  float      HEIGHT;

    private const
            float      CHANGE_POS_SPEED  = 0.1f;

    private bool        initialized      = false;
    private GameManager gameManager;
    private float       currentAngle;
    private float       minAngle;
    private float       maxAngle;

    void Start()
    {
        gameManager  = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        currentAngle = ANGLE;
        minAngle     = ANGLE - 5;
        maxAngle     = ANGLE + 5;

        //StartCoroutine(MoveToPlayerAtStart());
        initialized = true;
    }

    void FixedUpdate()
    {
        if (!initialized)
            return;

        //if (Input.GetMouseButton(1))
        //    ChangeAngle();
        ChangePosition();
    }

    public void Initialize()
    {
        initialized = true;
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
        if (!gameManager.Player)
            return;

        Vector3 dir, 
                newPos,
                tmp;
        float   _dist   = DIST_FROM_PLAYER,
                _height = HEIGHT,
                _angle  = currentAngle;

        if (IsInsideObject()/* || IsCameraInsideObject()*/)
        {
            _dist   /= 2;
            _height /= 1.5f;
            _angle  /= 1.5f;
        }

        dir                 = gameManager.Player.transform.forward.normalized;
        newPos              = gameManager.Player.transform.position + (-dir * _dist);
        newPos.y            = gameManager.Player.transform.position.y + _height;
        transform.position  = Vector3.Slerp(transform.position, CorrectPosition(newPos, _dist), CHANGE_POS_SPEED);

        // Set the desired angle.
        transform.forward          = gameManager.Player.transform.position - transform.position;
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
            dir = Quaternion.Euler(v) * (position - (gameManager.Player.transform.position + Vector3.up * Utils.PLAYER_HEIGHT_OFFSET));
            if (Physics.Raycast(gameManager.Player.transform.position + Vector3.up * Utils.PLAYER_HEIGHT_OFFSET, dir, out hit, dist, LayerMask.GetMask("Buildings", "Terrain")))
            {
                newPos = hit.point - dir.normalized * 2;
                break;
            }
        }

        return newPos;
    }

    private bool IsInsideObject()
    {
        Vector3[]  directions = { Quaternion.Euler(-20, 0, 0) * gameManager.Player.transform.up,
                                  gameManager.Player.transform.up,
                                  -gameManager.Player.transform.up };
        RaycastHit hit;
        int        count = 0;

        foreach (Vector3 d in directions)
        {
            if (Physics.Raycast(gameManager.Player.transform.position + Vector3.up * Utils.PLAYER_HEIGHT_OFFSET, d, out hit, DIST_FROM_PLAYER * 10f, LayerMask.GetMask("Buildings")))
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

    private IEnumerator MoveToPlayerAtStart()
    {
        if (!gameManager.Player || initialized)
            yield break;

        while (Vector3.Distance(transform.position, gameManager.Player.transform.position - gameManager.Player.transform.forward * DIST_FROM_PLAYER + Vector3.up * Utils.PLAYER_HEIGHT_OFFSET) > DIST_FROM_PLAYER)
        {
            transform.position = Vector3.Lerp(transform.position, gameManager.Player.transform.position - gameManager.Player.transform.forward * DIST_FROM_PLAYER + Vector3.up * Utils.PLAYER_HEIGHT_OFFSET, 0.01f);
            transform.forward  = Vector3.Lerp(transform.forward, gameManager.Player.transform.position + Vector3.up * Utils.PLAYER_HEIGHT_OFFSET - transform.position, 0.05f);

            yield return null;
        }

        initialized = true;
    }
}
