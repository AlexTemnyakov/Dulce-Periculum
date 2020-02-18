using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GoblinGenerationData
{
    public GoblinType type;
    public GameObject prefab;
    public int        count;
}

public class GoblinsManager : MonoBehaviour
{
    const   int              MAX_COUNT_OF_GOBLINS = 10;

    public  GoblinGenerationData[] 
                             GOBLINS_TO_GENERATE;
    public  GameObject       BASE_POINT;

    private List<GameObject> goblins               = new List<GameObject>();

    void Awake()
    {
        for (int i = 0; i < GOBLINS_TO_GENERATE.Length; i++)
        {
            for (int j = 0; j < GOBLINS_TO_GENERATE[i].count; j++)
            {
                Vector3      position, shift;
                GameObject   instance;
                GoblinBrains brains;

                shift      = Quaternion.Euler(0, Random.Range(0, 360), 0) * new Vector3(1, 0, 1) * Random.Range(10, 20);
                position   = transform.position + shift;
                position.y = Utils.GetTerrainHeight(position.x, position.z);

                instance                  = Instantiate(GOBLINS_TO_GENERATE[i].prefab, position, Quaternion.identity);
                instance.transform.parent = transform;

                brains           = instance.GetComponent<GoblinBrains>();
                brains.BasePoint = BASE_POINT;

                brains.Initialize(GOBLINS_TO_GENERATE[i].type);
            }
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
