using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public GameObject gameOverContainer;
    private bool _isPaused;
    public GameObject player;

    private void Update()
    {
        if (player.GetComponent<PlayerHealth>().health == 0f)
        {
                gameOverScreen();
        }
    }

    private void gameOverScreen()
    {
        _isPaused = !_isPaused;
        gameOverContainer.SetActive(true);
        Time.timeScale = _isPaused ? 0 : 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void RestartScene(int index)
    {
        SceneManager.LoadScene(index);
        gameOverContainer.SetActive(false);
        Time.timeScale = 1;
    }
}
