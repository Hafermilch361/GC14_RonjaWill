using UnityEngine.UI;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Image healthBar;

    void Start()
    {
        maxHealth = health;
    }

    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0 ,1);

        if (healthBar.fillAmount <= 0)
        {
            Destroy(gameObject);
        }
    }

}
