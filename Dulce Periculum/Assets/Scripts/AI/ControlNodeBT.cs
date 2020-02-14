using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ControlNodeBT : NodeBT
{
    protected List<NodeBT> nodes = new List<NodeBT>();

    public ControlNodeBT()
    {

    }

    public ControlNodeBT(List<NodeBT> __nodes)
    {
        nodes = __nodes;
    }

    public void AddNode(NodeBT n)
    {
        nodes.Add(n);
    }
}
