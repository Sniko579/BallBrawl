using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class EnemyAI : BehaviourTree.Tree
{
    public static Rigidbody S_RB;
    [Header("Target")]
    [SerializeField] Transform m_Target;
    [Header("Movement")]
    [SerializeField] float MoveSpeed;

    public static float S_MoveSpeed;
    private void Start()
    {
        S_RB = GetComponent<Rigidbody>();
    }
    private void OnValidate()
    {
        S_MoveSpeed = MoveSpeed;
    }
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new FollowBall(transform, m_Target.position),
            
        }

        );

        return root;
    }



}
