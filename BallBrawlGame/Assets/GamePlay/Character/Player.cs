using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour, IPVP
{
    [Header("Require")]
    [SerializeField] GlobalVariables Sc_GlobalVariables;
    public static Rigidbody2D RB;



    Vector2 _targetMove;
    PhysicForce S_ForceMove;

    float _radius;
    float _rollAngle;



    bool _canDash, _stopDash;
    float _dashTimer;
    float _dashCoolDownTimer;
    Vector2 _dashDirection;


    //statics
    public static bool IsDashing;
    public static bool IsGrounded;



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
        else
        {
            MoveOnDeceleration();
        }

    }

    void MoveOnAcceleration(Vector2 input)
    {
        _targetMove.x = input.x * Sc_GlobalVariables.MoveSpeed;

        float _currentAcceleration = IsGrounded ? Sc_GlobalVariables.Acceleration : Sc_GlobalVariables.AirAcceleration;

        RB.linearVelocityX = S_ForceMove.GetForceByT(_targetMove, _currentAcceleration).x;

    }

    void MoveOnDeceleration()
    {
        _targetMove.x = Vector2.zero.x;
        float _currentDeceleration = IsGrounded ? Sc_GlobalVariables.Deceleration : Sc_GlobalVariables.AirDeceleration;
        RB.linearVelocityX = S_ForceMove.GetForceByT(_targetMove, _currentDeceleration).x;
    }

    void MoveOnDashAcceleration()
    {
        _targetMove = _dashDirection * Sc_GlobalVariables.DashSpeed;
        RB.linearVelocity = S_ForceMove.GetForceByT(_targetMove, Sc_GlobalVariables.DashAcceleration);
    }

    void MoveOnDashDeceleration(Vector2 input)
    {
        _targetMove.x = Vector2.one.x * (input.x * 3); // it'll be Chengable after !
        _targetMove.y = Vector2.zero.y;

        RB.linearVelocity = S_ForceMove.GetForceByT(_targetMove, Sc_GlobalVariables.DashDeceleration);
    }

    #endregion

    // Jump 🦘
    private void ApplyJump(bool input)
    {
        if ((input && IsGrounded) && Mathf.Abs(RB.linearVelocity.y) < Sc_GlobalVariables.MoveSpeed)
        {
            _targetMove.y = Sc_GlobalVariables.MoveSpeed;
            RB.linearVelocityY = _targetMove.y;
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
            _dashTimer = Sc_GlobalVariables.DashTime;
            _dashCoolDownTimer = Sc_GlobalVariables.DashCoolDown;
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
            RB.gravityScale = Sc_GlobalVariables.DashGravity;
        else if (RB.linearVelocityY < -0.1)
            RB.gravityScale = Sc_GlobalVariables.FallGravity;
        else
            RB.gravityScale = Sc_GlobalVariables.NoramlGravity;

    }


    // Check Collisions 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _dashTimer = -1;
        IsDashing = false;
        _stopDash = false;


        if (collision.transform.CompareTag("Ground"))
        {
            IsGrounded = true;


        }
        if (collision.gameObject.GetComponent<IPVP>() != null && IsGrounded)
        {
            float x = Mathf.Abs(collision.rigidbody.linearVelocity.y);
            _targetMove.y = Vector2.up.y * x;
        
            RB.linearVelocityY = _targetMove.y;

        }


    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            IsGrounded = true;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            IsGrounded = false;
        }

    }




}
