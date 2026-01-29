using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class EnemyPatrolMovement : MonoBehaviour
{
    public enum EnemyMovementState
    {
        Idle,
        Movement,
        Chase
    }

    public enum EnemyActionState
    {
        Default,
        Attack,
        Dead
    }

    #region Public Variables

    [Header("States")] 
    public EnemyMovementState enemyMovementState;
    public EnemyActionState enemyActionState;

    [Header("Movement")] 
    [SerializeField] private bool _isFacingRight = true;
    [SerializeField] private float _moveSpeed = 4f;

    [Header("Attack")] [SerializeField] private float attackDistance = 1;

    #endregion

    #region Private Variables

    private Rigidbody2D _rb;
    private EnemyAnimation _enemyAnimation;
    private Vector2 _moveInput;
    private Animator _anim;
    
    private int _facingDirection;

    public int FacingDirection => _facingDirection;

    private Transform _chaseTarget;

    #endregion

    #region Unity Events

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _enemyAnimation = GetComponent<EnemyAnimation>();
        _anim = GetComponent<Animator>();

        _facingDirection = _isFacingRight ? 1 : -1;
        
        
    }

    private void FixedUpdate()
    {
        if (enemyActionState != EnemyActionState.Default) return;


        if (enemyMovementState == EnemyMovementState.Chase) //Chase Verhalten
        {
            if (_chaseTarget == null)
            {
                Debug.LogError("Chase Target ist null");
                return;
            } 
            if (Vector2.Distance(transform.position, _chaseTarget.position) < attackDistance)
            {
                _enemyAnimation.AnimationAttack();
                enemyActionState = EnemyActionState.Attack;
                return;
            }
            if ((transform.position.x < _chaseTarget.position.x && _facingDirection == -1) ||
                (transform.position.x > _chaseTarget.position.x && _facingDirection == 1))
            {
                ChangeDirection();
            }
    
        }
        
        
        _rb.linearVelocityX = _moveSpeed * _facingDirection;
    }
    private void MovementForAnim() 
    {
        float speed = Mathf.Abs(_rb.linearVelocity.x);
        _anim.SetFloat("MovementValue", speed > 0.01f ? 1f : 0f);
    }

    private void LateUpdate()
    {
        UpdateAnimator();
        MovementForAnim();
    }

    #endregion

    #region State

    public void SetMovementStateToChase(Transform target)
    {
        enemyMovementState = EnemyMovementState.Chase;
        _chaseTarget = target;
    }

    public void SetMovementState(int state)
    {
        enemyMovementState = (EnemyMovementState)state;
        if (enemyMovementState != EnemyMovementState.Chase)
        {
            _chaseTarget = null;
        }

    }

    public void SetActionState(int state)
    {
        enemyActionState = (EnemyActionState)state;
        if (enemyMovementState != EnemyMovementState.Chase)
        {
            _chaseTarget = null;
        }


/*
        public void SetActionStateToDefault()
        {
          enemyActionState = EnemyActionState.Default;
        }
 */   }

    #endregion


        #region Movement
        
        public void ChangeDirection()
        {
            _facingDirection *= -1;

            transform.rotation = Quaternion.Euler(0, _facingDirection == -1 ? 0 : 180, 0);
        }

        #endregion

        #region Animation

        public void UpdateAnimator()
        { 
            _enemyAnimation.AnimationSetMovementValue(Mathf.Abs(_rb.linearVelocityX));
        }

        #endregion
    }
