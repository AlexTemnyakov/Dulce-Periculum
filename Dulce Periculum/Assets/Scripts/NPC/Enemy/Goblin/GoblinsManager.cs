using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinsManager : MonoBehaviour
{
    public  GameObject       goblinPrefab;
    public  int              startCountOfGoblins;
    public  GameObject       runAwayPoint;

    private const
            int              MAX_COUNT_OF_GOBLINS   = 5;

    private List<GameObject> goblins;

    void Awake()
    {
        int angle = 360 / startCountOfGoblins;

        if (startCountOfGoblins > MAX_COUNT_OF_GOBLINS)
            startCountOfGoblins = MAX_COUNT_OF_GOBLINS;

        goblins = new List<GameObject>();

        for (int i = 0; i < startCountOfGoblins; i++)
        {
            Vector3      position, shift;
            GameObject   instance;
            GoblinBrains brains;

            shift      = Quaternion.Euler(0, angle * i, 0) * new Vector3(1, 0, 1) * 20;
            position   = transform.position + shift;
            position.y = Utils.GetTerrainHeight(position.x, position.z);

            instance                  = Instantiate(goblinPrefab, position, Quaternion.identity);
            instance.transform.parent = transform;

            brains           = instance.GetComponent<GoblinBrains>();
            brains.BasePoint = runAwayPoint;

            // One half attacks the player, other half attacks the village.
            if (i < startCountOfGoblins / 2)
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
        for (int i = goblins.Count - 1; i >= 0; i--)
        {
            if (goblins.Count <= 0)
                break;

            if (!goblins[i] || !goblins[i].activeInHierarchy)
            {
                goblins.RemoveAt(i);
            }

            yield return null;
        }
    }
}
