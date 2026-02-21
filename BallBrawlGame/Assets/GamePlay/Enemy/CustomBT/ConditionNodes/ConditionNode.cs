using System;
using System.Collections.Generic;
using CustomBehavior;
using Unity.VisualScripting;
using UnityEngine;

public class ConditionNode : Node
{
    private Func<bool> _condition;

    public ConditionNode(Func<bool> condition)
    {
        _condition = condition;
    }

    public override NodeState Evaluate()
    {
        state = _condition.Invoke() ? NodeState.Success : NodeState.Fail;
        return state;
    }

}
