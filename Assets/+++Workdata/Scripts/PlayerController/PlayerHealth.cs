using UnityEngine.UI;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public Image healthBar;
    public GameObject _ui;

    void Start()
    {
        maxHealth = health;
        Time.timeScale = 1;
    }

    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0 ,1);

        if (healthBar.fillAmount <= 0)
        {
            Destroy(gameObject);
            Destroy(_ui);
        }
    }

}
