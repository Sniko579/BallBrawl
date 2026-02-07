using UnityEngine;

public class Bounce : MonoBehaviour
{
    Rigidbody2D _rb;
    ReflectionBody _reflectionBody;

    [SerializeField] float BouncePower = 0.5f;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        _reflectionBody.SetVelocity(_rb.linearVelocity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _rb.linearVelocity = _reflectionBody.ReturnForce(collision, BouncePower);
    }
}
