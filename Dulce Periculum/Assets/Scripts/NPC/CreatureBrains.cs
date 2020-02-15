using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class CreatureBrains : MonoBehaviour
{
    public    float        SPEED;
    public    float        ACCELERATION;
    public    float        VISIBILITY;
    public    float        VISION_ANGLE;
    public    float        NEW_TARGET_DIST;
    public    float        ROTATION_SPEED;
    public    float        MAX_DIST_FROM_START;

    protected NavMeshAgent agent;

    protected bool         waiting          = false;
    protected GameManager  gameManager;

    void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        agent       = GetComponent<NavMeshAgent>();
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
            Debug.LogError("Creature, a problem with the navmesh.");
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

    protected void LookAtDirection(Vector3 direction)
    {
        direction.y       = 0;
        transform.forward = Vector3.Lerp(transform.forward, direction.normalized, Time.deltaTime * ROTATION_SPEED);
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

    protected IEnumerator WaitFor(float seconds)
    {
        waiting = true;
        yield return new WaitForSeconds(seconds);
        waiting = false;
    }
}
