using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillagersManager : MonoBehaviour
{
    public GameObject[] villagersPrefabs;
    public GameObject[] spawnPoints;
    public int          startCountOfVillagers;

    private const int MAX_COUNT_OF_VILLAGERS = 10;

    private List<GameObject> villagers;

    void Awake()
    {
        if (startCountOfVillagers > MAX_COUNT_OF_VILLAGERS)
            startCountOfVillagers = MAX_COUNT_OF_VILLAGERS;

        villagers = new List<GameObject>();

        for (int i = 0, j = 0, k = 0; i < startCountOfVillagers; i = i + 1, j = (j + 1) % spawnPoints.Length, k = (k + 1) % villagersPrefabs.Length)
        {
            Vector3        position, shift;
            GameObject     instance;
            VillagerBrains brains;

            shift      = Quaternion.Euler(0, Random.Range(0, 360), 0) * new Vector3(1, 0, 1) * 10;
            position   = spawnPoints[j].transform.position + shift;
            position.y = Utils.GetTerrainHeight(position.x, position.z);

            instance                  = Instantiate(villagersPrefabs[k], position, Quaternion.identity);
            instance.transform.parent = transform;

            brains           = instance.GetComponent<VillagerBrains>();
            brains.BasePoint = spawnPoints[j];

            brains.Initialize();

            villagers.Add(instance);
        }
    }

    void Update()
    {
        StartCoroutine(CheckVillagers());
    }

    private IEnumerator CheckVillagers()
    {
        for (int i = villagers.Count - 1; i >= 0; i--)
        {
            if (villagers.Count <= 0)
                break;

            if (!villagers[i] || !villagers[i].activeInHierarchy)
            {
                villagers.RemoveAt(i);
            }

            yield return null;
        }
    }
}
