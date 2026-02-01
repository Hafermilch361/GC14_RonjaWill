using UnityEngine;

public class PlayerAttackTrigger : MonoBehaviour
{
    [SerializeField] private int damage;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyHealth>().SetDamage(damage);
        }
        
        if (other.CompareTag("Boss"))
        {
            other.gameObject.GetComponent<BossBehaviour>().SetDamage(damage);
        }
    }
}
