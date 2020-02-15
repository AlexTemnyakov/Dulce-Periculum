using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class VillagerBrains : CreatureBrains
{
    public float           RUN_AWAY_TIME;

    private bool           inittialized       = false;
    private float          runAwayTimeCurrent = 0;
    private Animator       animator;
    private VillagerHealth health; 
    private GameObject     basePoint;
    private CompositeBT    behaviour;
    private Vector3        targetPoint        = Vector3.zero;
    private GameObject     closeEnemy         = null;

    void Start()
    {
        animator    = GetComponent<Animator>();
        health      = GetComponent<VillagerHealth>();
        agent.speed = SPEED;
    }

    void Update()
    {
        if (!inittialized)
            return;

        behaviour.Execute();
    }

    public void Initialize()
    {
        SelectorBT subtree;

        behaviour = new SelectorBT();
        subtree   = new SelectorBT();

        subtree.AddNode(CreateRunAwayNode());
        subtree.AddNode(CreateWanderNode());

        behaviour.AddNode(CreateDieNode());
        behaviour.AddNode(new InverterBT(CreateWaitNode()));
        behaviour.AddNode(subtree);

        inittialized = true;
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

    private IEnumerator IsEnemyNear()
    {
        foreach (GameObject o in gameManager.Enemies)
        {
            if (Vector3.Distance(transform.position, o.transform.position) <= VISIBILITY)
            {
                closeEnemy = o;
                yield break;
            }

            yield return null;
        }

        closeEnemy = null;
    }

    private ActionBT CreateDieNode()
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

    private ActionBT CreateWaitNode()
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

    private ActionBT CreateRunAwayNode()
    {
        return new ActionBT(() =>
        {
            StartCoroutine(IsEnemyNear());

            if (closeEnemy)
            {
                Vector3 dir;

                runAwayTimeCurrent = RUN_AWAY_TIME;
                dir                = transform.position - closeEnemy.transform.position;
                targetPoint        = transform.position + dir.normalized * 5;

                SetAsAgentTarget(targetPoint);
                Run();

                return NodeStatusBT.RUNNING;
            }
            else
            {
                runAwayTimeCurrent = 0;
                return NodeStatusBT.FAILURE;
            }
        });
    }

    /*private ActionBT CreateGoToBaseNode()
    {
        return new ActionBT(() =>
        {
            if (runAwayTimeCurrent <= 0 && Vector3.Distance(transform.position, basePoint.transform.position) > MAX_DIST_FROM_START)
            {
                if (targetPoint != basePoint.transform.position)
                {
                    targetPoint = basePoint.transform.position;
                    SetAsAgentTarget(targetPoint);
                }
                else
                {
                    Run();
                }

                return NodeStatusBT.RUNNING;
            }
            else
            {
                return NodeStatusBT.FAILURE;
            }
        });
    }*/

    private ActionBT CreateWanderNode()
    {
        return new ActionBT(() =>
        {
            if (runAwayTimeCurrent > 0)
                runAwayTimeCurrent -= Time.deltaTime;

            if (agent.remainingDistance < 1)
            {
                Vector3 newTargetPoint = CreateTargetPoint();
                if (runAwayTimeCurrent <= 0 && Vector3.Distance(newTargetPoint, basePoint.transform.position) > MAX_DIST_FROM_START)
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
        });
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
