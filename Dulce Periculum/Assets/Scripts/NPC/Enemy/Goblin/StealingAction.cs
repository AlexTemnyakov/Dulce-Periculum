using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StealingState
{ 
    NONE,
    BREAK_DOOR,
    STEAL_STUFF
}

public class StealingAction
{
    private BehaviourTreeSelector behaviour = new BehaviourTreeSelector();
    private bool                  completed = false;
    private StealingState         state     = StealingState.NONE;
    private GameObject            building  = null;
    private GameObject            door      = null;
    private List<GameObject>      stuff     = new List<GameObject>();

    public StealingAction(GameObject __building)
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

        // Break door node.
        behaviour.AddNode(new BehaviourTreeAction(() =>
        {
            if (completed)
            {
                state = StealingState.NONE;
                return BehaviourTreeNodeStatus.FAILURE;
            }

            if (door && door.activeInHierarchy)
            {
                state = StealingState.BREAK_DOOR;
                return BehaviourTreeNodeStatus.SUCCESS;
            }
            else
            {
                return BehaviourTreeNodeStatus.FAILURE;
            }
        }));

        // Steal stuff node.
        behaviour.AddNode(new BehaviourTreeAction(() =>
        {
            if (completed)
            {
                state = StealingState.NONE;
                return BehaviourTreeNodeStatus.FAILURE;
            }

            if (stuff.Count > 0)
            {
                state = StealingState.STEAL_STUFF;
                return BehaviourTreeNodeStatus.SUCCESS;
            }
            else
            {
                completed = true;
                return BehaviourTreeNodeStatus.FAILURE;
            }
        }));
    }

    public StealingState NextState()
    {
        behaviour.Execute();
        return state;
    }

    public void Finish()
    {
        building.GetComponent<House>().Stealed = true;
    }

    public StealingState State
    {
        get
        {
            return state;
        }
    }

    public GameObject House
    {
        get
        {
            return building;
        }
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
