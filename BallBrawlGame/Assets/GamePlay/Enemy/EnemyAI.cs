using System.Collections.Generic;
using CustomBehavior;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour
{
    [Header("Requires")]
    [SerializeField] GlobalVariables Sc_GlobalVariables;

    public static Rigidbody2D RB;

    private TreeManager behaiorTree;

    [Header("Follow Target")]
    [SerializeField] Transform Target;
    [SerializeField] float ZoneRadius;
    [SerializeField] Transform ShootPoint;


    bool _isGround;
    bool _isInShootPoint;


   
    
    //private
    private Vector2 _targetMove;
    private PhysicForce s_ForceMove;



    float _dashTimer;
    float _dashCoolDownTimer;
    Vector2 _dashAttackDirection;
    Vector2 _dashDirection;
    bool _dashHasFinished;

    public static bool IsDashing;

   

 





    

    private void Start()
    {

        RB = GetComponent<Rigidbody2D>();

        BehaviorInitializing();


    }
    private void Update()
    {
        s_ForceMove.SetCurrentVelocity(RB.linearVelocity);
 

    }
    private void FixedUpdate()
    {
        behaiorTree.Tike();

        GravityHandler();

    }


    #region Actions

    private NodeState FollowNextPoint()
    {
        if (_isGround)
        {
            RB.linearVelocityY = _targetMove.y;
            RB.linearVelocityX = s_ForceMove.GetForceByT(_targetMove, Sc_GlobalVariables.Acceleration).x;
        }
        else
        {
            RB.linearVelocityX = s_ForceMove.GetForceByT(_targetMove, Sc_GlobalVariables.AirAcceleration).x;
        }


        if (_isInShootPoint)
        {
            _targetMove.x = NormalDir(Target.position).x * Sc_GlobalVariables.MoveSpeed;
        }
        else
        {
            
            if (Vector2.Distance(transform.position , Target.position) < ZoneRadius)
            {
                _targetMove.y = Sc_GlobalVariables.MoveSpeed;
            }
            _targetMove.x = NormalDir(ShootPoint.position).x * Sc_GlobalVariables.MoveSpeed;
        }
        



        return NodeState.Running;
    }

    private NodeState DashToNextPoint()
    {
        Debug.Log("DashToNextPoint");
        return NodeState.Running;
    }

    private NodeState Shoot()
    {
        DashHandler(NormalDir(Target.position));
        return NodeState.Running;
    }

    #endregion


    #region Conditions 
    private bool IsFarNextPoint()
    {
        //Debug.Log("IsNextPointDistance");
        return false;
    }

    private bool IsInAttackRange()
    {
        return (_isInShootPoint && Vector2.Distance(transform.position, Target.position) < ZoneRadius || !_dashHasFinished);
    }

    #endregion


    // AGENT 🧠:
    private void BehaviorInitializing()
    {

        var ShootSequence = new Sequence(new List<Node>
        {
            new ConditionNode(IsInAttackRange),
            new ActionNode(Shoot),
        });

        var DashSequence = new Sequence(new List<Node>
        {
            new ConditionNode(IsFarNextPoint),
            new ActionNode(DashToNextPoint),
        });

        var FollowSelector = new Selector(new List<Node>
        {
            DashSequence,
            new ActionNode(FollowNextPoint),
        });


        // Main Root
        var MainRoot = new Selector(new List<Node>
        {
           ShootSequence,
           FollowSelector,

        });

        // Apply Data
        behaiorTree = new TreeManager(MainRoot);
    }



    // Mathematics 🧮 :
    private Vector2 NormalDir(Vector2 _target)
    {
        Vector2 result;

        result.x = transform.position.x < _target.x ? 1 : 0;
        result.y = transform.position.y < _target.y ? 1 : 0;
        result.x = transform.position.x > _target.x ? -1 : result.x;
        result.y = transform.position.y > _target.y ? -1 : result.y;

        return result;
    }

    // Handler :
    private void DashHandler(Vector2 direction)
    {
        if (_dashTimer > 0)
        {
            _dashHasFinished = false;
            _dashTimer -= Time.fixedDeltaTime;
            IsDashing = true;

            _targetMove = direction * Sc_GlobalVariables.DashSpeed;
            RB.linearVelocity = s_ForceMove.GetForceByT(_targetMove, Sc_GlobalVariables.DashAcceleration);
        }
        else if (_dashCoolDownTimer > 0)
        {
            _dashCoolDownTimer -= Time.fixedDeltaTime;
            IsDashing = false;

            _targetMove.x = Vector2.one.x;
            _targetMove.y = Vector2.one.y;
            RB.linearVelocity = s_ForceMove.GetForceByT(_targetMove, Sc_GlobalVariables.DashDeceleration);
        }
        else
        {
            _dashHasFinished = true;
            _dashTimer = Sc_GlobalVariables.DashTime;
            _dashCoolDownTimer = Sc_GlobalVariables.DashCoolDown;
        }

    }
    private void GravityHandler()
    {
        if (IsDashing)
            RB.gravityScale = Sc_GlobalVariables.DashGravity;
        else if (RB.linearVelocityY < -0.1)
            RB.gravityScale = Sc_GlobalVariables.FallGravity;
        else
            RB.gravityScale = Sc_GlobalVariables.NoramlGravity;
    }




    // Unity Functions
    private void OnCollisionEnter2D(Collision2D collision)
    {
   

        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGround = true;
           
        }
        


    }
    private void OnCollisionExit2D(Collision2D collision)
    {

        _isGround = false;


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("ShootPoint"))
        {
            _isInShootPoint = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("ShootPoint"))
        {
            _isInShootPoint = false;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Target.position, ZoneRadius);


    }



}




