using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static readonly int Hash_MovementValue = Animator.StringToHash("MovementValue");
    public static readonly int Hash_GroundValue = Animator.StringToHash("isGrounded");
    private static readonly int Hash_JumpValue = Animator.StringToHash("JumpValue");
    private static readonly int Hash_Actionid = Animator.StringToHash("ActionId");
    private static readonly int Hash_ActionTrigger = Animator.StringToHash("ActionTrigger");
    private static readonly int Hash_HoldingMouse = Animator.StringToHash("HoldingMouse");

    public enum PlayerMovementState { Idle, Move } 
    public enum PlayerActionState {Default, Attack, Attack2, Interact, Roll, Jump, Climb, Dash}
    public enum PlayerDirection {Left, Right}

    public PlayerMovementState playerMovementState;
    public PlayerActionState playerActionState;
    public PlayerDirection playerDirection;
    public PlayerHealth playerHealth;

    public Transform attackArea;
    
    #region Inspector

    //darauf möchte ich im INSPECTOR-Fenster in Unity zugreifen//


    [SerializeField] private float walkingSpeed = 6f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float jumpPower = 4f;
    private float _dashforce = 5f;
    public float _dashSpeed;

    [Header("GroundCheck")] [SerializeField]
    private Vector2 boxSize;

    [SerializeField] private Vector2 boxOffset;
    [SerializeField] private LayerMask groundLayer;


    [SerializeField] public bool IsGrounded;

    private SpriteRenderer _sr; //Zugriff zum Sprite Renderer im Inspector

    #endregion


    #region Private Variables

