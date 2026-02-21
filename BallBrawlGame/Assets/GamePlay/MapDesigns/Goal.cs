using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Goal : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] float Radius;
    [SerializeField] ColorState colorState;

    enum ColorState
    {
        Red, Green
    }

    private void OnValidate()
    {
        GetComponent<CircleCollider2D>().radius = Radius;
    }
    void Start()
    {
        GetComponent<CircleCollider2D>().radius = Radius;
    }

  
    private void OnDrawGizmos()
    {
        switch (colorState)
        {
            case ColorState.Red:
                Gizmos.color = Color.red;
                break;
            case ColorState.Green:
                Gizmos.color = Color.green;
                break;
        }

        Gizmos.DrawWireSphere(transform.position, Radius);
    }
}
