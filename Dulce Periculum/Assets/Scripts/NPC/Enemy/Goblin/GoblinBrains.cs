using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class GoblinBrains : EnemyBrains
{
    public  GoblinAction   ACTION;
    public  GameObject     WEAPON;

    private const
            int            HIT_TYPES_COUNT      = 2;

    // Components.
    private Animator       animator;
    private CreatureHealth health;
    private GoblinFight    fight;
    // Information for remembering at the start.
    private Vector3        startPoint;
    // Stealing.
    private StealingAction stealingAction;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
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

    override protected void Run()
    {
        agent.speed = SPEED * ACCELERATION;
        animator.SetFloat("Speed", agent.speed);
    }

    override protected void Walk()
    {
        agent.speed = SPEED;
        animator.SetFloat("Speed", agent.speed);
    }

    override protected void Stand()
    {
        agent.speed = 0;
        animator.SetFloat("Speed", agent.speed);
    }

    private void Steal()
    {
        if (stealingAction == null || stealingAction.House == null)
        {
            target = gameManager.HouseToSteal;

            if (target)
                stealingAction = new StealingAction(target);
            else
            {
                target = gameManager.Player;
                ACTION = GoblinAction.ATTACKING_PLAYER;
            }
        }

        switch (stealingAction.NextState())
        {
            case StealingState.BRAKE_DOOR:
            {
                if (!target || target != stealingAction.Door)
                {
                    target = stealingAction.Door;
                    SetAsAgentTarget(target.transform.position);
                    Run();
                }
                else if (IsTargetAtAttackDistance())
                {
                    Stand();
                    RotateTo(target.transform.position);
                    fight.Attack();
                }
            } break;

            case StealingState.STEAL_STUFF:
            {
                if (!target || !target.CompareTag("Stuff") || !target.transform.IsChildOf(stealingAction.House.transform))
                {
                    target = stealingAction.StuffPeace;
                    SetAsAgentTarget(target.transform.position);
                    Run();
                }
                else if (IsTargetAtAttackDistance())
                {
                    print("attack dist");
                    Stand();
                    RotateTo(target.transform.position);
                }
            } break;

            default:
                print("error");
                break;
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
            target = gameManager.House;

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
