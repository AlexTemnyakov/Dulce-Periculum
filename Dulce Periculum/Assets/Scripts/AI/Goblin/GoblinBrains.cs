using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class GoblinBrains : MonoBehaviour
{
    public  AIUtils       AI_UTILS;
    public  float         SPEED;
    public  float         ACCELERATION;
    public  float         MAX_DIST_FROM_START;
    public  float         VISIBILITY;
    public  float         VISION_ANGLE;
    public  float         ATTACK_DIST;
    private const
            float         NEW_TARGET_DIST      = 10;
    private const
            float         PLAYER_HEIGHT_OFFSET = 200.0f;
    private const
            int           HIT_TYPES_COUNT      = 2;

    private NavMeshAgent  agent;
    private GameObject    player;
    private Animator      animator;
    private Vector3       startPoint;
    private int           hitNum;

    void Start()
    {
        NavMeshHit hit;

        agent       = GetComponent<NavMeshAgent>();
        animator    = GetComponent<Animator>();
        player      = GameObject.FindGameObjectWithTag("Player");
        agent.speed = SPEED ;
        startPoint  = transform.position;
        hitNum      = 0;

        if (NavMesh.SamplePosition(startPoint, out hit, 10, NavMesh.AllAreas))
        {
            startPoint = hit.position;
            agent.SetDestination(CreateTargetPoint());
        }
        else
        {
            Debug.LogError("Goblin, a problem with the navmesh.");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (AI_UTILS.IsPlayerAtAttackDistance(transform.position, ATTACK_DIST))
        {
            Stand();
            Attack();
        }
        else
        {
            if (AI_UTILS.IsPlayerVisible(transform.position, VISIBILITY, VISION_ANGLE, PLAYER_HEIGHT_OFFSET))
            {
                SetPlayerAsTarget();
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

    // Create a random point within the maximum distance from the starting point.
    private Vector3 CreateTargetPoint()
    {
        int        angle;
        Vector3    vector;
        Vector3    targetPoint;
        NavMeshHit hit;

        targetPoint = startPoint;

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

    private void Attack()
    {
        animator.SetInteger("Hit", hitNum);
        animator.SetTrigger("Attack");

        hitNum = (hitNum + 1) % HIT_TYPES_COUNT;
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
        agent.speed = 0;
        animator.SetFloat("Speed", agent.speed);
    }

    private void SetPlayerAsTarget()
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(player.transform.position, out hit, 10, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            Debug.LogError("Goblin, a problem with the navmesh, he can not find player's position.");
        }
    }
}
