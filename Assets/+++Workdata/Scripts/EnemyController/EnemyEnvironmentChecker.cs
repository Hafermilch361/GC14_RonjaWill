using System;
using UnityEngine;

public class EnemyEnvironmentChecker : MonoBehaviour
{
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform groundCheck;

    [SerializeField] private float wallCheckRadius = 0.5f;
    [SerializeField] private float groundCheckRadius = 0.3f;

    [SerializeField] private LayerMask wallAndGroundMask;

    #region Private Variables

    private EnemyPatrolMovement _enemyPatrolMovement;

    #endregion

    private void Awake()
    {
        _enemyPatrolMovement = GetComponent<EnemyPatrolMovement>();
    }

    private void FixedUpdate()
    {
        if (_enemyPatrolMovement.enemyMovementState != EnemyPatrolMovement.EnemyMovementState.Movement) return;

        if (CheckForWalls() || CheckForGround())
        {
            _enemyPatrolMovement.ChangeDirection();
        }
    }

    bool CheckForWalls()
    {
        Vector2 direction = Vector2.right * _enemyPatrolMovement.FacingDirection;

        RaycastHit2D hit = Physics2D.Raycast(
            wallCheck.position,
            direction,
            wallCheckRadius,
            wallAndGroundMask
        );
        return hit.collider;
    }

    bool CheckForGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            groundCheck.position,
            Vector2.down,
            groundCheckRadius,
            wallAndGroundMask
        );
        return !hit.collider;
    }
    
    #region Gizmos

    private void OnDrawGizmos()
    {
        float direction = _enemyPatrolMovement ? _enemyPatrolMovement.FacingDirection : 1;
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(wallCheck.position, 
            wallCheck.position + Vector3.right * direction * wallCheckRadius);
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, 
            groundCheck.position + Vector3.down * groundCheckRadius);
    }

    #endregion
    
}