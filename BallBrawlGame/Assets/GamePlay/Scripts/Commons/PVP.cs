using UnityEngine;

[RequireComponent (typeof(Rigidbody2D))]
public class PVP : MonoBehaviour
{
    Rigidbody2D _rb;
    [Header("Power")]
    [SerializeField] float ShootingPower = 1f;
    [Header("Resistance")]
    [SerializeField] float ShootingResistance = 1f;

    private Vector2 _lastVelocity;
    public void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        _lastVelocity = _rb.linearVelocity;
    }
    public void Shoot(Collision2D _Collision, Vector2 _Velocity)
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Shoot(collision, _lastVelocity);
    }
}
