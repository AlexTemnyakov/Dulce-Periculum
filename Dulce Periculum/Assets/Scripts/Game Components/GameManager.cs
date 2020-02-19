﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameObject       player;
    private List<GameObject> houses            = new List<GameObject>();
    private List<GameObject> housesToSteal     = new List<GameObject>();
    private List<GameObject> enemies           = new List<GameObject>();
    private List<GameObject> goblinsManagers   = new List<GameObject>();
    private List<GameObject> villagersManagers = new List<GameObject>();

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // Find all buildings in the village.
        houses.AddRange(GameObject.FindGameObjectsWithTag("Village"));
        foreach (GameObject g in houses)
            if (g.GetComponent<House>())
                housesToSteal.Add(g);
        // Find all enemies.
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Goblin"));
        goblinsManagers.AddRange(GameObject.FindGameObjectsWithTag("Goblins Manager"));
        villagersManagers.AddRange(GameObject.FindGameObjectsWithTag("Villagers Manager"));

        if (!player)
            Debug.LogError("There is no object with the tag Player");
    }

    void Update()
    {
        StartCoroutine(CheckEnemies());
        StartCoroutine(CheckHousesToSteal());
    }

    private IEnumerator CheckHouses()
    {
        for (int i = houses.Count - 1; i >= 0; i--)
        {
            if (!houses[i] || !houses[i].activeInHierarchy)
            {
                houses.RemoveAt(i);
            }

            yield return null;
        }
    }

    private IEnumerator CheckHousesToSteal()
    {
        for (int i = housesToSteal.Count - 1; i >= 0; i--)
        {
            if (i >= housesToSteal.Count)
                break;

            if (!housesToSteal[i] || !housesToSteal[i].activeInHierarchy || housesToSteal[i].GetComponent<House>().Stealed)
            {
                housesToSteal.RemoveAt(i);
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

    public GameObject House
    {
        get
        {
            if (houses == null || houses.Count == 0)
                return null;

            return houses[Random.Range(0, houses.Count - 1)];
        }
    }

    public GameObject HouseToSteal
    {
        get
        {
            if (housesToSteal == null || housesToSteal.Count == 0)
                return null;

            return housesToSteal[Random.Range(0, housesToSteal.Count)];
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

    public List<GameObject> GoblinsManagers
    {
        get
        {
            return goblinsManagers;
        }
    }

    public List<GameObject> VillagersManager
    {
        get
        {
            return villagersManagers;
        }
    }
}
