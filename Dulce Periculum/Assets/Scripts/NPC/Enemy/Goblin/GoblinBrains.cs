using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class GoblinBrains : EnemyBrains
{
    public  GameObject runAwayPoint;

    private const int HIT_TYPES_COUNT = 2;

    // Components.
    //private GoblinsManager goblinsManager;
    private Animator       animator;
    private CreatureHealth health;
    private GoblinFight    fight;
    // Information for remembering at the start.
    private Vector3        startPoint;
    // Stealing.
    private StealingAction stealingAction = null;
    //Behaviour.
    private ControlNodeBT  behaviour      = null;  
    private bool           initialized    = false;
    private GoblinType     type           = GoblinType.STEALER;
    private GoblinAction   action         = GoblinAction.STEALING;


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
        if (!initialized)
            return;

        switch (behaviour.Execute())
        {
            case NodeStatusBT.SUCCESS:
            case NodeStatusBT.RUNNING:
                return;
            case NodeStatusBT.FAILURE:
                return;
        }
    }

    public void Initialize(GoblinType __type)
    {
        type      = __type;
        switch (type)
        {
            case GoblinType.ATTACKER:
                behaviour = new SelectorBT();
                behaviour.AddNode(CreateDieNode());
                behaviour.AddNode(CreateWaitNode());
                behaviour.AddNode(CreateAttackPlayerNode());
                behaviour.AddNode(CreateGoToPlayerNode());
                behaviour.AddNode(CreateRunAwayNode());
                break;

            case GoblinType.STEALER:
                behaviour = new SelectorBT();
                behaviour.AddNode(CreateDieNode());
                behaviour.AddNode(CreateWaitNode());
                behaviour.AddNode(CreateAttackPlayerNode());
                behaviour.AddNode(CreateStealNode());
                behaviour.AddNode(CreateRunAwayNode());
                break;
        }
        initialized = true;
    }

    public void Initialize(ControlNodeBT __behaviour)
    {
        type        = GoblinType.CUSTOM;
        behaviour   = __behaviour;
        initialized = true;
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

    protected NodeStatusBT Steal()
    {
        // Initialize this action.
        if (stealingAction == null || stealingAction.House == null)
        {
            target = gameManager.HouseToSteal;

            if (target)
            {
                stealingAction = new StealingAction(target);
                return NodeStatusBT.SUCCESS;
            }
            else
            {
                action = GoblinAction.RUNNING_AWAY;
                return NodeStatusBT.FAILURE;
            }
        }

        // Complete this action. The action is running.
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
            }
            break;

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
                    StartCoroutine(WaitFor(2));
                }
            }
            break;

            default:
                stealingAction.Finish();
                stealingAction = null;
                Stand();
                agent.ResetPath();
                break;
        }

        return NodeStatusBT.RUNNING;
    }

    protected NodeStatusBT AttackPlayer()
    {
        if (IsPlayerAtAttackDistance())
        {
            Stand();
            RotateTo(gameManager.Player.transform.position);
            fight.Attack();

            return NodeStatusBT.SUCCESS;
        }
        else
        {
            return NodeStatusBT.FAILURE;
        }
    }

    protected NodeStatusBT GoToPlayer()
    {
        target = gameManager.Player;
        SetAsAgentTarget(gameManager.Player.transform.position);
        Run();

        return NodeStatusBT.RUNNING;
    }

    protected NodeStatusBT RunAway()
    {
        if (target != runAwayPoint)
        {
            target = runAwayPoint;
            SetAsAgentTarget(runAwayPoint.transform.position);

            return NodeStatusBT.SUCCESS;
        }
        else if (Vector3.Distance(transform.position, runAwayPoint.transform.position) <= 10)
        {
            //StartWander();

            return NodeStatusBT.RUNNING;
        }
        else
        {
            Run();

            return NodeStatusBT.RUNNING;
        }
    }

    public ActionBT CreateDieNode()
    {
        return new ActionBT(() =>
        {
            if (!health.IsAlive())
            {
                agent.ResetPath();
                Stand();
                return NodeStatusBT.SUCCESS;
            }
            else
                return NodeStatusBT.FAILURE;
        });
    }

    public ActionBT CreateWaitNode()
    {
        return new ActionBT(() =>
        {
            if (waiting)
            {
                Stand();
                return NodeStatusBT.RUNNING;
            }
            else
                return NodeStatusBT.FAILURE;
        });
    }

    public ActionBT CreateStealNode()
    {
        return new ActionBT(() =>
        {
            if (action == GoblinAction.RUNNING_AWAY)
                return NodeStatusBT.FAILURE;
            else
            {
                NodeStatusBT status = Steal();

                switch (status)
                {
                    case NodeStatusBT.SUCCESS:
                    case NodeStatusBT.RUNNING:
                        action = GoblinAction.STEALING;
                        break;
                    case NodeStatusBT.FAILURE:
                        action = GoblinAction.RUNNING_AWAY;
                        break; 
                }

                return status;
            }
        });
    }

    public ActionBT CreateRunAwayNode()
    {
        return new ActionBT(() =>
        {
            action = GoblinAction.RUNNING_AWAY;
            return RunAway();
        });
    }

    public ActionBT CreateAttackPlayerNode()
    {
        return new ActionBT(() =>
        {
            if (action == GoblinAction.RUNNING_AWAY)
                return NodeStatusBT.FAILURE;
            else
                return AttackPlayer();
        });
    }

    public ActionBT CreateGoToPlayerNode()
    {
        return new ActionBT(() =>
        {
            if (action == GoblinAction.RUNNING_AWAY)
                return NodeStatusBT.FAILURE;
            else
                return GoToPlayer();
        });
    }

    public void ForceRunAway()
    {
        action = GoblinAction.RUNNING_AWAY;
    }

    public GoblinType Type
    {
        get
        {
            return type;
        }
    }

    public GoblinAction Action
    {
        get
        {
            return action;
        }
    }
}
