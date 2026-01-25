using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu_Manager : MonoBehaviour
{
    public GameObject pauseMenuContainer;
    public AudioSource gameMusic;
    public AudioSource pauseMenuMusic;
    private bool _isPaused;
    private bool _musicHasStarted = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
           if (_isPaused)
               ResumeGame();
           else
           {
              OpenPauseMenu(); 
           }
            
        }
    }

    private void OpenPauseMenu()
    {
        _isPaused = !_isPaused;
        pauseMenuContainer.SetActive(true);
        Time.timeScale = _isPaused ? 0 : 1;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameMusic.Pause();

        if (!_musicHasStarted)
        {
            pauseMenuMusic.Play();
            _musicHasStarted = true;
        }
        else
        {
            pauseMenuMusic.UnPause();
        }
    }

    public void ResumeGame()
    {
        _isPaused = false;
        pauseMenuContainer.SetActive(false);
        Time.timeScale = 1;
        gameMusic.UnPause();
        pauseMenuMusic.Pause();
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
        pauseMenuContainer.SetActive(false);
    }
}