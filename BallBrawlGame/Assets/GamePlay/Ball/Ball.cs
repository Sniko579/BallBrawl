using UnityEngine;

public class Ball : MonoBehaviour, IPVP
{
    public static Rigidbody2D RB;
    ReflectionBody _reflectionBody;
    PhysicForce _forceMove;
    public static Vector2 TargetMove;



    [SerializeField][Range(0f, 100f)] float ReflectionPower = 0.5f;

    [SerializeField] float Deceleration;
    private bool _isGround;

    private void OnValidate()
    {


    }
    void Start()
    {
        RB = GetComponent<Rigidbody2D>();

    }


    void Update()
    {
        _reflectionBody.SetVelocity(RB.linearVelocity);
        _forceMove.SetCurrentVelocity(RB.linearVelocity);

    }

    private void FixedUpdate()
    {
        if (_isGround)
            DecelerationMove();

    }
    private void DecelerationMove()
    {

        TargetMove.x = Vector2.zero.x;
        RB.linearVelocityX = _forceMove.GetForceByT(TargetMove, Deceleration).x;

    }
    public void IsInGoal()
    {
        TargetMove = Vector2.zero;
        RB.linearVelocity = TargetMove;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGround = true;
        }


        TargetMove = _reflectionBody.ReturnForce(collision, ReflectionPower);
        RB.linearVelocity = TargetMove;

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _isGround = false;
    }





}
