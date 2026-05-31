using UnityEngine;

public class RollingBall : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;
    float _radius;
    private float _rollAngle;

    void Start()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _radius = GetComponentInParent<CapsuleCollider2D>().size.x * 0.5f * transform.lossyScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        Rolling();
    }

    void Rolling()
    {
        _rollAngle += _rb.linearVelocityX / _radius;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y, -_rollAngle);
    }
}
