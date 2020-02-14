using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealingHandler
{
    private GameObject       building = null;
    private GameObject       door     = null;
    private List<GameObject> stuff    = new List<GameObject>();

    public StealingHandler(GameObject __building)
    {
        building = __building;

        foreach (Transform child in building.transform)
        {
            if (child.CompareTag("Interactable"))
                if (child.GetComponent<Door>())
                    door = child.gameObject;
            if (child.CompareTag("Stuff"))
                stuff.Add(child.gameObject);
        }
    }

    public void Finish()
    {
        building.GetComponent<House>().Stealed = true;
    }

    public GameObject Door
    {
        get
        {
            return door;
        }
    }

    public GameObject StuffPeace
    {
        get
        {
            for (int i = stuff.Count - 1; i >= 0; i--)
            {
                if (!stuff[i] || !stuff[i].transform.IsChildOf(building.transform))
                    stuff.RemoveAt(i);
                else
                    return stuff[i];
            }

            return null;
        }
    }
}