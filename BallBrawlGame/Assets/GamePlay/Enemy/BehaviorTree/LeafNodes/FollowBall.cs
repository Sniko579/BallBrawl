using BehaviourTree;
using UnityEngine;
using UnityEngine.EventSystems;

public class FollowBall : Node
{
    Vector2 _destination;
    Transform _origin;
    
    public FollowBall(Transform orgin , Vector2 destination)
    {
        _origin = orgin;
        _destination = destination;
        
    }

    public override NodeState Evaluate()
    {
        
        _origin.position = Vector2.MoveTowards(_origin.position, _destination, EnemyAI.S_MoveSpeed * Time.deltaTime);
        

        state = NodeState.RUNNING;
        return state;
    }
}
