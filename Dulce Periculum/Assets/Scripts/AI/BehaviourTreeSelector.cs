using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTreeSelector : BehaviourTreeNode
{
    protected List<BehaviourTreeNode> nodes = new List<BehaviourTreeNode>();

    public BehaviourTreeSelector()
    {

    }

    public BehaviourTreeSelector(List<BehaviourTreeNode> __nodes)
    {
        nodes = __nodes;
    }

    public override BehaviourTreeNodeStatus Execute()
    {
        foreach (BehaviourTreeNode n in nodes)
        {
            BehaviourTreeNodeStatus status = n.Execute();

            if (status == BehaviourTreeNodeStatus.SUCCESS || status == BehaviourTreeNodeStatus.RUNNING)
                return status;
        }

        return BehaviourTreeNodeStatus.FAILURE;
    }

    public void AddNode(BehaviourTreeNode n)
    {
        nodes.Add(n);
    }
}
