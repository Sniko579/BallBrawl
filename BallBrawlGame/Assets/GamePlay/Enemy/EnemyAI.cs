using System.Collections.Generic;
using CustomBehavior;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAI : MonoBehaviour, IPVP, IGlobalData
{
    [Header("Requires")]
    [SerializeField] GlobalVariables Sc_GlobalVariables;
    [SerializeField] Transform[] EnemyGoals;
    public static Rigidbody2D RB;

    private TreeManager behaiorTree;

    [Header("Follow Target")]
    [SerializeField] Transform Target;
    [SerializeField] float AttackDistance;
    [SerializeField] float FollowDistance;
    [SerializeField] LayerMask GoalsLayer;
    //private
    private Vector2[] _targetPoints = new Vector2[3];

    private Vector2 _currentTarget;



    #region Movement Var

    float _moveSpeed;
    float _acceleration;

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

    bool _canDash, _stopDash;
    float _dashTimer;
    float _dashCoolDownTimer;
    Vector2 _dashAttackDirection;
    Vector2 _dashDirection;

    public static bool IsDashing;
    #endregion

    #region Bounceness System

    float _reflectionPower;
    ReflectionBody s_ReflectionBody;

    #endregion

    #region Gravity
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
    }

    private void OnValidate()
    {
        setTargetPoints();
    }

    private void Start()
    {
        setTargetPoints();
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
        setTargetPoints();

    }


    #region Actions
    private NodeState FollowTarget()
    {
        if (Vector2.Distance(transform.position, _currentTarget) < FollowDistance)
        {
            _targetMove.x = NearestTargetPoint().x * _moveSpeed;
            RB.linearVelocityX = s_ForceMove.GetForceByT(_targetMove, _acceleration).x;
        }
        else
        {
            DashHandler(NearestTargetPoint());
        }

        return NodeState.Running;
    }

    private NodeState Shoot()
    {
        DashHandler(_dashAttackDirection);

        return NodeState.Running;
    }


    #endregion


    #region Conditions 


    private bool IsInAttackRange()
    {
        _dashAttackDirection = NormalDir(Target.position); // For Dash !
        return Vector2.Distance(transform.position, Target.position) < AttackDistance;
    }

    private bool FoundShootPoint()
    {
        Vector2 normal = (transform.position - Target.position).normalized;

        //_dashAttackDirection = Vector2.Reflect(NormalDir(Target.position), normal);


        Debug.Log(_dashAttackDirection);


        RaycastHit2D raycast = Physics2D.Raycast(Target.position, _dashAttackDirection, 10f, GoalsLayer);
        return true;
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



        // Main Root
        var MainRoot = new Selector(new List<Node>
        {
            ShootSequence,
            new ActionNode(FollowTarget)
        });

        // Apply Data
        behaiorTree = new TreeManager(MainRoot);
    }


    // Maths 🧮 :
    private Vector2 NormalDir(Vector2 _target)
    {
        Vector2 result;

        _currentTarget = _target;

        result.x = transform.position.x < _target.x ? 1 : 0;
        result.y = transform.position.y < _target.y ? 1 : 0;
        result.x = transform.position.x > _target.x ? -1 : result.x;
        result.y = transform.position.y > _target.y ? -1 : result.y;

        return result;
    }
    private Vector2 NearestTargetPoint()
    {
        float smalest = Vector2.Distance(transform.position, _targetPoints[0]);
        Vector2 nearestPoint = _targetPoints[0];
        for (int i = 1; i < _targetPoints.Length; i++)
        {
            if (Vector2.Distance(transform.position, _targetPoints[i]) < smalest)
            {
                smalest = Vector2.Distance(transform.position, _targetPoints[i]);
                nearestPoint = _targetPoints[i];
            }
        }

        return NormalDir(nearestPoint);
    }
    private Vector2 NearsetGoal(Transform[] goals)
    {
        Vector2 nearestGoal = goals[0].position;
        for (int i = 1; i < goals.Length; i++)
        {
            float a = Vector2.Distance(nearestGoal, Target.position);
            float b = Vector2.Distance(Target.position, goals[i].position);
            if (a > b)
            {
                nearestGoal = goals[i].position;
            }
        }

        return nearestGoal;
    }


    // Handler :
    private void DashHandler(Vector2 direction)
    {
        if (_dashTimer > 0)
        {
            _dashTimer -= Time.fixedDeltaTime;
            IsDashing = true;

            _targetMove = direction * _dashSpeed;
            RB.linearVelocity = s_ForceMove.GetForceByT(_targetMove, _dashAcceleration);
        }
        else if (_dashCoolDownTimer > 0)
        {
            _dashCoolDownTimer -= Time.fixedDeltaTime;
            IsDashing = false;

            _targetMove.x = Vector2.one.x * (NormalDir(Target.position).x * 3);
            _targetMove.y = Vector2.one.y;
            RB.linearVelocity = s_ForceMove.GetForceByT(_targetMove, _dashDeceleration);
        }
        else
        {
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



    //intitializing
    private void setTargetPoints()
    {
        Vector2 n = new Vector2(1, 0).normalized * (AttackDistance - 0.3f);
        Vector2 n1 = new Vector2(1, 1).normalized * (AttackDistance - 0.3f);
        Vector2 n2 = new Vector2(1, -1).normalized * (AttackDistance - 0.3f);
        /////////////////                               ///////////////////////
        _targetPoints[0] = new Vector2(Target.position.x, Target.position.y) + n;
        _targetPoints[1] = new Vector2(Target.position.x, Target.position.y) + n1;
        _targetPoints[2] = new Vector2(Target.position.x, Target.position.y) + n2;
    }




    // Unity Functions
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _targetMove = s_ReflectionBody.ReturnForce(collision, _reflectionPower);
        RB.linearVelocity = _targetMove;

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackDistance);

        Gizmos.color = Color.blue;
        foreach (var point in _targetPoints)
            Gizmos.DrawSphere(point, 0.2f);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(Target.position, NearsetGoal(EnemyGoals));


    }

}


public struct Points
{
    public Vector2[] AttackPoints { private set; get; }
    public Vector2[] FollowPoints { private set; get; }
    
    public void SetAttackPoints(Vector2[] points)
    {
        AttackPoints = points;
    }
    public void SetFollowPoints(Vector2[] points)
    {
       FollowPoints = points;
    }

    public Vector2 Target => CurrentTarget();

    Vector2 CurrentTarget()
    {

        return Target;
    }


}
