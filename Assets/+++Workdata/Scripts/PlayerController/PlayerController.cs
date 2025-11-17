using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static readonly int Hash_MovementValue = Animator.StringToHash("MovementValue");
    public static readonly int Hash_GroundValue = Animator.StringToHash("isGrounded");
    
    #region Inspector
    //darauf möchte ich im INSPECTOR-Fenster in Unity zugreifen//
   
   
    [SerializeField] private float walkingSpeed = 6f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float jumpPower = 4f;
    
    
    
    [Header("GroundCheck")]
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private Vector2 boxOffset;
    [SerializeField] private LayerMask groundLayer;


    [SerializeField] public bool IsGrounded;

    private SpriteRenderer _sr; //Zugriff zum Sprite Renderer im Inspector

    #endregion

    
    #region Private Variables
//meine Inputs und "Abkürzungen" für spätere Verwendung//

    public Vector2 _moveInput;
    private Rigidbody2D _rb;
    private Animator anim;
    
    private float _movementSpeed;
    
    private bool isGrounded; //Abfrage ob der Player den Boden berührt
    private bool isJumping;
    private bool canJump;
    private bool isFacingRight = true; //angeben wohin unser Player "normal" schaut
    private bool MouseClicked;
    
    #region Input Variables
    private InputSystem_Actions _inputActions;
    private InputAction _moveAction; //zum bewegen (WASD)
    private InputAction _jumpAction; //zum springen (Space)
    private InputAction _attackAction; //zum angreifen (Mouseclick)
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
        _attackAction = _inputActions.Player.Attack;
    }

    private void OnEnable()
    {
        _inputActions.Enable();

        _moveAction.performed += Move;
        _moveAction.canceled += Move;
        
        _jumpAction.performed += Jump;

        _attackAction.performed += Attack;

    }

    private void FixedUpdate()
    {
        CheckGround();
        
        _rb.linearVelocityX = _moveInput.x * walkingSpeed;
        
        UpdateAnimator(); //in Animations definiert, hier, damit es jeden Frame aufgerufen wird
    }

    private void OnDisable()
    {
        _inputActions.Disable();

        _moveAction.performed -= Move;
        _moveAction.canceled -= Move;
        
        _jumpAction.performed -= Jump;

        _attackAction.performed -= Attack;
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

    void UpdateAnimator() //für Übersichtlichkeit steht es hier und nicht direkt im FixedUpdate
    {
        anim.SetFloat(Hash_MovementValue,Mathf.Abs(_rb.linearVelocity.x)); //oder: anim.SetFloat("MovementValue", _rb.linearVelocityX)
        anim.SetBool(Hash_GroundValue, IsGrounded); //Mathf.Abs= dass passiert, egal welche Richtung (sonst nur nach rechts)
        
    }
    
    
    #endregion
    #region Input

    private void Move(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();

        if (_moveInput.x > 0) //wenn Input größer 0 (D) -> nicht flipX nutzen (dreht nur die Sprite)!!!
        {
            transform.rotation = Quaternion.Euler(0,0,0); //Spieler soll sich rotieren, vorher isFacingRight definieren
            isFacingRight = true;
        }
        else if (_moveInput.x < 0) //wenn Input kleiner 0 (A)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); //durch Rotation ganzer Player rotiert und nicht nur die Sprite
            isFacingRight = false;
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

    private void Attack(InputAction.CallbackContext ctx)
    {
        anim.SetInteger("Actionid", 11);
        anim.SetTrigger("ActionTrigger");
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

