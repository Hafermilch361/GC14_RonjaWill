using UnityEngine.UI;
using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Image healthBar;
    private Rigidbody2D _rb;

    void Start()
    {
        health = maxHealth;
        
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0 ,1);

        if (healthBar.fillAmount <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
