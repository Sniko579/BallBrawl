using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Abilitis : MonoBehaviour
{
    Rigidbody2D _rb;

    [Header("Dash")]
    [SerializeField] float DashSpeed = 50;
    [SerializeField] float DashTime = 0.5f;
    [SerializeField] float DashCoolDown = 0.5f;
    //privates
    bool canDash;
    TrailRenderer _tr;
    Camera _camera;
    //statics
    public static bool IsDashing;


    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _tr = GetComponent<TrailRenderer>();
        _camera = Camera.main;

    }
    void Start()
    {
        canDash = true;
        _tr.emitting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canDash && InputManager.Dash)
        {
            StartCoroutine(ApplyDash());
        }

    }

    #region Dash

    IEnumerator ApplyDash()
    {
        canDash = false;
        
        Vector2 mouseWorldPos = _camera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 Dir = (mouseWorldPos - _rb.position).normalized;
        _rb.linearVelocity = Dir * DashSpeed;
        _tr.emitting = true;
        IsDashing = true;

        float preGravity = _rb.gravityScale;

        yield return new WaitForSeconds(DashTime);
        _rb.gravityScale = 0f;
        _tr.emitting = false;
        IsDashing = false;
        yield return new WaitForSeconds(DashCoolDown);
        canDash = true;
        _rb.gravityScale = preGravity;
       
    }



    #endregion

}
