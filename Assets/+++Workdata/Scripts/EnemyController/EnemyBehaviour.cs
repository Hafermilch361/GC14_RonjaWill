using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{ 
    private EnemyAnimation _animation;
    private void Start()
    {
        _animation = GetComponent<EnemyAnimation>();
    }

    public void EnemyHitEvent()
    {
        _animation.AnimationEnemyHit();
    }

}

