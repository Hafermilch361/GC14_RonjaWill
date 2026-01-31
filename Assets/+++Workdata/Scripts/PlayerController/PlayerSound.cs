using UnityEngine;

public class PlayerSound : MonoBehaviour
{
[Header("Audio Sources")]
[SerializeField] private AudioSource _attackSound;

public void PlayAttackSound()
{
    _attackSound.Play();
}

public void StopAttackSound()
{
    _attackSound.Stop();
}
}
