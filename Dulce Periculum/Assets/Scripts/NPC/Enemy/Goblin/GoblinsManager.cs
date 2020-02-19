using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinsManager : MonoBehaviour
{
    private GameObject       basePoint = null;
    private List<GameObject> goblins   = new List<GameObject>();

    void Update()
    {
        StartCoroutine(CheckGoblins());
    }

    public void Initialize(GoblinSquadData data)
    {
        transform.position         = data.basePoint.transform.position;
        basePoint                  = data.basePoint;
        basePoint.transform.parent = transform;

        InstantiateGoblins(data.attackersCount, data.attackerPrefab, GoblinType.ATTACKER);
        InstantiateGoblins(data.stealersCount, data.stealerPrefab, GoblinType.STEALER);
        InstantiateGoblins(data.baseDefendersCount, data.baseDefenderPrefab, GoblinType.BASE_DEFENDER);
    }

    private IEnumerator CheckGoblins()
    {
        if (goblins.Count == 0)
        {
            Destroy(gameObject);
            yield break;
        }

        for (int i = goblins.Count - 1; i >= 0; i--)
        {
            if (!goblins[i] || !goblins[i].activeInHierarchy)
            {
                goblins.RemoveAt(i);
            }

            yield return null;
        }
    }

    private void InstantiateGoblins(int count, GameObject prefab, GoblinType type)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3      position, shift;
            GameObject   instance;
            GoblinBrains brains;

            shift      = Quaternion.Euler(0, Random.Range(0, 360), 0) * new Vector3(1, 0, 1) * Random.Range(10, 20);
            position   = transform.position + shift;
            position.y = Utils.GetTerrainHeight(position.x, position.z);

            instance                  = Instantiate(prefab, position, Quaternion.identity);
            instance.transform.parent = transform;

            brains           = instance.GetComponent<GoblinBrains>();
            brains.BasePoint = basePoint;

            brains.Initialize(type);

            goblins.Add(instance);
        }
    }
}
