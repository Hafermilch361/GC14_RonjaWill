using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{ 
    private EnemyAnimation _animation;
   public GameObject enemylight;
   public GameObject enemyBossFolder;
  

    private void Start()
    {
        _animation = GetComponent<EnemyAnimation>();
    }

    public void EnemyHitEvent()
    {
        _animation.AnimationEnemyHit();
    }

    ///public void EnemyDeathLight()
  /// { enemylight.GetComponent<TriggerEnemyLightsSimple>().TurnOffLight(); }

  /// public void EnemyDeathSpawnBoss()
    /// { enemyBossFolder.SetActive(true); }


}

