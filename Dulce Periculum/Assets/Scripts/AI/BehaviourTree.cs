using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree
{
    private BehaviourTreeSelector root;

    public BehaviourTree(BehaviourTreeSelector __root)
    {
        root = __root;
    }
}
