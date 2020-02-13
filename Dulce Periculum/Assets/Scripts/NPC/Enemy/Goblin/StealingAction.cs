using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StealingState
{ 
    START,
    OPEN_DOOR,
    STEAL_STUFF
}

public class StealingAction
{
    private bool          completed;
    private StealingState state;
    private GameObject    building;
    private GameObject    door;

    public StealingAction(GameObject __building)
    {
        completed = false;
        state     = StealingState.START;
        building  = __building;
        door      = null;

        foreach (Transform child in building.transform)
        {
            if (child.CompareTag("Door"))
                door = child.gameObject;
        }
    }

    /*public StealingState NextState()
    {
        if (state == StealingState.START)
        {
            state = StealingState.OPEN_DOOR;
        }
        if (state == StealingState.OPEN_DOOR)
        {

        }
    }*/

    public GameObject Building
    {
        get
        {
            return building;
        }
    }

    public StealingState State
    {
        get
        {
            return state;
        }
        set
        {
            state = value;
        }
    }

    public GameObject Door
    {
        get
        {
            return door;
        }
    }
}
