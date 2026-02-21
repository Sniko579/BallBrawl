using System.Collections.Generic;
using System.Diagnostics;

namespace CustomBehavior
{
    public class Selector : Node
    {
        public Selector(List<Node> children) : base(children)
        {

        } 

        

        int currentChildIndex = 0;
        public override NodeState Evaluate()
        {

            for (int index = currentChildIndex; index < children.Count; index++)
            {
                switch (children[index].Evaluate())
                {
                    case NodeState.Success:
                        currentChildIndex = 0;
                        state = NodeState.Success;
                        return state;

                    case NodeState.Running:
                        state = NodeState.Running;
                        return state;


                }

            }

            currentChildIndex = 0;
            state = NodeState.Fail;
            return state;

        }
    }

}