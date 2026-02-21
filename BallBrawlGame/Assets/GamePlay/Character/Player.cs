using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IPVP, IGlobalData
{
    [Header("Require")]
    [SerializeField] GlobalVariables Sc_GlobalVariables;
    public static Rigidbody2D RB;

    #region Movement Var
    
    float _moveSpeed = 4f;
    float _jumpHeight = 4f;
    float _acceleration = 30f;
    float _airAcceleration = 30f;
    float _deceleration = 30f;

    //privates
    Vector2 _targetMove;
    PhysicForce S_ForceMove;

    float _radius;
    float _rollAngle;

    // static
    public static bool IsGrounded;

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
    Vector2 _dashDirection;


    //statics
    public static bool IsDashing;

    #endregion

    #region Bounce Setting

    float _reflectionPower;
    // privates
    ReflectionBody S_ReflectionBody;
   
    #endregion

    #region Gravity Setting
    float _noramlGravity;
    float _fallGravity;
    float _dashGravity;
    #endregion
 
    public void UpdateVaribles()
    {
        // Gravity
        _noramlGravity = Sc_GlobalVariables.NoramlGravity;
        _fallGravity = Sc_GlobalVariables.FallGravity;
        _dashGravity = Sc_GlobalVariables.DashGravity;

        // Dash
        _dashSpeed = Sc_GlobalVariables.DashSpeed;
        _dashTime = Sc_GlobalVariables.DashTime;
        _dashCoolDown = Sc_GlobalVariables.DashCoolDown;
        _dashAcceleration = Sc_GlobalVariables.DashAcceleration;
        _dashDeceleration = Sc_GlobalVariables.DashDeceleration;

        // Bounceness
        _reflectionPower = Sc_GlobalVariables.ReflectionPower;
        // Movement
        _moveSpeed = Sc_GlobalVariables.MoveSpeed;
        _jumpHeight = Sc_GlobalVariables.JumpHeight;
        _acceleration = Sc_GlobalVariables.Acceleration;
        _airAcceleration = Sc_GlobalVariables.AirAcceleration;
        _deceleration = Sc_GlobalVariables.Deceleration;
    }
    private void OnEnable()
    {
        Sc_GlobalVariables.GlobalDatas.Add(this);
        UpdateVaribles();
    }
    private void OnDisable()
    {
        Sc_GlobalVariables.GlobalDatas.Remove(this);
    }

    private void Awake()
    {
        _radius = GetComponent<CapsuleCollider2D>().size.x * 0.5f * transform.lossyScale.x;
        RB = GetComponent<Rigidbody2D>();
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
        DashHandler(InputManager.Movement);

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
        _targetMove.x = input.x * _moveSpeed;

        float _currentAcceleration = IsGrounded ? _acceleration : _airAcceleration;

        RB.linearVelocityX = S_ForceMove.GetForceByT(_targetMove, _currentAcceleration).x;

    }

    void MoveOnDeceleration()
    {
        _targetMove.x = Vector2.zero.x;
        RB.linearVelocityX = S_ForceMove.GetForceByT(_targetMove, _deceleration).x;
    }

    void MoveOnDashAcceleration()
    {

        _targetMove = _dashDirection * _dashSpeed;
        RB.linearVelocity = S_ForceMove.GetForceByT(_targetMove, _dashAcceleration);
    }

    void MoveOnDashDeceleration(Vector2 input)
    {
        _targetMove.x = Vector2.one.x * (input.x * 3); // it'll be Chengable after !
        _targetMove.y = Vector2.zero.y;

        RB.linearVelocity = S_ForceMove.GetForceByT(_targetMove, _dashDeceleration);
    }

    #endregion

    // Jump 🦘
    private void ApplyJump(bool input)
    {
        if ((input && IsGrounded) && Mathf.Abs(_targetMove.y) < _jumpHeight)
        {
            _targetMove.y = _jumpHeight;
            RB.linearVelocityY = S_ForceMove.GetForce(_targetMove).y;
        }

    }


    // Abilities 💪
    private void DashHandler(Vector2 input)
    {
        if ((InputManager.Dash && input != Vector2.zero) && _canDash)
        {
            IsDashing = true;
            _canDash = false;
            _dashDirection = input;
            _dashTimer = _dashTime;
            _dashCoolDownTimer = _dashCoolDown;
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
            RB.gravityScale = _dashGravity;
        else if (RB.linearVelocityY < -0.1)
            RB.gravityScale = _fallGravity;
        else
            RB.gravityScale = _noramlGravity;

    }


    // Check Collisions 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _dashTimer = -1;
        IsDashing = false;
        _stopDash = false;

        _targetMove = S_ReflectionBody.ReturnForce(collision, _reflectionPower);

        if (collision.transform.CompareTag("Ground"))
        {
            IsGrounded = true;
            RB.linearVelocityX = S_ForceMove.GetForceByT(_targetMove, _deceleration).x;
            RB.linearVelocityY = S_ForceMove.GetForce(_targetMove).y;
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
