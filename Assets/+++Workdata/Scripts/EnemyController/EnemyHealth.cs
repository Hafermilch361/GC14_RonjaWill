using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
        public int maxHealth = 50;
        public int currentHealth;

        void Start()
        {
            currentHealth = maxHealth;
        }

        public void TakeDamage(int amount)
        {
            currentHealth -= amount;
            Debug.Log("Enemy HP: " + currentHealth);
        
            if (currentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
