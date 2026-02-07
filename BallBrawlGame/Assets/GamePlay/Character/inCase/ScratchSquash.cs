using UnityEngine;

public class ScratchSquash : MonoBehaviour
{

    [Header("References")]
    [SerializeField] Transform VisualChild;

    [Header("Sitting")]
    [SerializeField] float TimeSpeed = .1f;
    //private
    Rigidbody2D _rb;

    Vector2 baseScale;
    private Vector2 rVelocity;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        baseScale = VisualChild.localScale;

    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = VisualChild.localScale;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

       
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }

}
