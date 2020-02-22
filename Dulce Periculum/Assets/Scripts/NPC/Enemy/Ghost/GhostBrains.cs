using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(GhostFight))]
public class GhostBrains : EnemyBrains
{
    // Components.
    private Animator       animator;
    private CreatureHealth health;
    private GhostFight     fight;
    // Information for remembering at the start.
    public GameObject     basePoint;
    // Behaviour.
    private CompositeBT    behavior    = null;
    private bool           initialized = false;

    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameManager>();
        animator    = GetComponent<Animator>();
        health      = GetComponent<CreatureHealth>();
        fight       = GetComponent<GhostFight>();
        agent.speed = SPEED;

        Initialize();
    }

    void FixedUpdate()
    {
        if (!initialized)
            return;

        behavior.Execute();
    }

    public void Initialize()
    {
        behavior = new SelectorBT();

        behavior.AddNode(CreateWanderSubtree());

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

    public ActionBT CreateAttackPlayerNode()
    {
        return new ActionBT(() =>
        {
            if (IsPlayerAtAttackDistance())
            {
                animator.SetBool("Attacking State", true);
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
                animator.SetBool("Attacking State", false);
                RunToBase();
                return NodeStatusBT.RUNNING;
            }
        }));

        ret.AddNode(new InverterBT(new ActionBT(() =>
        {
            if (IsPlayerVisible())
            {
                animator.SetBool("Attacking State", true);
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
}
