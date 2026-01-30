using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static readonly int Hash_MovementValue = Animator.StringToHash("MovementValue");
    public static readonly int Hash_GroundValue = Animator.StringToHash("isGrounded");

    public enum PlayerMovementState { Idle, Move } 
    public enum PlayerActionState {Default, Attack, Attack2, Interact, Roll, Jump, Climb, Dash}
    public enum PlayerDirection {Left, Right}

    public PlayerMovementState playerMovementState;
    public PlayerActionState playerActionState;
    public PlayerDirection playerDirection;

    public Transform attackArea;
    
    #region Inspector

    //darauf möchte ich im INSPECTOR-Fenster in Unity zugreifen//


    [SerializeField] private float walkingSpeed = 6f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float jumpPower = 4f;
    
    [SerializeField] private float dashForce = 15f;
    [SerializeField] private float dashDuration = 1f;
    [SerializeField] private float dashCooldown = 1f;

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
    private Animator anim;
    public LayerMask enemies;
    public float radius;
    public float damage;
    
    public OneWayChecker _oneway;

    private PlayerInteractions _playerInteractions;
    
    private float _movementSpeed;

    private bool isGrounded; //Abfrage ob der Player den Boden berührt
    private bool isJumping;
    private bool canJump;
    
    private bool canDash = true;
    private bool isDashing = false;
    private float dashTimer = 0f;
    
    private bool isFacingRight = true; //angeben wohin unser Player "normal" schaut
    private bool MouseClicked;
    private bool _isHoldingMouse;
    
#endregion

    #region Input Variables

    private InputSystem_Actions _inputActions;
    private InputAction _moveAction; //zum bewegen (WASD)
    private InputAction _jumpAction; //zum springen (Space)
    private InputAction _attackAction; //zum angreifen (Mouseclick)
    private InputAction _secondAttackAction; //zum double attack
    private InputAction _dashAction; //dashen
    private InputAction _interactAction; //interagieren mit Interactables

    #endregion


    #region Unity Event Functions

    private void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        _playerInteractions = GetComponent<PlayerInteractions>();

        _oneway = GetComponentInChildren<OneWayChecker>();

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

        _interactAction.performed += Interact;

    }
    private void Update()
    {
        // Dash Timer
        if(isDashing)
        {
            dashTimer -= Time.deltaTime;
            if(dashTimer <= 0)
            {
                EndDash();
            }
        }
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
        
        _interactAction.performed -= Interact;
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
            _oneway.DisableOneWayCollider();
        }

    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (isGrounded && canJump)
        {
            isJumping = true;
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
        if (!canDash || isDashing) return;

        anim.SetInteger("ActionID", 3); //gleiche Zahl wie im Animator
        anim.SetTrigger("ActionTrigger"); //löst die StateMachine aus

        StartCoroutine(PerformDash());
    }

    private void EndDash()
    {
        isDashing = false;
        canDash = true;
        AnimationSetActionId(0);
    }

    private System.Collections.IEnumerator PerformDash()
    {
        isDashing = true;
        canDash = false;

        playerActionState = PlayerActionState.Dash;

        float originalGravity = _rb.gravityScale;
        _rb.gravityScale = 0; // Optional: kein Fall während Dash

        float direction = transform.rotation.y == 0 ? 1f : -1f;

        float timer = 0f;
        while (timer < dashDuration)
        {
            _rb.linearVelocity = new Vector2(direction * dashForce, 0);
            timer += Time.deltaTime;
            yield return null;
        }

        // Stop Movement
        _rb.linearVelocity = Vector2.zero;

        _rb.gravityScale = originalGravity; 

        isDashing = false;

        // Nachdem Dash vorbei ist, wieder normaler Zustand
        if (isGrounded)
            playerActionState = PlayerActionState.Default;

        yield return new WaitForSeconds(dashCooldown);

        canDash = true;
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        if(_interactAction == null)
        {
            Debug.LogWarning("No PlayerInteraction component to the player attached");
            return;
        }
        
        _playerInteractions.TryInteract();
    }
    
    private void SetStateToDefault()
    {
        playerActionState = PlayerActionState.Default;
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
            _interactAction = _inputActions.Player.Interact;
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
