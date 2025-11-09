using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static readonly int Hash_MovementValue = Animator.StringToHash("MovementValue");
    public static readonly int Hash_GroundValue = Animator.StringToHash("isGrounded");
    
    #region Inspector
    //darauf möchte ich im INSPECTOR-Fenster in Unity zugreifen//
   
    [Header("Movement")]
    [SerializeField] private float walkingSpeed = 6f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float jumpPower = 5f;
    
    [Header("GroundCheck")]
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private Vector2 boxOffset;
    [SerializeField] private LayerMask groundLayer;


    [SerializeField] public bool IsGrounded;

    private SpriteRenderer _sr;

    #endregion

    
    #region Private Variables
//meine Inputs und "Abkürzungen" für spätere Verwendung//

    public Vector2 _moveInput;
    private Rigidbody2D _rb;
    private Animator anim;
    
    private float _movementSpeed;
    
    private bool isGrounded;
    private bool isJumping;
    private bool canJump;
    
    #region Input Variables
    private InputSystem_Actions _inputActions;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    #endregion
    
    #endregion

    
    #region Unity Event Functions
    private void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        
        _movementSpeed = walkingSpeed;
        canJump = true;
        
        _inputActions = new InputSystem_Actions();
        _moveAction = _inputActions.Player.Move;
        _jumpAction = _inputActions.Player.Jump;
    }

    private void OnEnable()
    {
        _inputActions.Enable();

        _moveAction.performed += Move;
        _moveAction.canceled += Move;
        
        _jumpAction.performed += Jump;
        
    }

    private void FixedUpdate()
    {
        CheckGround();
        
        _rb.linearVelocityX = _moveInput.x * walkingSpeed;
        
        UpdateAnimator();
    }

    private void OnDisable()
    {
        _inputActions.Disable();

        _moveAction.performed -= Move;
        _moveAction.canceled -= Move;
        
        _jumpAction.performed -= Jump;
    }

    #endregion

    #region Physics

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapBox(boxOffset + (Vector2)transform.position, boxSize, 0, groundLayer);
        IsGrounded = isGrounded;
        if (isGrounded)
        {
            canJump = true;
        }
    }

    #endregion
    
    #region Animation Methods

    void UpdateAnimator()
    {
        anim.SetFloat(Hash_MovementValue,Mathf.Abs(_rb.linearVelocity.x));
        anim.SetBool(Hash_GroundValue, IsGrounded);
    }
    
    
    #endregion
    #region Input

    private void Move(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();

        if (_moveInput.x > 0)
        {
            _sr.flipX = false;
        }
        else if (_moveInput.x < 0)
        {
            _sr.flipX = true;
        }
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (isGrounded && canJump)
        {
            canJump = false;
            _rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }
    #endregion

    #region Gizmos

    void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        
        Gizmos.DrawWireCube(boxOffset+(Vector2)transform.position,boxSize);
    }

    #endregion
    
}

