using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeAction : BehaviourTreeNode
{
    private Func<BehaviourTreeNodeStatus> action;

    public BehaviourTreeAction(Func<BehaviourTreeNodeStatus> __action)
    {
        action = __action;
    }

    public override BehaviourTreeNodeStatus Execute()
    {
        return action.Invoke();
    }
}
