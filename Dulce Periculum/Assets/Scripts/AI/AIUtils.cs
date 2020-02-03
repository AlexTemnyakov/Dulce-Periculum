using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUtils : MonoBehaviour
{
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
        if (!player)
        {
            Debug.LogError("There is no object with the tag Player");
        }
    }

    void Update()
    {
        
    }

    public float DistanceToPlayer(Vector3 position) { return Vector3.Distance(position, player.transform.position); }
    public bool  IsPlayerVisible(Vector3 position, Vector3 forward, float visibility, float visionAngle, float playerHeightOffset)
    {
        RaycastHit hit;
        Vector3    playerDirection = player.transform.position - position;

        //Debug.DrawRay(position, playerDirection * visibility + Vector3.up * playerHeightOffset, Color.black);
        if (Physics.Raycast(position, playerDirection * visibility + Vector3.up * playerHeightOffset, out hit, visibility))
        {
            if (hit.transform.gameObject == player && Vector3.Angle(playerDirection, forward) <= visionAngle)
                return true;
        }

        return false;
    }

    public bool IsPlayerAtAttackDistance(Vector3 position, float attackDistance)
    {
        if (DistanceToPlayer(position) < attackDistance)
            return true;
        else
            return false;
    }
}
