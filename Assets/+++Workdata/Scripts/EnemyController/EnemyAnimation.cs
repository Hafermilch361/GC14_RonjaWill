using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    private static readonly int HashMovementValue = Animator.StringToHash("MovementValue");
    private static readonly int HashActionTrigger = Animator.StringToHash("ActionTrigger");
    private static readonly int HashActionId = Animator.StringToHash("ActionId");
    private static readonly int HashisDead = Animator.StringToHash("isDead");

    private Animator _animator;
 private EnemyPatrolMovement _enemyPatrolMovement;

 private void Awake()
 {
     _animator = GetComponent<Animator>();
     _enemyPatrolMovement = GetComponentInParent<EnemyPatrolMovement>();
 }

 public void AnimationEnemyDeath()
    {
        _animator.SetBool(HashisDead, true);
        AnimationSetActionID(10);
        
    }

    public void AnimationEnemyHit()
    {
        AnimationSetActionID(2);
    }
    public void AnimationAttack()
    {
        AnimationSetActionID(1);
    }

    public void AnimationSetMovementValue(float value)
    {
        _animator.SetFloat(HashMovementValue, value);
    }

    private void AnimationSetActionID(int number)
    {
        _animator.SetTrigger(HashActionTrigger);
        _animator.SetInteger(HashActionId, number);
    }
public void AnimationActionValue()
    {
        _enemyPatrolMovement.SetActionState(0);
    }

    public void EnemyDeath()
    {
        Destroy(gameObject);
    }
}