//meine Inputs und "Abkürzungen" für spätere Verwendung//

    public Vector2 _moveInput;
    private Rigidbody2D _rb;
    private PlayerPlatformHandler _playerPlatformHandler;
    
    private Animator anim;
    public LayerMask enemies;
    public float radius;
    public float damage;
    public float bounceForce;
    
    // public OneWayChecker _oneway;

    
    
    private float _movementSpeed;

    private bool isGrounded; //Abfrage ob der Player den Boden berührt
    private bool isJumping;
    private bool canJump;
    
    private bool canDash;
    private bool isDashing;
    
    private bool isFacingRight = true; //angeben wohin unser Player "normal" schaut
    private bool MouseClicked;
    private bool _isHoldingMouse;

    #region Input Variables

    private InputSystem_Actions _inputActions;
    private InputAction _moveAction; //zum bewegen (WASD)
    private InputAction _jumpAction; //zum springen (Space)
    private InputAction _attackAction; //zum angreifen (Mouseclick)
    private InputAction _secondAttackAction; //zum double attack
    private InputAction _dashAction; //dashen

    #endregion

    #endregion


    #region Unity Event Functions

    private void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        _playerPlatformHandler = GetComponent<PlayerPlatformHandler>();

    //    _oneway = GetComponentInChildren<OneWayChecker>();

        _movementSpeed = walkingSpeed;
        canJump = true;

        SetInputActionsInput();
        SetDirection(playerDirection);

        playerActionState = PlayerActionState.Default;

        if (playerDirection == PlayerDirection.Right)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (playerDirection == PlayerDirection.Left)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    

    private void OnEnable()
    {
        _inputActions.Enable();

        _moveAction.performed += Move;
        _moveAction.canceled += Move;

        _jumpAction.performed += Jump;

        _attackAction.performed += Attack;
        _attackAction.canceled += AttackReleased;
        _secondAttackAction.started += SecondAttack;
        
        _dashAction.performed += Dash;

    }

    private void FixedUpdate()
    {
        CheckGround();
        if(playerActionState != PlayerActionState.Dash)
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
        _attackAction.canceled += AttackReleased;
        _secondAttackAction.canceled -= SecondAttack;
        
        _dashAction.performed -= Dash;
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

    private void AnimationSetActionId(int id)
    {
        anim.SetTrigger("ActionTrigger");
        anim.SetInteger("Actionid", id);
    }

    void UpdateAnimator() //für Übersichtlichkeit steht es hier und nicht direkt im FixedUpdate
    {
        anim.SetFloat(Hash_MovementValue,
            Mathf.Abs(_rb.linearVelocity.x)); //oder: anim.SetFloat("MovementValue", _rb.linearVelocityX)
        anim.SetBool(Hash_GroundValue,
            IsGrounded); //Mathf.Abs= dass passiert, egal welche Richtung (sonst nur nach rechts)

    }
private void AnimEvent_EndJump()
    {
        
        canJump = true;
    }

    private void AnimEvent_EndDash()
    {
     print("Player has dashed");
        isDashing =  false;
        canDash = true;
        SetActionToDefault();
    }
private void SetActionToDefault()
    {
        playerActionState = PlayerActionState.Default;
    }
    #endregion

    #region Input

    private void Move(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();

        if (_moveInput.x > 0) //wenn Input größer 0 (D) -> nicht flipX nutzen (dreht nur die Sprite)!!!
        { SetDirection(PlayerDirection.Right); }
        
        else if (_moveInput.x < 0) //wenn Input kleiner 0 (A)
        { SetDirection(PlayerDirection.Left); }

        if (_moveInput.x == 0)
        { playerMovementState = PlayerMovementState.Idle; }
        else
        { playerMovementState = PlayerMovementState.Move; }

        if (_moveInput.y < 0)
        {
            _playerPlatformHandler.TryDisableOneWayEffector();
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
        if (playerActionState == PlayerActionState.Attack) return;
        
            playerActionState = PlayerActionState.Attack;
            AnimationSetActionId(11);
            _isHoldingMouse = true;
            anim.SetBool("HoldingMouse", true);
            DealDamage();
            
    }
    

    private void AttackReleased(InputAction.CallbackContext ctx)
    {
        anim.SetBool("HoldingMouse", false);
    }

    private void SecondAttack(InputAction.CallbackContext ctx)
    {
        if (playerActionState == PlayerActionState.Attack2) return;
        
            playerActionState = PlayerActionState.Attack2;
            AnimationSetActionId(12);
        
    }

    private void Dash(InputAction.CallbackContext ctx)
    {
        
        if (isGrounded && canDash)
            
        {
            canDash = false;
            isDashing = true;
            
            _rb.AddForce((isFacingRight ? Vector2.right : Vector2.left) * _dashforce, ForceMode2D.Impulse);
            playerActionState = PlayerActionState.Dash;
            AnimationSetActionId(3);
        }
    }
    
    private void DealDamage()
    {
        Collider2D[] enemy = Physics2D.OverlapCircleAll(attackArea.transform.position,radius, enemies);
        foreach (Collider2D enemyGameobject in enemy)
        {
            Debug.Log("Hit enemy");
            enemyGameobject.GetComponent<EnemyHealth>().health -= damage;
        }
    }


    private void SetStateToDefault()
    {
        playerActionState = PlayerActionState.Default;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Damage"))
        {
            _rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
        }
    }

    #endregion

    #region Gizmos

        void OnDrawGizmos()
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireCube(boxOffset + (Vector2)transform.position, boxSize);
            
            Gizmos.DrawWireSphere(attackArea.transform.position, radius);
        }

        #endregion

        #region Utility

        private void SetInputActionsInput()
        {
            _inputActions = new InputSystem_Actions();
            _moveAction = _inputActions.Player.Move;
            _jumpAction = _inputActions.Player.Jump;
            _attackAction = _inputActions.Player.Attack;
            _secondAttackAction = _inputActions.Player.SecondAttack;
            _dashAction = _inputActions.Player.Dash;
        }
        private void SetDirection(PlayerDirection newPlayerDirectionState)
        {
            playerDirection = newPlayerDirectionState;
        
            if (playerDirection == PlayerDirection.Left)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0); //durch Rotation ganzer Player rotiert und nicht nur die Sprite
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0); //Spieler soll sich rotieren, vorher isFacingRight definieren
            }
        }


        #endregion
    }
