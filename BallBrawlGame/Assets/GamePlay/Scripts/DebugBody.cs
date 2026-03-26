using UnityEngine;

public class DebugBody : MonoBehaviour
{
    Rigidbody2D RB;

    public Vector2 LastValue;


    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IPVP enemy = collision.gameObject.GetComponent<IPVP>();
        if (enemy != null)
        {
            Debug.Log($"Enemy Name Was {collision.gameObject.name} And Its Velocity Was {collision.rigidbody.linearVelocity}");
            LastValue = RB.linearVelocity;
        }

    }


}
