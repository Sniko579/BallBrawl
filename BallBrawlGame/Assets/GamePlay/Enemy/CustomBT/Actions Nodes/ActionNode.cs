using System;
using CustomBehavior;
using UnityEngine;

public class ActionNode : Node
{
    private Func<NodeState> _action;
    
    public ActionNode(Func<NodeState> action)
    {
        _action = action;
    }

    public override NodeState Evaluate()
    {
        state = _action.Invoke();
        return state;
    }
}
