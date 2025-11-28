using System.Collections;
using UnityEngine;

public class DamageZone : MonoBehaviour
    
{
        public int damageAmount = 10;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerHealth>()?.TakeDamage(damageAmount);
            }
        }
    }

void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
