using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    

    PlayerInput _playerInput;
    InputAction _moveAction;
    InputAction _jumpAction;
    InputAction _dashAction;

    public static Vector2 Movement;
    
    public static bool Dash;
    public static bool Jump;

    private void Awake()
    {
        
        _playerInput = GetComponent<PlayerInput>();
    }
    void Start()
    {
        _moveAction = _playerInput.actions["Move"];
        _jumpAction = _playerInput.actions["Jump"];
        _dashAction = _playerInput.actions["Dash"];
    }  

    
    void Update()
    {
        
        Movement = _moveAction.ReadValue<Vector2>();
        

        Dash = _dashAction.IsPressed();
        
        Jump = _jumpAction.IsPressed();
        
        
    }
}
