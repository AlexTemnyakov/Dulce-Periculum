using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class GoblinBrains : EnemyBrains
{
    public  GoblinAction   action;
    public  GameObject     weapon;
    public  GameObject     runAwayPoint;

    private const
            int            HIT_TYPES_COUNT      = 2;

    // Components.
    //private GoblinsManager goblinsManager;
    private Animator       animator;
    private CreatureHealth health;
    private GoblinFight    fight;
    // Information for remembering at the start.
    private Vector3        startPoint;
    // Stealing.
    private StealingAction stealingAction;

    void Start()
    {
        //goblinsManager = gameObject.GetComponentInParent<GoblinsManager>();
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

        if (action == GoblinAction.RUNNING_AWAY)
        {
            RunAway();
        }
        else if (IsPlayerAtAttackDistance())
        {
            AttackPlayer();
        }
        else if(action == GoblinAction.STEALING)
        {
            Steal();
        }
        else if (action == GoblinAction.ATTACKING_VILLAGE)
        {
            AttackVillage();
        }
        else if (action == GoblinAction.ATTACKING_PLAYER)
        {
            GoToPlayer();
        }

        /*if (IsPlayerAtAttackDistance())
        {
            AttackPlayer();
        }
        else
        {
            if (action == GoblinAction.STEALING)
            {
                Steal();
            }
            else if (action == GoblinAction.ATTACKING_VILLAGE)
            {
                AttackVillage();
            }
            else if (action == GoblinAction.ATTACKING_PLAYER)
            {
                GoToPlayer();
            }
        }*/
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
            {
                stealingAction = new StealingAction(target);
            }
            else
            {
                action = GoblinAction.RUNNING_AWAY;
                return;
            }
        }

        switch (stealingAction.NextState())
        {
            case StealingState.BREAK_DOOR:
            {
                if (target != stealingAction.Door)
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
                if (target != stealingAction.StuffPeace)
                {
                    target = stealingAction.StuffPeace;
                    SetAsAgentTarget(target.transform.position);
                    Run();
                }
                else if (IsTargetAtAttackDistance())
                {
                    Stand();
                    RotateTo(target.transform.position);
                    Destroy(target);
                }
            } break;

            default:
                print("Not enter/Run away");
                stealingAction.Finish();
                stealingAction = null;
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

    private void RunAway()
    {
        if (target != runAwayPoint)
        {
            target = runAwayPoint;
            SetAsAgentTarget(runAwayPoint.transform.position);
        }

        Run();
    }
}
