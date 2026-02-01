using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeal : MonoBehaviour
{
    private PlayerHealth player;
    private bool isHealing;
    public float healPerSecond = 5f;
    
    public float healAmount;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerHealth>().health += healAmount;
            isHealing = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isHealing = false;
        }
    }

    private void Update()
    {
        if (isHealing)
        {
            player.health += healPerSecond * Time.deltaTime;
        }
    }
}