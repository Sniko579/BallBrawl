using System.Collections;
using UnityEngine;

public class EnemyAgent : MonoBehaviour
{
    [Header("Refferences")]
    Rigidbody2D _rb;
    [SerializeField] Transform PlayerPos;

    [Header("Pre Movement")]
    [SerializeField] float MoveSpeed = 4f;
    [SerializeField] float JumpHeight = 4f;
    [SerializeField] float Acceleration = 30f;
    [SerializeField] float AirAcceleration = 30f;
    [SerializeField] float Deceleration = 8f;
    [SerializeField] float AirDeceleration = 8f;

    private Vector2 _dashDirection;
    private Vector2 _targetMove;
    private Vector2 _playerMove;
    private bool _isGrounded;

    private float _radius;
    private float _rollAngle;

    [Header("Bounciness System")]
    ReflectionBody _reflectionBody;

    [Header("Dash")]
    [SerializeField] float DashSpeed = 50;
    [SerializeField] float DashTime = 0.5f;
    [SerializeField] float DashCoolDown = 0.5f;
    [SerializeField] float DashAcceleration, DashDeceleration;
    private bool _canDash;
    private bool _isDashing;

    private float _dashStopTimer;

    [Header("Gravity Setting")]
    [SerializeField] float NoramlGravity = 1f;
    [SerializeField] float FallGravity = 1f;

    private void Awake()
    {

    }
    void Start()
    {
        _canDash = true;
        _radius = GetComponent<CapsuleCollider2D>().size.x * 0.5f * transform.lossyScale.x;
        _rb = GetComponent<Rigidbody2D>();
        _reflectionBody = new ReflectionBody();

    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void FixedUpdate()
    {
        Agent();
    }

    private void Agent()
    {
        Vector2 n = Vector2.one;
       
        ApplyMovement(n);

    }



    #region EnemyMove

    private void ApplyMovement(Vector2 input)
    {
        if (_isDashing)
        {
            MoveOnDashAcceleration(_dashDirection);
            _dashStopTimer = 0;
        }
        else if ((!_isDashing && !_canDash) && _dashStopTimer <= DashTime)
        {
            MoveOnDashDeceleration();
            _dashStopTimer += Time.fixedDeltaTime;
        }
        else if (input.x != 0 && _canDash)
        {
            Debug.Log("A");
            MoveOnAcceleration(input);
        }
        else
        {
            Debug.Log("D");
            MoveOnDeceleration();
        }



    }

    void MoveOnAcceleration(Vector2 input)
    {
        _targetMove = input * MoveSpeed;

        float _currentAcceleration = _isGrounded ? Acceleration : AirAcceleration;

        _playerMove = Vector2.Lerp(_playerMove, _targetMove, _currentAcceleration * Time.fixedDeltaTime);

        rollingOnMove();

        _rb.linearVelocityX = _playerMove.x * _radius;
    }


    void MoveOnDeceleration()
    {
        _targetMove = Vector2.zero;

        float _currentAcceleration = _isGrounded ? Deceleration : AirDeceleration;

        _playerMove = Vector2.Lerp(_playerMove, _targetMove, _currentAcceleration * Time.fixedDeltaTime);

        rollingOnMove();

        _rb.linearVelocityX = _playerMove.x * _radius;

    }

    void MoveOnDashAcceleration(Vector2 input)
    {
        _targetMove = input * DashSpeed;

        _playerMove = Vector2.Lerp(_playerMove, _targetMove, DashAcceleration * Time.fixedDeltaTime);

        _rb.linearVelocity = _playerMove * _radius;

    }

    void MoveOnDashDeceleration()
    {
        _targetMove = Vector2.zero;

        _playerMove = Vector2.Lerp(_playerMove, _targetMove, DashDeceleration * Time.fixedDeltaTime);

        _rb.linearVelocity = _playerMove * _radius;

    }


    #endregion

    // Jump 
    private void ApplyJump(bool input)
    {
        if ((input && _isGrounded) && _rb.linearVelocityY < JumpHeight)
        {
            _rb.linearVelocityY = JumpHeight;
        }
    }

    // Abilities 💪
    private void ApplyDash(Vector2 input)
    {
        if ((InputManager.Dash && input != Vector2.zero) && _canDash && !_isGrounded)
        {
            StartCoroutine(EnemyDash(input));

        }

    }


    // physical 

    private void rollingOnMove()
    {
        _rollAngle += _rb.linearVelocityX / _radius;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -_rollAngle);
    }


    // IEnumerator's_Funions
    IEnumerator EnemyDash(Vector2 direction)
    {
        _canDash = false;

        _dashDirection = direction;

        _isDashing = true;


        yield return new WaitForSeconds(DashTime);

        _isDashing = false;

        yield return new WaitForSeconds(DashCoolDown);


        _canDash = true;

    }




    private void GravityManager()
    {
        if (_isDashing)
        {
            _rb.gravityScale = 1;
        }
        else if (_rb.linearVelocityY < -0.1)
        {
            _rb.gravityScale = FallGravity;
        }
        else
        {
            _rb.gravityScale = NoramlGravity;
        }

    }





    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            _isGrounded = true;

        }


        //RflectionBody(collision);

    }

    //private void RflectionBody(Collision2D collision)
    //{
    //    var speed = _lastVelocity.magnitude * 1.2f;
    //    _dashDirection = Vector2.Reflect(_lastVelocity.normalized, collision.contacts[0].normal);
    //    _targetMove = _dashDirection * speed;
    //    _playerMove = _targetMove;
    //    _rb.linearVelocity = _playerMove * _radius;
    //}

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Ground"))
        {
            _isGrounded = false;

        }

    }


}
