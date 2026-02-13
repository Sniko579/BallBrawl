using UnityEngine;

namespace BehaviourTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;
        void Start()
        {
            _root = SetupTree();

        }
        private void Update()
        {
            if (_root != null)
            {
                _root.Evaluate();
            }
        }


        protected abstract Node SetupTree();
    }


}
