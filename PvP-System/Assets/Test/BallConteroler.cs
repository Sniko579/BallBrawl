using UnityEngine;

[RequireComponent (typeof(Rigidbody2D),typeof(CircleCollider2D))]
public class BallConteroler : MonoBehaviour
{
    [SerializeField] ValuePanel valuePanel;
    [SerializeField] Vector2 TargetMove;
    [SerializeField] float GravityScale = 0;
    
    
    bool Tasking;

    Rigidbody2D RB;
    
    ReflectionSystem reflectionSystem;

    Vector2 _lastVelocity;


    private void OnValidate()
    {
        TargetMove.x = TargetMove.x > 1 ? 1 : TargetMove.x;
        TargetMove.x = TargetMove.x < -1 ? -1 : TargetMove.x;
        TargetMove.y = TargetMove.y > 1 ? 1 : TargetMove.y;
        TargetMove.y = TargetMove.y < -1 ? -1 : TargetMove.y;
       
    }

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
      
    }
    void Start()
    {
        Tasking = true;
        RB.gravityScale = GravityScale;
    }

    private void FixedUpdate()
    {
        if (Tasking)
        {
            Vector2 x = TargetMove * valuePanel.Speed;
            RB.linearVelocity = Vector2.Lerp(RB.linearVelocity,x,valuePanel.Accelration * Time.fixedDeltaTime);
        }
        else
        {
            Vector2 x = Vector2.zero;
            RB.linearVelocity = Vector2.Lerp(RB.linearVelocity, x, valuePanel.Decelration * Time.fixedDeltaTime);
        }

    }

    private void Update()
    {
        reflectionSystem.SetVelocity(RB.linearVelocity);
        _lastVelocity = RB.linearVelocity;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Tasking = false;

        //TargetMove = reflectionSystem.ReturnForce(collision, valuePanel.Bounceness_Power);

        //RB.linearVelocity = TargetMove;
    
    }

    public Vector2 LastVelocity()
    {
        return _lastVelocity;
    }


}
