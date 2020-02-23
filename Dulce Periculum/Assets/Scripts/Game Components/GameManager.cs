using System.Collections;
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
    private InGameGUI        inGameGUI;
    private bool             winMenuShowed     = false;

    void Start()
    {
        Time.timeScale = 1;

        player = GameObject.FindGameObjectWithTag("Player");

        // Find all buildings in the village.
        houses.AddRange(GameObject.FindGameObjectsWithTag("Village"));
        foreach (GameObject g in houses)
            if (g.GetComponent<House>())
                housesToSteal.Add(g);

        // Find all enemies.
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Goblin"));
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Ghost"));
        goblinsManagers.AddRange(GameObject.FindGameObjectsWithTag("Goblins Manager"));

        villagersManagers.AddRange(GameObject.FindGameObjectsWithTag("Villagers Manager"));

        inGameGUI = GetComponentInChildren<InGameGUI>();
    }

    void Update()
    {
        StartCoroutine(CheckGoblinSquads());
        StartCoroutine(CheckHousesToSteal());

        if (goblinsManagers.Count == 0 && !winMenuShowed)
        {
            winMenuShowed = true;
            StartCoroutine(inGameGUI.ShowWinMenu());
        }
        else if (!player)
            inGameGUI.GoToLoseMenu();

        if (Input.GetKeyDown(KeyCode.P))
            inGameGUI.GoToPauseMenu();
    }

    private IEnumerator CheckHousesToSteal()
    {
        if (housesToSteal.Count == 0)
            yield break;

        for (int i = housesToSteal.Count - 1; i >= 0; i--)
        {
            if (!housesToSteal[i] || !housesToSteal[i].activeInHierarchy || housesToSteal[i].GetComponent<House>().Stealed)
            {
                housesToSteal.RemoveAt(i);
            }

            yield return null;
        }
    }

    private IEnumerator CheckEnemies()
    {
        if (enemies.Count == 0)
            yield break;

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (!enemies[i] || !enemies[i].activeInHierarchy)
            {
                enemies.RemoveAt(i);
            }

            yield return null;
        }
    }

    private IEnumerator CheckGoblinSquads()
    {
        if (goblinsManagers.Count == 0)
            yield break;

        for (int i = goblinsManagers.Count - 1; i >= 0; i--)
        {
            if (!goblinsManagers[i] || !goblinsManagers[i].activeInHierarchy)
            {
                goblinsManagers.RemoveAt(i);
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

            return houses[Random.Range(0, houses.Count)];
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
