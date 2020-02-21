using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class VillagerBrains : CreatureBrains
{
    public float           RUN_AWAY_TIME;
    public float           SAFE_DISTANCE;

    private bool           initialized        = false;
    private bool           runningAway        = false;
    private float          runAwayTimeCurrent = 0;
    private Animator       animator;
    private VillagerHealth health; 
    private GameObject     basePoint;
    private CompositeBT    behavior;
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
        /*if (!initialized)
            return;

        behavior.Execute();*/
    }

    void FixedUpdate()
    {
        if (!initialized)
            return;

        behavior.Execute();
    }

    public void Initialize()
    {
        SelectorBT subtree;

        behavior = new SelectorBT();
        subtree  = new SelectorBT();

        subtree.AddNode(CreateRunAwaySubtree());
        subtree.AddNode(CreateWanderNode());

        behavior.AddNode(CreateDieNode());
        behavior.AddNode(new InverterBT(CreateWaitNode()));
        behavior.AddNode(subtree);

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

    private IEnumerator IsEnemyNear()
    {
        for (int i = gameManager.Enemies.Count - 1; i >= 0; i--)
        {
            if (gameManager.Enemies.Count <= 0)
                yield break;
            if (!gameManager.Enemies[i] || !gameManager.Enemies[i].activeInHierarchy)
                continue;

            if (Vector3.Distance(transform.position, gameManager.Enemies[i].transform.position) <= VISIBILITY)
            {
                closeEnemy = gameManager.Enemies[i];
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
            if (!health.IsAlive)
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

    private ActionBT CreateNewRunAwayPointNode()
    {
        return new ActionBT(() =>
        {
            StartCoroutine(IsEnemyNear());

            if (closeEnemy)
            {
                Vector3 dir;

                runAwayTimeCurrent = RUN_AWAY_TIME;

                dir = transform.position - closeEnemy.transform.position;

                if (Vector3.Distance(agent.destination, closeEnemy.transform.position) < SAFE_DISTANCE)
                {
                    targetPoint   = transform.position + dir.normalized * (SAFE_DISTANCE * 1.5f);
                    targetPoint.y = Utils.GetTerrainHeight(targetPoint);
                    SetAsAgentTarget(targetPoint);
                    targetPoint   = agent.destination;
                }

                return NodeStatusBT.SUCCESS;
            }
            else
            {
                //return NodeStatusBT.FAILURE;
                return NodeStatusBT.SUCCESS;
            }
        });
    }

    private ActionBT CreateRunAwayNode()
    {
        return new ActionBT(() =>
        {
            // If runAwayTimeCurrent > 0 and 
            // if the distance to the destination > NEW_TARGET_DIST * 2,
            // it means that he is running away now.
            // So run to the destination.
            if (runAwayTimeCurrent > 0)
            {
                //runAwayTimeCurrent -= Time.deltaTime;

                if (agent.remainingDistance > NEW_TARGET_DIST * 2)
                {
                    Run();
                    return NodeStatusBT.RUNNING;
                }
                else
                {
                    return NodeStatusBT.SUCCESS;
                }
            }
            else
            {
                //return NodeStatusBT.FAILURE;
                return NodeStatusBT.SUCCESS;
            }
        });
    }

    private SequenceBT CreateRunAwaySubtree()
    {
        SequenceBT ret;

        ret = new SequenceBT();

        ret.AddNode(CreateNewRunAwayPointNode());
        ret.AddNode(new InverterBT(CreateRunAwayNode()));

        return ret;
    }

    private ActionBT CreateWanderNode()
    {
        return new ActionBT(() =>
        {
            if (runAwayTimeCurrent > 0)
                runAwayTimeCurrent -= Time.deltaTime;

            // Near to the destination, create a new destination.
            if (agent.remainingDistance < 1)
            {
                targetPoint = CreateTargetPoint();

                // If this villager hasn't run away and it is to far from the base, go to to base. Otherwise go to the created point.
                if (runAwayTimeCurrent <= 0 && Vector3.Distance(targetPoint, basePoint.transform.position) > MAX_DIST_FROM_START)
                {
                    targetPoint = basePoint.transform.position;
                }

                SetAsAgentTarget(targetPoint);
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
