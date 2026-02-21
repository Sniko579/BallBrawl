using System.Collections.Generic;

namespace CustomBehavior
{
    public enum NodeState
    {
        Fail, Success, Running
    }
    public abstract class Node
    {
        protected NodeState state;

        protected Node parent;

        protected List<Node> children = new List<Node>();
        public Node() { }
        public Node(List<Node> childern)
        {

            foreach (Node child in childern)
            { 
                attach(child);
            }
        }

        private void attach(Node child)
        {
            child.parent = this;
            children.Add(child);
        }


        public NodeState State => state;

        public abstract NodeState Evaluate();

    }

}