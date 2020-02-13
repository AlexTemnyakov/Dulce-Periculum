using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StealingState
{ 
    START,
    BRAKE_DOOR,
    STEAL_STUFF
}

public class StealingAction
{
    private BehaviourTreeSelector behaviour = new BehaviourTreeSelector();
    private bool                  completed = false;
    private StealingState         state     = StealingState.START;
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
                return BehaviourTreeNodeStatus.FAILURE;

            if (door)
            {
                state = StealingState.BRAKE_DOOR;
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
                return BehaviourTreeNodeStatus.FAILURE;

            if (stuff.Count > 0)
            {
                state = StealingState.STEAL_STUFF;
                return BehaviourTreeNodeStatus.SUCCESS;
            }
            else
            {
                return BehaviourTreeNodeStatus.FAILURE;
            }
        }));
    }

    public StealingState NextState()
    {
        behaviour.Execute();
        return state;
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
