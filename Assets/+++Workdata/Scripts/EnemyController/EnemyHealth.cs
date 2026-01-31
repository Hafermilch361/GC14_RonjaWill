using UnityEngine.UI;
using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    #region Variables
    public float enemyMaxHealth;
    public float health;
    public Image healthBar;

    private EnemyBehaviour _behaviour;
    private Collider2D _collider;
    private Rigidbody2D _rb;
    private EnemyPatrolMovement enemyPatrol;
    private EnemyAnimation _enemyAnimation;
    

    #endregion

    private void Awake()
    {
        health = enemyMaxHealth;
        _behaviour = GetComponent<EnemyBehaviour>();
        _collider = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        enemyPatrol = GetComponent<EnemyPatrolMovement>();
        _enemyAnimation = GetComponent<EnemyAnimation>();
    }
    
    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(health / enemyMaxHealth, 0 ,1);
    }

    public void SetDamage(int damage)
    {
        Debug.Log("DAMAGE");
        health -= damage;
        
        if (health <= 0) //wenn keien health mehr
            {
                _collider.enabled = false;
                _rb.bodyType = RigidbodyType2D.Kinematic;
                _enemyAnimation.AnimationEnemyDeath();
                enemyPatrol.SetActionState(2);
            enemyPatrol.SetMovementState(0);
            enemyPatrol.enabled = false; //enemy funktionen alle "ausschalten"
            }
    }
}