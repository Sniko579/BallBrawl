using CustomBehavior;
using UnityEngine;

public class TreeManager
{
    Node _root;
    public TreeManager(Node root)
    {
        _root = root;
    }
    public void Tike()
    {
        _root.Evaluate();
    }
}
