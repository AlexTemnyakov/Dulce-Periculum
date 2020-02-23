using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagersManager : MonoBehaviour
{
    private GameObject       basePoint;
    private List<GameObject> villagers   = new List<GameObject>();

    void Update()
    {
        StartCoroutine(CheckVillagers());
    }

    public void Initialize(VillagerSquadData data)
    {
        transform.tag = "Villagers Manager";
        basePoint     = data.basePoint;

        basePoint.transform.parent = transform;

        for (int i = 0, j = 0; i < data.startCountOfVillagers; i = i + 1, j = (j + 1) % data.villagersPrefabs.Length)
        {
            Vector3        position, shift;
            GameObject     instance;
            VillagerBrains brains;

            shift      = Quaternion.Euler(0, Random.Range(0, 360), 0) * new Vector3(1, 0, 1) * 10;
            position   = basePoint.transform.position + shift;
            position.y = Utils.GetTerrainHeight(position.x, position.z);

            instance                  = Instantiate(data.villagersPrefabs[j], position, Quaternion.identity);
            instance.transform.parent = transform;

            brains           = instance.GetComponent<VillagerBrains>();
            brains.BasePoint = basePoint;

            brains.Initialize();

            villagers.Add(instance);
        }
    }

    private IEnumerator CheckVillagers()
    {
        if (villagers.Count == 0)
        {
            Destroy(gameObject);
            yield break;
        }

        for (int i = villagers.Count - 1; i >= 0; i--)
        {
            if (!villagers[i] || !villagers[i].activeInHierarchy)
            {
                villagers.RemoveAt(i);
            }

            yield return null;
        }
    }

    public List<GameObject> Villagers
    {
        get
        {
            return villagers;
        }
    }

    public GameObject BasePoint
    {
        get
        {
            return basePoint;
        }
    }
}
