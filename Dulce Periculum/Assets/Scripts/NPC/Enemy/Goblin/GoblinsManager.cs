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
            Vector3    position, shift;
            GameObject instance;

            shift      = Quaternion.Euler(0, angle * i, 0) * new Vector3(1, 0, 1) * 20;
            position   = transform.position + shift;
            position   = position + Vector3.down * Utils.GetHeight(position);

            instance                  = Instantiate(GOBLIN_PREFAB, position, Quaternion.identity);
            instance.transform.parent = transform;

            instance.GetComponent<GoblinBrains>().action       = GoblinAction.STEALING;
            instance.GetComponent<GoblinBrains>().runAwayPoint = runAwayPoint.transform.position 
                                                               + shift 
                                                               + Vector3.down * Utils.GetHeight(runAwayPoint.transform.position + shift);

            /*// One half attacks the player, other half attacks the village.
            if (i < START_COUNT_OF_GOBLINS / 2)
                instance.GetComponent<GoblinBrains>().ACTION = GoblinAction.ATTACKING_PLAYER;
            else
                instance.GetComponent<GoblinBrains>().ACTION = GoblinAction.ATTACKING_VILLAGE;*/

            goblins.Add(instance);
        }
    }

    void Update()
    {
        CheckGoblins();
    }

    private IEnumerator CheckGoblins()
    {
        for (int i = goblins.Count - 1; i >= 0; i--)
        {
            if (!goblins[i] || !goblins[i].activeInHierarchy)
            {
                goblins.RemoveAt(i);
            }

            yield return null;
        }
    }
}
