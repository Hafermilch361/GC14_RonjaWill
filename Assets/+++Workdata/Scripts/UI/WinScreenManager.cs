using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenManager : MonoBehaviour
{
    public GameObject winScreenContainer;
    private bool _isPaused;
    
    public GameObject boss;
    public AudioSource gameMusic;
    public AudioSource winMusic;
    
    public void winScreen()
    {
        Destroy(boss);
        _isPaused = !_isPaused;
        winScreenContainer.SetActive(true);
        Time.timeScale = _isPaused ? 0 : 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Destroy(gameMusic);
        winMusic.Play();
    }

    public void RestartScene(int index)
    {
        SceneManager.LoadScene(index); 
        winScreenContainer.SetActive(false);
        Time.timeScale = 1;
    }
}
