using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : MonoBehaviour
{
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;
 
    public string targetTag;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            onTriggerEnter?.Invoke();
            gameObject.GetComponent<CinemachineCamera>().Priority = 10;
        }
    }
 
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            onTriggerExit?.Invoke();
            gameObject.GetComponent<CinemachineCamera>().Priority = 0;
        }
    }
}
