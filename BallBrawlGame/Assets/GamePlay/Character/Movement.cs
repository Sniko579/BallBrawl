using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public static Rigidbody2D RB;


    [Header("Movement")]
    [SerializeField] float MoveSpeed = 4f;
    [SerializeField] float JumpHeight = 4f;
    [SerializeField] float Acceleration = 30f;
    [SerializeField] float AirAcceleration = 30f;
    [SerializeField] float Deceleration = 30f;



    //privates
    Vector2 _targetMove;

    PhysicForce S_ForceMove;


    float _radius;
    float _rollAngle;

    // static
    public static bool IsGrounded;

    [Header("Dash")]
    [SerializeField] float DashSpeed = 50;
    [SerializeField] float DashTime = 0.5f;
    [SerializeField] float DashCoolDown = 0.5f;
    [SerializeField] float DashAcceleration = 20f;
    [SerializeField] float DashDeceleration = 20f;


    bool _canDash, _stopDash;
    float _dashTimer;
    float _dashCoolDownTimer;
    Vector2 _dashDirection;


    //statics
    public static bool IsDashing;


    [Header("Bounce Setting")]
    [SerializeField] float PowerReflection = 0.4f;

    // privates
    ReflectionBody S_ReflectionBody;

    [Header("Gravity Setting")]
    [SerializeField] float NoramlGravity = 1f;
    [SerializeField] float FallGravity = 1f;
    [SerializeField] float DashGravity = 1f;


    private void Awake()
    {
        _radius = GetComponent<CapsuleCollider2D>().size.x * 0.5f * transform.lossyScale.x;


        RB = GetComponent<Rigidbody2D>();

        S_ReflectionBody = new ReflectionBody();
        S_ForceMove = new PhysicForce();
    }

    void Start()
    {
        _canDash = true;
        _stopDash = false;
    }


    void Update()
    {
        ApplyJump(InputManager.Jump);

        S_ForceMove.SetCurrentVelocity(RB.linearVelocity);
        S_ReflectionBody.SetVelocity(RB.linearVelocity);
    }
    private void FixedUpdate()
    {
        ApplyMovement(InputManager.Movement);
        ApplyDash(InputManager.Movement);

        RollingOnMove();

        CurrentGravity();


    }


    #region Movement 🏃

    private void ApplyMovement(Vector2 input)
    {
        if (IsDashing)
        {
            MoveOnDashAcceleration();
        }
        else if (_stopDash)
        {
            MoveOnDashDeceleration(input);
        }
        else if (input.x != 0)
        {
            MoveOnAcceleration(input);
        }
        else if (IsGrounded)
        {
            MoveOnDeceleration();
        }

    }

    void MoveOnAcceleration(Vector2 input)
    {
        _targetMove.x = input.x * MoveSpeed;

        float _currentAcceleration = IsGrounded ? Acceleration : AirAcceleration;

        RB.linearVelocityX = S_ForceMove.GetXForceByT(_targetMove.x, _currentAcceleration);

    }

    void MoveOnDeceleration()
    {
        _targetMove.x = Vector2.zero.x;
        RB.linearVelocityX = S_ForceMove.GetXForceByT(_targetMove.x, Deceleration);
    }

    void MoveOnDashAcceleration()
    {

        _targetMove = _dashDirection * DashSpeed;
        RB.linearVelocity = S_ForceMove.GetForceByT(_targetMove, DashAcceleration);
    }

    void MoveOnDashDeceleration(Vector2 input)
    {
        _targetMove.x = Vector2.one.x * (input.x * 3); // it'll be Chengable after !
        _targetMove.y = Vector2.zero.y;

        RB.linearVelocity = S_ForceMove.GetForceByT(_targetMove, DashDeceleration);
    }

    #endregion

    // Jump 🦘
    private void ApplyJump(bool input)
    {
        if ((input && IsGrounded) && Mathf.Abs(_targetMove.y) < JumpHeight)
        {
            _targetMove.y = JumpHeight;
            RB.linearVelocityY = S_ForceMove.GetYForce(_targetMove.y);
        }

    }



    // Abilities 💪
    private void ApplyDash(Vector2 input)
    {
        if ((InputManager.Dash && input != Vector2.zero) && _canDash)
        {
            IsDashing = true;
            _canDash = false;
            _dashDirection = input;
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
        }


    }



    // Physical_Funcions 🥎

    void RollingOnMove()
    {
        _rollAngle += RB.linearVelocityX / _radius;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -_rollAngle);
    }




    // Effects 🎇




    // Gravity_Handeler 🍎
    private void CurrentGravity()
    {
        if (IsDashing)
            RB.gravityScale = DashGravity;
        else if (RB.linearVelocityY < -0.1)
            RB.gravityScale = FallGravity;
        else
            RB.gravityScale = NoramlGravity;

    }

    // Check Collisions 
    private void OnCollisionEnter2D(Collision2D collision)
    {

        _dashTimer = -1;
        IsDashing = false;
        _stopDash = false;

        _targetMove = S_ReflectionBody.ReturnForce(collision, PowerReflection);

        if (collision.transform.CompareTag("Ground"))
        {
            IsGrounded = true;
            RB.linearVelocityX = S_ForceMove.GetXForceByT(_targetMove.x, Deceleration);
            RB.linearVelocityY = S_ForceMove.GetYForce(_targetMove.y);
        }
        else
            RB.linearVelocity = S_ForceMove.GetForce(_targetMove);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.transform.CompareTag("Ground"))
        {
            IsGrounded = false;

        }

    }


}
