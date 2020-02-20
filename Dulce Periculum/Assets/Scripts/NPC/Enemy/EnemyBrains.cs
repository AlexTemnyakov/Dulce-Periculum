using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBrains : CreatureBrains
{
    public    float      ATTACK_DIST;
    public    float      INTERACTION_DIST;

    protected GameObject target            = null;

    protected float DistanceToPlayer()
    {
        if (!gameManager.Player)
            return int.MaxValue;
        else
            return Vector3.Distance(transform.position, gameManager.Player.transform.position);
    }

    protected bool IsPlayerVisible()
    {
        if (!gameManager.Player)
            return false;

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
        if (DistanceToPlayer() <= ATTACK_DIST)
            return true;
        else
            return false;
    }

    protected bool IsTargetAtAttackDistance()
    {
        // If the target object has been disappeared from the world.
        if (!target || !target.activeInHierarchy)
        {
            return false;
        }
        else
        {
            /*Collider[] hitColliders = Physics.OverlapSphere(transform.position + transform.forward + Vector3.up * Utils.PLAYER_HEIGHT_OFFSET,
                                                            ATTACK_DIST - transform.forward.magnitude);
            foreach (Collider c in hitColliders)
            {
                if (c.gameObject == target)
                {
                    return true;
                }
            }

            return false;*/

            return Vector3.Distance(transform.position, target.transform.position) <= ATTACK_DIST;/* && Vector3.Angle(transform.forward, target.transform.position) <= 60;*/
            
        
            /*RaycastHit hit;

            Debug.DrawRay(transform.position, target.transform.position - transform.position, Color.green, ATTACK_DIST);

            if (Physics.Raycast(transform.position, target.transform.position - transform.position, out hit, ATTACK_DIST, LayerMask.GetMask("Buildings")))
            {
                return true;
            }
            else
            {
                return false;
            }*/
        }
    }

    protected bool IsTargetAtInteractionDistance()
    {
        // If the target object has been disappeared from the world.
        if (!target || !target.activeInHierarchy)
        {
            return false;
        }
        else
        {
            return Vector3.Distance(transform.position, target.transform.position) <= INTERACTION_DIST;
        }
    }
}
