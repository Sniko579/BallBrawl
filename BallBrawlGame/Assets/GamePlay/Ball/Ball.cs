using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour, IPVP
{
    Rigidbody2D _rb;
    ReflectionBody _reflectionBody;
    PhysicForce _forceMove;
    Vector2 _targetMove;

    Vector2 spwonPoint;

    [SerializeField][Range(0f, 100f)] float ReflectionPower = 0.5f;

    [SerializeField] float Deceleration;
    private bool _isGround;

    private void OnValidate()
    {


    }
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        spwonPoint = transform.position;
    }


    void Update()
    {
        _reflectionBody.SetVelocity(_rb.linearVelocity);
        _forceMove.SetCurrentVelocity(_rb.linearVelocity);

    }

    private void FixedUpdate()
    {
        if (_isGround)
            DecelerationMove();

    }
    private void DecelerationMove()
    {

        _targetMove.x = Vector2.zero.x;
        _rb.linearVelocityX = _forceMove.GetForceByT(_targetMove, Deceleration).x;

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGround = true;
        }
        

        _targetMove = _reflectionBody.ReturnForce(collision, ReflectionPower);
        _rb.linearVelocity = _targetMove;

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _isGround = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Goal"))
        {
            transform.position = spwonPoint;
            _rb.linearVelocity = Vector2.zero;
        }
        
    }


}
