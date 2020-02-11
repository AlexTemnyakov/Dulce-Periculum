using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class GoblinBrains : MonoBehaviour
{
    public  GoblinAction   ACTION;
    public  float          SPEED;
    public  float          ACCELERATION;
    public  float          ROTATION_SPEED;
    public  float          MAX_DIST_FROM_START;
    public  float          VISIBILITY;
    public  float          VISION_ANGLE;
    public  float          ATTACK_DIST;
    public  GameObject     WEAPON;

    private const
            float          NEW_TARGET_DIST      = 10;
    private const
            float          PLAYER_HEIGHT_OFFSET = 200.0f;
    private const
            int            HIT_TYPES_COUNT      = 2;

    private AIUtils        AI_Utils;
    private NavMeshAgent   agent;
    private GameObject     player;
    private Animator       animator;
    private CreatureHealth health;
    private GoblinFight    fight;
    private Vector3        startPoint;
    private GameObject     target;

    void Start()
    {
        AI_Utils    = GameObject.FindGameObjectWithTag("AI Utils").GetComponent<AIUtils>();
        agent       = GetComponent<NavMeshAgent>();
        player      = GameObject.FindGameObjectWithTag("Player");
        animator    = GetComponent<Animator>();
        health      = GetComponent<CreatureHealth>();
        fight       = GetComponent<GoblinFight>();
        agent.speed = SPEED ;
        startPoint  = transform.position;
    }

    void Update()
    {
        if (!health.IsAlive())
            return;

        //Debug.DrawLine(transform.position, target.transform.position, Color.red);

        if (AI_Utils.IsPlayerAtAttackDistance(transform.position, ATTACK_DIST))
        {
            Stand();
            RotateTo(player.transform.position);
            fight.Attack();
        }
        else
        {
            if (ACTION == GoblinAction.ATTACK_VILLAGE)
            {
                if (!target || !target.activeInHierarchy)
                {
                    target = AI_Utils.GetHouseInVillage();
                    //target = GameObject.FindGameObjectWithTag("Village");

                    if (target)
                    {
                        SetAsAgentTarget(target.transform.position);
                    }
                    else
                    {
                        ACTION = GoblinAction.ATTACK_PLAYER;
                    }
                }
                else
                {
                    if (IsTargetAtAttackDistance())
                    {
                        Stand();
                        RotateTo(target.transform.position);
                        fight.Attack();
                    }
                    else
                    {
                        Run();
                    }
                }
            }
            else if (ACTION == GoblinAction.ATTACK_PLAYER)
            {
                if (AI_Utils.IsPlayerVisible(transform.position, transform.forward, VISIBILITY, VISION_ANGLE))
                {
                    SetAsAgentTarget(player.transform.position);
                    Run();
                }
                else
                {
                    if (agent.remainingDistance < 1)
                    {
                        agent.SetDestination(CreateTargetPoint());
                    }

                    Walk();
                }
            }
        }
    }

    // Create a random point within the maximum distance from the starting point.
    private Vector3 CreateTargetPoint()
    {
        int        angle;
        Vector3    vector;
        Vector3    targetPoint;
        NavMeshHit hit;

        angle       = Random.Range(0, 360);
        vector      = Quaternion.Euler(0, angle, 0) * new Vector3(1, 0, 1) * 20;
        targetPoint = transform.position + vector;

        if (NavMesh.SamplePosition(targetPoint, out hit, 10, NavMesh.AllAreas))
        {
            if (Vector3.Distance(startPoint, hit.position) < MAX_DIST_FROM_START)
                targetPoint = hit.position;
        }
        else
        {
            Debug.LogError("Goblin, a problem with the navmesh, he can not find a new position.");
        }

        return targetPoint;
    }

    private void Run()
    {
        agent.speed = SPEED * ACCELERATION;
        animator.SetFloat("Speed", agent.speed);
    }

    private void Walk()
    {
        agent.speed = SPEED;
        animator.SetFloat("Speed", agent.speed);
    }

    private void Stand()
    {
        agent.ResetPath();
        agent.speed = 0;
        animator.SetFloat("Speed", agent.speed);
    }

    private void RotateTo(Vector3 point)
    {
        Vector3 dir;

        dir               = point - transform.position;
        dir.y             = transform.forward.y;
        transform.forward = Vector3.Lerp(transform.forward, dir.normalized, Time.deltaTime * ROTATION_SPEED);
    }

    private void SetAsAgentTarget(Vector3 position)
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(position, out hit, 10, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            Debug.LogError("Goblin, a problem with the navmesh.");
            Debug.Break();
        }
    }

    private bool IsTargetAtAttackDistance()
    {
        // If the target object has been disappeared from the world.
        if (!target || !target.activeInHierarchy)
        {
            return false;
        }
        else
        {
            // If the target object is to far. It is possible that the raycast hits another building in the world, 
            // so this situation must be avoided.
            if (Vector3.Distance(transform.position, target.transform.position) > 20)
            {
                return false;
            }
            else
            {
                RaycastHit hit;

                // Vector3.up * 2 is used because transform.position is on ground, it must be lifted.
                if (Physics.Raycast(transform.position + Vector3.up * 2, target.transform.position - transform.position, out hit, ATTACK_DIST, LayerMask.GetMask("Buildings")))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
