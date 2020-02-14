using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CompositeBT : NodeBT
{
    protected List<NodeBT> nodes = new List<NodeBT>();

    public CompositeBT()
    {

    }

    public CompositeBT(List<NodeBT> __nodes)
    {
        nodes = __nodes;
    }

    public void AddNode(NodeBT n)
    {
        nodes.Add(n);
    }
}
