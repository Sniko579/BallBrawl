using UnityEngine;

public class CameraDistance : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform m_Target;

    [Header("CameraBoxSetting")]
    [SerializeField] Vector2 CameraBoxPosition;
    [SerializeField] float DistanceX;
    [Space]
    [SerializeField] float DistanceYUp;
    [SerializeField] float DistanceYDown;

    //private
    Vector2 _cameraBox;
    float _cameraRightLimet;
    float _cameraLeftLimet;
    float _cameraUpLimet;
    float _cameraDownLimet;
    [Header("GeneralSetting")]
    [SerializeField] float SpeedFollow;
 


    //private
    Vector2 _currentPosition;



    private void OnValidate()
    {
        m_Target = FindAnyObjectByType<Player>().transform;
        Initializing();
        CameraFallow();

    }
    void Start()
    {
        Initializing();
    }

    // Update is called once per frame
    void Update()
    {
       
        CameraFallow();

    }
    private void FixedUpdate()
    {
    }
    private void CameraFallow()
    {

       
        if (m_Target.position.x > _cameraRightLimet || m_Target.position.x < _cameraLeftLimet)
        {
            if (m_Target.position.x > _cameraBox.x)
                _currentPosition.x = Mathf.Lerp(_currentPosition.x, _cameraRightLimet, Time.fixedDeltaTime * SpeedFollow);
            else
                _currentPosition.x = Mathf.Lerp(_currentPosition.x, _cameraLeftLimet, Time.fixedDeltaTime * SpeedFollow);
        }
        else
        {
            _currentPosition.x = Mathf.Lerp(_currentPosition.x, m_Target.position.x, Time.fixedDeltaTime * SpeedFollow);
        }


        if (m_Target.position.y > _cameraUpLimet || m_Target.position.y < _cameraDownLimet)
        {
            if (m_Target.position.y > _cameraBox.y)
                _currentPosition.y = Mathf.Lerp(_currentPosition.y, _cameraUpLimet, Time.fixedDeltaTime * SpeedFollow);
            else
                _currentPosition.y = Mathf.Lerp(_currentPosition.y, _cameraDownLimet, Time.fixedDeltaTime * SpeedFollow);
        }
        else
        {
            _currentPosition.y = Mathf.Lerp(_currentPosition.y, m_Target.position.y, Time.fixedDeltaTime * SpeedFollow);
        }

      

        GetComponent<Camera>().transform.position = _currentPosition;
    }


    private void Initializing()
    {
        _cameraBox = CameraBoxPosition;
        _cameraRightLimet = _cameraBox.x + DistanceX;
        _cameraLeftLimet = _cameraBox.x - DistanceX;

        _cameraUpLimet = _cameraBox.y + DistanceYUp;
        _cameraDownLimet = _cameraBox.y - DistanceYDown;
    }




    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_cameraBox, Vector2.right * DistanceX);
        Gizmos.DrawRay(_cameraBox, Vector2.left * DistanceX);

        Gizmos.color = Color.green;
        Gizmos.DrawRay(_cameraBox, Vector2.up * DistanceYUp);
        Gizmos.DrawRay(_cameraBox, Vector2.down * DistanceYDown);
    }


}
