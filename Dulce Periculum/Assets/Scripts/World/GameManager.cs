using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject       player;
    private List<GameObject> village;
    private List<GameObject> enemies;

    void Start()
    {
        GameObject[] tmp;

        player  = GameObject.FindGameObjectWithTag("Player");
        // Find all buildings in the village.
        tmp     = GameObject.FindGameObjectsWithTag("Village");
        village = new List<GameObject>(tmp);
        // Find all enemies.
        enemies = new List<GameObject>();
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Goblin"));

        if (!player)
        {
            Debug.LogError("There is no object with the tag Player");
        }
    }

    void Update()
    {
        StartCoroutine(CheckEnemies());
        StartCoroutine(CheckVillage());
    }

    public GameObject GetHouseInVillage()
    {
        if (village == null || village.Count == 0)
            return null;

        return village[Random.Range(0, village.Count)];
    }

    private IEnumerator CheckVillage()
    {
        for (int i = village.Count - 1; i >= 0; i--)
        {
            if (!village[i] || !village[i].activeInHierarchy)
            {
                village.RemoveAt(i);
            }

            yield return null;
        }
    }

    private IEnumerator CheckEnemies()
    {
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (!enemies[i] || !enemies[i].activeInHierarchy)
            {
                enemies.RemoveAt(i);
            }

            yield return null;
        }
    }

    public GameObject Player
    {
        get
        {
            return player;
        }
    }

    public List<GameObject> Enemies
    {
        get
        {
            return enemies;
        }
    }
}
