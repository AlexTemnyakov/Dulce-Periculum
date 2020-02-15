using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBrains : MonoBehaviour
{
    public    float        SPEED;
    public    float        ACCELERATION;
    public    float        MAX_DIST_FROM_START;
    public    float        VISIBILITY;
    public    float        VISION_ANGLE;
    public    float        ATTACK_DIST;
    public    float        INTERACTION_DIST;
    public    float        ROTATION_SPEED;
    public    float        NEW_TARGET_DIST;

    protected bool         waiting     = false;
    protected GameManager  gameManager;
    protected NavMeshAgent agent;
    protected GameObject   target      = null;

    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        agent       = GetComponent<NavMeshAgent>();
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

    protected void SetAsAgentTarget(Vector3 position)
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(position, out hit, 10, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            Debug.LogError("Enemy, a problem with the navmesh.");
            //Debug.Break();
        }
    }

    protected Vector3 CreateTargetPoint()
    {
        int     angle;
        Vector3 vector;
        Vector3 targetPoint;

        angle       = Random.Range(0, 360);
        vector      = Quaternion.Euler(0, angle, 0) * new Vector3(1, 0, 1) * NEW_TARGET_DIST;
        targetPoint = transform.position + vector;

        return targetPoint;
    }

    protected void RotateTo(Vector3 point)
    {
        Vector3 dir;

        dir               = point - transform.position;
        dir.y             = transform.forward.y;
        transform.forward = Vector3.Lerp(transform.forward, dir.normalized, Time.deltaTime * ROTATION_SPEED);
    }

    virtual protected void Run()
    {
        agent.speed = SPEED * ACCELERATION;
    }

    virtual protected void Walk()
    {
        agent.speed = SPEED;
    }

    virtual protected void Stand()
    {
        agent.speed = 0;
    }

    protected  IEnumerator WaitFor(float seconds)
    {
        waiting = true;
        yield return new WaitForSeconds(seconds);
        waiting = false;
    }
}
