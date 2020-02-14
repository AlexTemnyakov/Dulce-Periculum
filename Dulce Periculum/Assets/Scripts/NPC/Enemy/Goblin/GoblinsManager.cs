using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinsManager : MonoBehaviour
{
    public  GameObject       GOBLIN_PREFAB;
    public  int              START_COUNT_OF_GOBLINS;
    public  GameObject       runAwayPoint;

    private const
            int              MAX_COUNT_OF_GOBLINS   = 5;

    private List<GameObject> goblins;

    void Awake()
    {
        int angle = 360 / START_COUNT_OF_GOBLINS;

        if (START_COUNT_OF_GOBLINS > MAX_COUNT_OF_GOBLINS)
            START_COUNT_OF_GOBLINS = MAX_COUNT_OF_GOBLINS;

        goblins = new List<GameObject>();

        for (int i = 0; i < START_COUNT_OF_GOBLINS; i++)
        {
            Vector3      position, shift;
            GameObject   instance;
            GoblinBrains brains;

            shift    = Quaternion.Euler(0, angle * i, 0) * new Vector3(1, 0, 1) * 20;
            position = transform.position + shift;
            position = position + Vector3.down * Utils.GetHeight(position);

            instance                  = Instantiate(GOBLIN_PREFAB, position, Quaternion.identity);
            instance.transform.parent = transform;

            brains              = instance.GetComponent<GoblinBrains>();
            brains.runAwayPoint = runAwayPoint;

            // One half attacks the player, other half attacks the village.
            if (i < START_COUNT_OF_GOBLINS / 2)
                brains.Initialize(GoblinType.ATTACKER);
            else
                brains.Initialize(GoblinType.STEALER);

            goblins.Add(instance);
        }
    }

    void Update()
    {
        StartCoroutine(CheckGoblins());
    }

    private IEnumerator CheckGoblins()
    {
        int              stealersCount    = 0;
        int              runningAwayCount = 0;
        List<GameObject> attackers        = new List<GameObject>();

        for (int i = goblins.Count - 1; i >= 0; i--)
        {
            if (!goblins[i] || !goblins[i].activeInHierarchy)
            {
                goblins.RemoveAt(i);
            }
            else
            {
                if (goblins[i].GetComponent<GoblinBrains>().Type == GoblinType.ATTACKER)
                    attackers.Add(goblins[i]);
                if (goblins[i].GetComponent<GoblinBrains>().Type == GoblinType.STEALER)
                    stealersCount++;
                if (goblins[i].GetComponent<GoblinBrains>().Action == GoblinAction.RUNNING_AWAY)
                    runningAwayCount++;
            }

            yield return null;
        }

        if (stealersCount == runningAwayCount)
        {
            foreach (GameObject g in attackers)
            {
                if (g && g.activeInHierarchy)
                    g.GetComponent<GoblinBrains>().ForceRunAway();

                yield return null;
            }
        }
    }
}
