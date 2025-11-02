using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region Insepctor Variables

    [SerializeField]
    private float walkingSpeed = 5f;

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
    private Rigidbody2D rb;
    private float movementSpeed = 5f;
    
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

        rb = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        _inputActions.Enable();
        _moveAction.performed += Move;
        _moveAction.canceled += Move;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(_moveInput.x * movementSpeed, rb.velocity.y
        );
        
    }

    private void OnDisable()
    {
        _inputActions.Disable();
        _moveAction.performed -= Move;
        _moveAction.canceled -= Move;
    }
    #endregion
    #region Input Methods

    private void Move(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
    }
     
    #endregion
}
