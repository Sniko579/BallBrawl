using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PVP : MonoBehaviour
{
    Rigidbody2D _rb;
    [Header("Power")]
    [SerializeField][Range(0f, 100f)] float ShootingPower = 1f;
    [Header("Resistance")]
    [SerializeField][Range(1f, 100f)] float ShootingResistance = 1f;

    private Vector2 _lastVelocity;
    public void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        _lastVelocity = _rb.linearVelocity;
    }
    public void Shoot(PVP _EnemyPVP, Vector2 _Velocity)
    {
        float _shootPower = ShootingPower / 100;
        _EnemyPVP.BeShoot(_Velocity * _shootPower);
    }
    public void BeShoot(Vector2 _EnemyVelocity)
    {
        float _shootResistance = ShootingResistance / 100;
        _rb.linearVelocity += _EnemyVelocity * _shootResistance;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PVP enemyPvP = collision.gameObject.GetComponent<PVP>();
        if (enemyPvP != null)
            Shoot(enemyPvP, _lastVelocity);
    }
}
