using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class GoblinBrains : EnemyBrains
{
    public  GoblinAction   ACTION;
    public  float          SPEED;
    public  float          ACCELERATION;
    public  float          ROTATION_SPEED;
    public  float          MAX_DIST_FROM_START;
    public  GameObject     WEAPON;

    private const
            float          NEW_TARGET_DIST      = 10;
    private const
            float          PLAYER_HEIGHT_OFFSET = 200.0f;
    private const
            int            HIT_TYPES_COUNT      = 2;

    // Components.
    private NavMeshAgent   agent;
    private Animator       animator;
    private CreatureHealth health;
    private GoblinFight    fight;
    // Information for remembering at the start.
    private Vector3        startPoint;
    private GameObject     target;
    // Stealing.
    private StealingAction stealingAction;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        agent       = GetComponent<NavMeshAgent>();
        animator    = GetComponent<Animator>();
        health      = GetComponent<CreatureHealth>();
        fight       = GetComponent<GoblinFight>();
        agent.speed = SPEED ;
        startPoint  = transform.position;
    }

    void Update()
    {
        if (!health.IsAlive())
        {
            agent.ResetPath();
            Stand();
            return;
        }

        if (IsPlayerAtAttackDistance())
        {
            AttackPlayer();
        }
        else
        {
            if (ACTION == GoblinAction.STEALING)
            {
                Steal();
            }
            else if (ACTION == GoblinAction.ATTACKING_VILLAGE)
            {
                AttackVillage();
            }
            else if (ACTION == GoblinAction.ATTACKING_PLAYER)
            {
                GoToPlayer();
            }
        }
    }

    // Create a random point within the maximum distance from the starting point.
    private Vector3 CreateTargetPoint()
    {
        int        angle;
        Vector3    vector;
        Vector3    targetPoint;

        angle       = Random.Range(0, 360);
        vector      = Quaternion.Euler(0, angle, 0) * new Vector3(1, 0, 1) * 20;
        targetPoint = transform.position + vector;

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

    private void Steal()
    {
        if (stealingAction == null || stealingAction.Building != target)
        {
            stealingAction = new StealingAction(target);
        }

        switch (stealingAction.State)
        {
            case StealingState.START:
            {
                if (stealingAction.Door)
                {
                    SetAsAgentTarget(stealingAction.Door.transform.position);
                    stealingAction.State = StealingState.OPEN_DOOR;
                }
                else
                {

                }
            } break;

            case StealingState.OPEN_DOOR:
            {

            } break;
        }
    }

    private void AttackPlayer()
    {
        Stand();
        RotateTo(gameManager.Player.transform.position);
        fight.Attack();
    }

    private void GoToPlayer()
    {
        if (target != gameManager.Player)
        {
            target = gameManager.Player;
        }

        SetAsAgentTarget(gameManager.Player.transform.position);
        Run();
    }

    private void AttackVillage()
    {
        if (!target || !target.activeInHierarchy)
        {
            target = gameManager.GetHouseInVillage();

            if (target)
            {
                SetAsAgentTarget(target.transform.position);
            }
            else
            {
                Stand();
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
}
