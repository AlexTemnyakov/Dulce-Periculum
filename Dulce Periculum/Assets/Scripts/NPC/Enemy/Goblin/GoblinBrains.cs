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
    private StealingHandler stealingAction = null;
    //Behaviour.
    private CompositeBT    behaviour      = null;  
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
        type = __type;
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
                SelectorBT subnodeA = new SelectorBT(), 
                           subnodeB = new SelectorBT();
                behaviour = new SelectorBT();

                subnodeA.AddNode(CreateAttackPlayerNode());
                subnodeA.AddNode(CreateStealNode());
                subnodeA.AddNode(CreateRunAwayNode());

                behaviour.AddNode(CreateDieNode());
                behaviour.AddNode(CreateWaitNode());
                behaviour.AddNode(subnodeA);
                behaviour.AddNode(subnodeB);
                //behaviour.AddNode(CreateAttackPlayerNode());
                //behaviour.AddNode(CreateStealNode());
                //behaviour.AddNode(CreateRunAwayNode());
                break;
        }
        initialized = true;
    }

    public void Initialize(CompositeBT __behaviour)
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

    protected void AttackPlayer()
    {
        Stand();
        RotateTo(gameManager.Player.transform.position);
        fight.Attack();
    }

    protected void GoToPlayer()
    {
        target = gameManager.Player;
        SetAsAgentTarget(gameManager.Player.transform.position);
        Run();
    }

    protected void RunAway()
    {
        if (target != runAwayPoint)
        {
            target = runAwayPoint;
            SetAsAgentTarget(runAwayPoint.transform.position);
        }
        else
            Run();
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

    public SequenceBT CreateStealNode()
    {
        SequenceBT ret = new SequenceBT();

        ret.AddNode(new ActionBT(() =>
        {
            if (stealingAction == null)
            {
                GameObject __target = gameManager.HouseToSteal;
                if (__target)
                {
                    target         = __target;
                    stealingAction = new StealingHandler(target);
                    action         = GoblinAction.STEALING;
                    return NodeStatusBT.SUCCESS;
                }
                else
                {
                    action = GoblinAction.RUNNING_AWAY;
                    return NodeStatusBT.FAILURE;
                }
            }
            else
            {
                return NodeStatusBT.SUCCESS;
            }
        }));

        ret.AddNode(new ActionBT(() =>
        {
            if (stealingAction.Door)
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

                return NodeStatusBT.RUNNING;
            }
            else
            {
                return NodeStatusBT.SUCCESS;
            }
        }));

        ret.AddNode(new ActionBT(() =>
        {
            if (stealingAction.StuffPeace)
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

                return NodeStatusBT.RUNNING;
            }
            else
            {
                print("No stuff, finish");
                stealingAction.Finish();
                stealingAction = null;
                Stand();
                agent.ResetPath();
                return NodeStatusBT.SUCCESS;
            }
        }));

        return ret;
    }

    public ActionBT CreateRunAwayNode()
    {
        return new ActionBT(() =>
        {
            action = GoblinAction.RUNNING_AWAY;
            if (Vector3.Distance(transform.position, runAwayPoint.transform.position) <= 10)
            {
                return NodeStatusBT.FAILURE;
            }
            else
            {
                RunAway();
                return NodeStatusBT.RUNNING;
            }
        });
    }

    public ActionBT CreateAttackPlayerNode()
    {
        return new ActionBT(() =>
        {
            if (action == GoblinAction.RUNNING_AWAY)
                return NodeStatusBT.FAILURE;
            else
            {
                if (IsPlayerAtAttackDistance())
                {
                    AttackPlayer();
                    return NodeStatusBT.RUNNING;
                }
                else
                    return NodeStatusBT.FAILURE;
            }
        });
    }

    public ActionBT CreateGoToPlayerNode()
    {
        return new ActionBT(() =>
        {
            if (action == GoblinAction.RUNNING_AWAY)
                return NodeStatusBT.FAILURE;
            else
            {
                GoToPlayer();
                return NodeStatusBT.RUNNING;
            }
        });
    }

    /*public ActionBT CreateWanderNode()
    {
        return new ActionBT(() =>
        {
            state = GoblinAction.WANDERING;
            if (agent.remainingDistance < 1)
            {

            }
        });
    }*/

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
