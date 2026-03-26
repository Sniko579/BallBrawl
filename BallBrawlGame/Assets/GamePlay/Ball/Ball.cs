using UnityEngine;

public class Ball : MonoBehaviour , IPVP
{
    public static Rigidbody2D RB;
    PhysicForce _forceMove;
    Vector2 _targetMove;


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

        _forceMove.SetCurrentVelocity(RB.linearVelocity);


    }

    private void FixedUpdate()
    {
        if (_isGround)
            DecelerationMove();


    }
    private void DecelerationMove()
    {

        _targetMove.x = Vector2.zero.x;
        RB.linearVelocityX = _forceMove.GetForceByT(_targetMove, Deceleration).x;

    }
    public void IsInGoal()
    {
        _targetMove = Vector2.zero;
        RB.linearVelocity = _targetMove;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {



        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGround = true;

        }
        if (collision.gameObject.GetComponent<IPVP>() != null && _isGround)
        {

            float x = Mathf.Abs(collision.rigidbody.linearVelocity.y);
            _targetMove.y = Vector2.up.y * x;
      
            RB.linearVelocityY = _targetMove.y;
        }


    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGround = true;


        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        _isGround = false;
    }





}
