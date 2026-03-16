using System.Collections.Generic;
using CustomBehavior;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour, IPVP, IGlobalData
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


    #region Movement Var

    float _moveSpeed;
    float _acceleration;
    float _airAcceleration;

    float _jumpHeight;
    //private
    private Vector2 _targetMove;
    private PhysicForce s_ForceMove;

    #endregion

    #region Dash Var
    float _dashSpeed = 50;
    float _dashTime = 0.5f;
    float _dashCoolDown = 0.5f;
    float _dashAcceleration = 20f;
    float _dashDeceleration = 20f;

    float _dashTimer;
    float _dashCoolDownTimer;
    Vector2 _dashAttackDirection;
    Vector2 _dashDirection;
    bool _dashHasFinished;

    public static bool IsDashing;

    #endregion

    #region Bounceness System Var

    float _reflectionPower;
    ReflectionBody s_ReflectionBody;

    #endregion

    #region Gravity Var
    float _noramlGravity;
    float _fallGravity;
    float _dashGravity;
    #endregion



    private void OnEnable()
    {
        Sc_GlobalVariables.GlobalDatas.Add(this);
        UpdateVaribles();
    }
    private void OnDisable()
    {
        Sc_GlobalVariables.GlobalDatas.Remove(this);
    }
    public void UpdateVaribles()
    {
        // Dash
        _dashSpeed = Sc_GlobalVariables.DashSpeed;
        _dashTime = Sc_GlobalVariables.DashTime;
        _dashCoolDown = Sc_GlobalVariables.DashCoolDown;
        _dashAcceleration = Sc_GlobalVariables.DashAcceleration;
        _dashDeceleration = Sc_GlobalVariables.DashDeceleration;
        // Gravity
        _noramlGravity = Sc_GlobalVariables.NoramlGravity;
        _fallGravity = Sc_GlobalVariables.FallGravity;
        _dashGravity = Sc_GlobalVariables.DashGravity;
        // Bounceness
        _reflectionPower = Sc_GlobalVariables.ReflectionPower;
        // Movement
        _moveSpeed = Sc_GlobalVariables.MoveSpeed;
        _acceleration = Sc_GlobalVariables.Acceleration;
        _airAcceleration = Sc_GlobalVariables.AirAcceleration;
        _jumpHeight = Sc_GlobalVariables.JumpHeight;
    }



    private void Start()
    {

        RB = GetComponent<Rigidbody2D>();

        BehaviorInitializing();


    }
    private void Update()
    {
        s_ForceMove.SetCurrentVelocity(RB.linearVelocity);
        s_ReflectionBody.SetVelocity(RB.linearVelocity);

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
            RB.linearVelocityX = s_ForceMove.GetForceByT(_targetMove, _acceleration).x;
        }
        else
        {
            RB.linearVelocityX = s_ForceMove.GetForceByT(_targetMove, _airAcceleration).x;
        }


        if (_isInShootPoint)
        {
            _targetMove.x = NormalDir(Target.position).x * _moveSpeed;
        }
        else
        {
            
            if (Vector2.Distance(transform.position , Target.position) < ZoneRadius)
            {
                _targetMove.y = _jumpHeight;
            }
            _targetMove.x = NormalDir(ShootPoint.position).x * _moveSpeed;
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

            _targetMove = direction * _dashSpeed;
            RB.linearVelocity = s_ForceMove.GetForceByT(_targetMove, _dashAcceleration);
        }
        else if (_dashCoolDownTimer > 0)
        {
            _dashCoolDownTimer -= Time.fixedDeltaTime;
            IsDashing = false;

            _targetMove.x = Vector2.one.x;
            _targetMove.y = Vector2.one.y;
            RB.linearVelocity = s_ForceMove.GetForceByT(_targetMove, _dashDeceleration);
        }
        else
        {
            _dashHasFinished = true;
            _dashTimer = _dashTime;
            _dashCoolDownTimer = _dashCoolDown;
        }

    }
    private void GravityHandler()
    {
        if (IsDashing)
            RB.gravityScale = _dashGravity;
        else if (RB.linearVelocityY < -0.1)
            RB.gravityScale = _fallGravity;
        else
            RB.gravityScale = _noramlGravity;
    }




    // Unity Functions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _targetMove = s_ReflectionBody.ReturnForce(collision, _reflectionPower);

        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGround = true;
            RB.linearVelocityX = s_ForceMove.GetForceByT(_targetMove, Sc_GlobalVariables.Deceleration).x;
            RB.linearVelocityY = _targetMove.y;
        }
        else
            RB.linearVelocity = _targetMove;


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




