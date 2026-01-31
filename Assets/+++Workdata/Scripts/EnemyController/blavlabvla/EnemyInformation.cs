using System;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInformation : MonoBehaviour
{
    [SerializeField] private int enemyMaxLifePoints;

    public float _currentLifePoints;

    private Collider2D _coll;
    private Rigidbody2D _rb;
    private EnemyAnimation _enemyAnimation;
    private EnemyPatrolMovement _enemyPatrolMovement;
    public Image healthBar;

    private void Awake()
    {
        _currentLifePoints = enemyMaxLifePoints;

        _enemyAnimation = GetComponentInChildren<EnemyAnimation>();
        _coll = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _enemyPatrolMovement = GetComponentInChildren<EnemyPatrolMovement>();
        
    }
    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(_currentLifePoints / enemyMaxLifePoints, 0 ,1);
    }
    public void SetDamage(int dmg)
    {
        _currentLifePoints -= dmg;

        if (_currentLifePoints <= 0)
        {
            _coll.enabled = false;
            _rb.bodyType = RigidbodyType2D.Kinematic;
            _enemyAnimation.AnimationEnemyDeath();
            _enemyPatrolMovement.SetActionState(2);
            _enemyPatrolMovement.SetMovementState(0);
            _enemyPatrolMovement.enabled = false;
        }
    }


}
