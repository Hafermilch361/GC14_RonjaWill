using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    public Transform attackOrigin;
    public float attackRadius = 1f;
    public LayerMask enemyMask;

    public int attackDamage = 1;

    public float cooldownTime = .5f;
    public float cooldownTimer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (cooldownTimer <= 0)
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(attackOrigin.position, attackRadius, enemyMask);
                foreach (var enemy in enemiesInRange)
                {
                    enemy.GetComponent<EnemyHealth>().SetDamage(attackDamage); 
                }
                cooldownTimer -= cooldownTime;
            }
            else
            {
                cooldownTimer = Time.deltaTime;
            }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackOrigin.position, attackRadius);
    }
}