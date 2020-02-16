using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class GoblinBrains : EnemyBrains
{
    private const int HIT_TYPES_COUNT = 2;

    // Components.
    //private GoblinsManager goblinsManager;
    private Animator        animator;
    private CreatureHealth  health;
    private GoblinFight     fight;
    // Information for remembering at the start.
    private GameObject      basePoint;
    private Vector3         startPoint;
    // Stealing.
    private StealingHandler stealingAction = null;
    //Behaviour.
    private CompositeBT     behavior       = null;  
    private bool            initialized    = false;
    private GoblinType      type           = GoblinType.STEALER;


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

        behavior.Execute();

        /*switch (behaviour.Execute())
        {
            case NodeStatusBT.SUCCESS:
            case NodeStatusBT.RUNNING:
                return;
            case NodeStatusBT.FAILURE:
                return;
        }*/
    }

    public void Initialize(GoblinType __type)
    {
        type = __type;
        switch (type)
        {
            case GoblinType.ATTACKER:
            {
                SequenceBT subtree;

                behavior = new SelectorBT();
                subtree   = new SequenceBT();

                subtree.AddNode(CreateDefeatGoblinsSubtree());
                subtree.AddNode(CreateWanderSubtree());

                behavior.AddNode(CreateDieNode());
                behavior.AddNode(new InverterBT(CreateWaitNode()));
                behavior.AddNode(subtree);
            }
            break;

            case GoblinType.STEALER:
            {
                SequenceBT subtree;

                behavior = new SelectorBT();
                subtree   = new SequenceBT();

                subtree.AddNode(new InverterBT(CreateStealSubtree()));
                subtree.AddNode(CreateWanderSubtree());

                behavior.AddNode(CreateDieNode());
                behavior.AddNode(new InverterBT(CreateWaitNode()));
                behavior.AddNode(subtree);
            }
            break;
        }
        initialized = true;
    }

    public void Initialize(CompositeBT __behaviour)
    {
        type        = GoblinType.CUSTOM;
        behavior   = __behaviour;
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

    protected void RunToBase()
    {
        if (target != basePoint)
        {
            target = basePoint;
            SetAsAgentTarget(basePoint.transform.position);
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
                GetComponent<Inventory>().DropAll();
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
                return NodeStatusBT.SUCCESS;
        });
    }

    public SequenceBT CreateStealSubtree()
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
                    return NodeStatusBT.SUCCESS;
                }
                else
                {
                    return NodeStatusBT.FAILURE;
                }
            }
            else
            {
                return NodeStatusBT.SUCCESS;
            }
        }));

        // A node for attacking the player during stealing.
        // If the player is near, attack him.
        ret.AddNode(new InverterBT(CreateAttackPlayerNode()));

        ret.AddNode(new ActionBT(() =>
        {
            if (stealingAction.Door)
            {
                if (target != stealingAction.Door)
                {
                    target = stealingAction.Door;
                    SetAsAgentTarget(target.transform.position);
                }
                else if (IsTargetAtAttackDistance())
                {
                    Stand();
                    RotateTo(target.transform.position);
                    fight.Attack();
                }
                else
                    Run();

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
                }
                else if (IsTargetAtInteractionDistance())
                {
                    Stand();
                    RotateTo(target.transform.position);
                    GetComponent<Inventory>().Add(target);
                    StartCoroutine(WaitFor(2));
                }
                else
                {
                    Run();
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

    public ActionBT CreateAttackPlayerNode()
    {
        return new ActionBT(() =>
        {
            if (IsPlayerAtAttackDistance())
            {
                AttackPlayer();
                return NodeStatusBT.RUNNING;
            }
            else
                return NodeStatusBT.FAILURE;
        });
    }

    public SequenceBT CreateWanderSubtree()
    {
        SequenceBT ret = new SequenceBT();

        ret.AddNode(new InverterBT(CreateAttackPlayerNode()));

        ret.AddNode(new ActionBT(() =>
        {
            if (Vector3.Distance(transform.position, basePoint.transform.position) <= MAX_DIST_FROM_START)
            {
                return NodeStatusBT.SUCCESS;
            }
            else
            {
                RunToBase();
                return NodeStatusBT.RUNNING;
            }
        }));

        ret.AddNode(new InverterBT(new ActionBT(() =>
        {
            if (IsPlayerVisible())
            {
                GoToPlayer();
                return NodeStatusBT.RUNNING;
            }
            else
            {
                return NodeStatusBT.FAILURE;
            }
        })));

        ret.AddNode(new ActionBT(() =>
        {
            if (agent.remainingDistance < 1)
            {
                Vector3 newTargetPoint = CreateTargetPoint();
                if (Vector3.Distance(newTargetPoint, basePoint.transform.position) > MAX_DIST_FROM_START)
                {
                    SetAsAgentTarget(basePoint.transform.position);
                }
                else
                {
                    SetAsAgentTarget(newTargetPoint);
                }
            }
            else
            {
                Walk();
            }

            return NodeStatusBT.RUNNING;
        }));

        return ret;
    }

    public SelectorBT CreateDefeatGoblinsSubtree()
    {
        SelectorBT ret = new SelectorBT();

        ret.AddNode(new ActionBT(() =>
        {
            GameObject house = gameManager.HouseToSteal;
            if (house)
            {
                return NodeStatusBT.FAILURE;
            }
            else
            {
                return NodeStatusBT.SUCCESS;
            }
        }));

        ret.AddNode(CreateAttackPlayerNode());

        ret.AddNode(new ActionBT(() =>
        {
            GoToPlayer();
            return NodeStatusBT.RUNNING;
        }));

        return ret;
    }

    public GoblinType Type
    {
        get
        {
            return type;
        }
    }

    public GameObject BasePoint
    {
        get
        {
            return basePoint;
        }
        set
        {
            basePoint = value;
        }
    }
}
