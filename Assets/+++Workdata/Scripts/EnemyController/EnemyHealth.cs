using UnityEngine.UI;
using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    #region Public Variables

    [SerializeField] private int enemyMaxHealth;
    public int currentHealth;

    #endregion

    #region Private Variables

    private EnemyBehaviour _behaviour;
    private Collider2D _collider;
    private Rigidbody2D _rb;
    private EnemyPatrolMovement enemyPatrol;

    #endregion

    private void Awake()
    {
        currentHealth = enemyMaxHealth;
        _behaviour = GetComponent<EnemyBehaviour>();
        _collider = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        enemyPatrol = GetComponent<EnemyPatrolMovement>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        _behaviour.EnemyHitEvent();

        if (currentHealth < 1)
        {
            _collider.enabled = false;
            _rb.bodyType = RigidbodyType2D.Kinematic;
            enemyPatrol.SetMovementState(0);
            enemyPatrol.SetActionState(2);
            enemyPatrol.enabled = false;
            _behaviour.EnemyDeathEvent();
        }
    }
}