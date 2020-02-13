using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBrains : MonoBehaviour
{
    public    float       VISIBILITY;
    public    float       VISION_ANGLE;
    public    float       ATTACK_DIST;

    protected GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
    }

    void Update()
    {

    }

    protected float DistanceToPlayer() { return Vector3.Distance(transform.position, gameManager.Player.transform.position); }

    protected bool IsPlayerVisible()
    {
        RaycastHit hit;
        Vector3    playerDirection = gameManager.Player.transform.position - transform.position;

        if (Physics.Raycast(transform.position, playerDirection + Vector3.up * Utils.PLAYER_HEIGHT_OFFSET, out hit, VISIBILITY))
        {
            if (hit.transform.gameObject == gameManager.Player && Vector3.Angle(playerDirection, transform.forward) <= VISION_ANGLE)
                return true;
        }

        return false;
    }

    protected bool IsPlayerAtAttackDistance()
    {
        if (DistanceToPlayer() < ATTACK_DIST)
            return true;
        else
            return false;
    }
}
