using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class VillagerBrains : MonoBehaviour
{
    public float         MAX_DIST_FROM_START;
    public float         SPEED;
    public float         ACCELERATION;

    private NavMeshAgent agent;
    private Animator     animator;
    private Vector3      startPoint;
    private Vector3      targetPoint;

    void Start()
    {
        NavMeshHit hit;

        agent       = GetComponent<NavMeshAgent>();
        animator    = GetComponent<Animator>();
        agent.speed = SPEED;
        startPoint  = transform.position;

        if (NavMesh.SamplePosition(startPoint, out hit, 10, NavMesh.AllAreas))
        {
            startPoint = hit.position;
            agent.SetDestination(CreateTargetPoint());
            Walk();
        }
        else
        {
            Debug.LogError("Villager, a problem with the navmesh.");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (agent.remainingDistance < 1)
        {
            agent.SetDestination(CreateTargetPoint());

            Walk();
        }
    }

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
            Debug.LogError("Vilalger, a problem with the navmesh, he can not find a new position.");
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
}
