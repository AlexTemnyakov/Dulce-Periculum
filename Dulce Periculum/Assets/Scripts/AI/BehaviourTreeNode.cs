using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviourTreeNode
{
    /*protected Func<BehaviourTreeNodeStatus> action;

    public BehaviourTreeNode(Func<BehaviourTreeNodeStatus> __action)
    {
        action = __action;
    }

    public BehaviourTreeNodeStatus Execute()
    {
        return action.Invoke();
    }*/

    public abstract BehaviourTreeNodeStatus Execute();
}
