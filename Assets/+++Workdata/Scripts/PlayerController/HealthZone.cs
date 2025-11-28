using UnityEngine;

public class HealthZone : MonoBehaviour
{
    public class HealZone : MonoBehaviour
    {
        public int healAmount = 20;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerHealth>()?.Heal(healAmount);
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
