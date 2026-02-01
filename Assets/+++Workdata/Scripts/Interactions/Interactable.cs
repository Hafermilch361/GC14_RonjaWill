using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public UnityEvent onInteract;
    public UnityEvent onSelected;
    public UnityEvent onDeselected;

    private PlayerHealth player;
    public float healAmount;
    
    public bool reusable;
    public bool destroyAfterUse;

    private bool _alreadyInteracted;
    
    public void Interact()
    {
        onInteract?.Invoke();
        _alreadyInteracted = true;
        
        if(destroyAfterUse)
            Destroy(gameObject);
    }

   /* public void Healing(Collider2D other)
    {
        other.gameObject.GetComponent<PlayerHealth>().health += healAmount;
    }*/
}
