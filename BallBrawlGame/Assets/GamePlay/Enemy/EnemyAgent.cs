using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D))]
public class EnemyAgent : MonoBehaviour
{
    Rigidbody2D RB;
    [Header("Target")]
    [SerializeField] Transform m_Target;
    [SerializeField] Transform[] m_Goals;
    private Vector2 _targetGoal;
    private Vector2 _targetDirection;

    [Header("Movement Sitting")]
    [SerializeField] float AttackRange;
    [SerializeField] float MoveSpeed;
    [SerializeField] float JumpHeight;
    [SerializeField] float Accelration;
    [SerializeField] float AirAccelration;
    private bool _isGround;

    //private
    Vector2 _targetMove;

    PhysicForce S_forceMove;
    [Header("Bounce Sitting")]
    [SerializeField][Range(0, 100)] float BouncePower;

    ReflectionBody S_reflectionBody;


    [Header("Dash Sitting")]
    [SerializeField] float DashSpeed = 50;
    [SerializeField] float DashTime = 0.5f;
    [SerializeField] float DashCoolDown = 0.5f;
    [SerializeField] float DashAcceleration = 20f;
    [SerializeField] float DashDeceleration = 20f;

    //private
    bool _canDash, _stopDash;
    float _dashTimer;
    float _dashCoolDownTimer;
    Vector2 _dashDirection;

    bool _dashButton;

    //statics
    public static bool IsDashing;

    private void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        // Dash : 
        _canDash = true;
        _stopDash = false;
        _dashButton = false;
    }
    void Update()
    {
        //S_reflectionBody.SetVelocity(RB.linearVelocity);
        S_forceMove.SetCurrentVelocity(RB.linearVelocity);
    }
    private void FixedUpdate()
    {
        MoveDirection();

        MoveAgent();
        JumpAgent();
        Dash();

        _targetGoal = NearestGoal();
    }

    // Movement
    private void MoveAgent()
    {
        if (IsDashing)
        {
            _targetMove = _dashDirection * DashSpeed;
            RB.linearVelocity = S_forceMove.GetForceByT(_targetMove, DashAcceleration);
        }
        else if (_stopDash)
        {
            _targetMove.x = Vector2.one.x * (_targetDirection.x * 3); // it'll be Chengable after !

            _targetMove.y = Vector2.zero.y;

            RB.linearVelocity = S_forceMove.GetForceByT(_targetMove, DashDeceleration);
        }
        else if (Mathf.Abs(transform.position.x - m_Target.position.x) > AttackRange)
        {
            if (m_Target.position.x > _targetMove.x && _targetMove.y > m_Target.position.y)
            {
                _targetMove.x = Mathf.Abs(_targetDirection.x) * MoveSpeed;
            }
            else
            {
                _targetMove.x = _targetDirection.x * MoveSpeed;
            }
            

            float _currentAcceleration = _isGround ? Accelration : AirAccelration;

            RB.linearVelocityX = S_forceMove.GetXForceByT(_targetMove.x, _currentAcceleration);
        }
        else
        {
            _dashButton = true;
        }

    }

    // Direction
    private void MoveDirection()
    {
        // X
        _targetDirection.x = m_Target.position.x > transform.position.x ? 1 : 0;
        _targetDirection.x = m_Target.position.x < transform.position.x ? -1 : _targetDirection.x;
        // Y
        _targetDirection.y = m_Target.position.y > transform.position.y ? 1 : 0;
        _targetDirection.y = m_Target.position.y < transform.position.y ? -1 : _targetDirection.y;

    }

    // Jump
    private void JumpAgent()
    {
        if (m_Target.position.y > transform.position.y && _isGround)
        {
            _targetMove.y = JumpHeight;
            RB.linearVelocityY = _targetMove.y;
        }
    }


    // Abilities
    private void Dash()
    {
        if (_dashButton && _canDash)
        {
            IsDashing = true;
            _canDash = false;
            _dashDirection = _targetDirection;
            _dashTimer = DashTime;
            _dashCoolDownTimer = DashCoolDown;
        }
        else if (_dashTimer >= 0)
        {
            _dashTimer -= Time.fixedDeltaTime;
            _stopDash = true;
        }
        else if (_dashCoolDownTimer >= 0)
        {
            IsDashing = false;
            _dashCoolDownTimer -= Time.fixedDeltaTime;
        }
        else
        {
            _stopDash = false;
            _canDash = true;
            _dashButton = false;
        }
    }


    // Goal 
    private Vector2 NearestGoal()
    {
        Vector2 nearstVector = m_Goals[0].position;
        float minDistance = Vector2.Distance(m_Target.position, m_Goals[0].position);
        for (int i = 1; i < m_Goals.Length; i++)
        {
            if (Vector2.Distance(m_Target.position, m_Goals[i].position) < minDistance)
            {
                minDistance = Vector2.Distance(m_Target.position, m_Goals[i].position);
                nearstVector = m_Goals[i].position;
            }

        }
        return nearstVector;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isGround = true;

        _targetMove = S_reflectionBody.ReturnForce(collision, BouncePower);
        RB.linearVelocity = _targetMove;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _isGround = false;
    }


}
