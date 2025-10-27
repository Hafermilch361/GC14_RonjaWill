using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Insepctor Variables

    [SerializeField]
    private float walkingSpeed;

    #endregion

    #region Private Variables

    private InputSystem_Actions _inputActions;
    private InputAction _jumpAction;
    private InputAction _moveAction;
    private InputAction _attackAction;
    private InputAction _interactAction;
    private InputAction _crouchAction;
    private InputAction _sprintAction;

    public Vector2 _moveInput;
    
    #endregion
    
    #region Unity Event Functions
    
    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _moveAction = _inputActions.Player.Move;
        _jumpAction = _inputActions.Player.Jump;
        _attackAction = _inputActions.Player.Attack;
        _interactAction = _inputActions.Player.Interact;
        _crouchAction = _inputActions.Player.Crouch;
        _sprintAction = _inputActions.Player.Sprint;
    }
    private void OnEnable()
    {
        _inputActions.Enable();
        _moveAction.performed += Move;
        _moveAction.canceled += Move;
    }

    private void OnDisable()
    {
        _inputActions.Disable();
        _moveAction.performed -= Move;
        _moveAction.canceled -= Move;
    }
    #endregion
    #region Input

    private void Move(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
    }
     
    #endregion
}
