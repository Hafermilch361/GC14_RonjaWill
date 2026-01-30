using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : MonoBehaviour
{
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;
    
    private CinemachineCamera cm;
    public string targetTag;
    public AudioSource _audio;

    public void Start()
    {
        cm = gameObject.GetComponent<CinemachineCamera>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cm.Priority.Value = 5;
            _audio.Play();
        }
        
        
        /*if (other.CompareTag(targetTag))
        {
        onTriggerEnter?.Invoke();
        cm.Priority = 10;
          }*/
    }
 
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            cm.Priority.Value = 0;
            _audio.Stop();
        }
        /*if (other.CompareTag(targetTag))
        {
            onTriggerExit?.Invoke();
            cm.Priority = 0;
        }*/
    }
}
