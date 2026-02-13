using UnityEngine;

public class Bounce : MonoBehaviour, IPVP
{
    Rigidbody2D _rb;
    ReflectionBody _reflectionBody;
    PhysicForce _forceMove;
    Vector2 _targetMove;


    [SerializeField][Range(0f, 100f)] float BouncePower = 0.5f;
    [SerializeField] float Deceleration;
    private bool _isGround;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _reflectionBody = new ReflectionBody();
    }


    void Update()
    {
        _reflectionBody.SetVelocity(_rb.linearVelocity, "Updating Enemy");
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
        _rb.linearVelocityX = _forceMove.GetXForceByT(_targetMove.x, Deceleration);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isGround = true;

        _targetMove = _reflectionBody.ReturnForce(collision, BouncePower);
        _rb.linearVelocity = _targetMove;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _isGround = false;
    }

    public ReflectionBody GetReflectionBody()
    {
        return _reflectionBody;

    }
}
