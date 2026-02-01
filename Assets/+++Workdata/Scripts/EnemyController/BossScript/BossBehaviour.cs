using UnityEngine.UI;
using UnityEngine;
using System;

public class BossBehaviour : MonoBehaviour
{ 
    private static readonly int HashActionId = Animator.StringToHash("ActionId");
    [SerializeField] private WinScreenManager winScreenManager;
    
    #region Variables
    public float bossMaxHealth;
    public float bossHealth;
    public Image bossHealthBar;

    private Collider2D _collider;
    private Rigidbody2D _rb;
    public Animator _anim;
    

    #endregion

    private void Awake()
    {
        bossHealth = bossMaxHealth;
        _collider = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        winScreenManager = GetComponent<WinScreenManager>();
    }

    void Start()
    {
        _anim.enabled = true;
    }
    void Update()
    {
        bossHealthBar.fillAmount = Mathf.Clamp(bossHealth / bossMaxHealth, 0 ,1);
    }

    public void SetDamage(int damage)
    {
        Debug.Log("DAMAGE");
        bossHealth -= damage;
        
        if (bossHealth <= 0)
        {
            GetComponentInParent<SpriteRenderer>().enabled = false;
            AnimationSetActionID(1);
        }
    }
    
    private void AnimationSetActionID(int number)
    {
        _anim.SetInteger(HashActionId, number);
    }

    public void playerWon()
    {
       Destroy(gameObject);
       winScreenManager.winScreen();
        
    }
}