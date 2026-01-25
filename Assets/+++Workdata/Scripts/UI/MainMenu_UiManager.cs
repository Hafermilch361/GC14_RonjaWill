
using System;
using UnityEngine;

public class MainMenu_UiManager : MonoBehaviour
{
    public GameObject mainMenuContainer;
    public GameObject loadMenuContainer;
    public GameObject optionsMenuContainer;
    public GameObject creditsContainer;

    public GameObject _currentMenu;
    public bool mute;
    
    public AudioSource clickSound;
    public AudioSource startSound;
    public AudioSource backgroundMusic;
    public AudioSource alternativeMusic;
    private AudioSource current;

    private void Awake()
    {
        _currentMenu = mainMenuContainer;
        mute = false;
        clickSound.mute = mute;
        startSound.mute = mute;
        backgroundMusic.mute = mute;
        current = backgroundMusic;
    }

    private void Start()
    {
        backgroundMusic.Play();
        backgroundMusic.Pause();
        
        alternativeMusic.Play();
        alternativeMusic.Pause();

        backgroundMusic.UnPause();  
        current = backgroundMusic;
    }

    public void OpenLoadMenu()
    {
        _currentMenu.SetActive(false);

        loadMenuContainer.SetActive(true);
        _currentMenu = loadMenuContainer;
    }

    public void OpenMainMenu()
    {
        _currentMenu.SetActive(false);
        mainMenuContainer.SetActive(true);
        _currentMenu = mainMenuContainer;
    }

    public void OpenOptionsMenu()
    {
        _currentMenu.SetActive(false);
        optionsMenuContainer.SetActive(true);
        _currentMenu = optionsMenuContainer;
    }

    public void OpenCredits()
    {
        _currentMenu.SetActive(false);
        creditsContainer.SetActive(true);
        _currentMenu = creditsContainer;
    }


    public void ToggleMuteFX()
    {
        mute = !mute;
        clickSound.mute = mute;
        startSound.mute = mute;
    }

    public void ToggleMuteMusic()
    {
        mute = !mute;
        backgroundMusic.mute = mute;
        alternativeMusic.mute = mute;
    }

    public void SwitchMusic()
    {
        if (current == backgroundMusic)
        {
            backgroundMusic.Pause();
            alternativeMusic.UnPause();
            current = alternativeMusic;
        }
        else
        {
            alternativeMusic.Pause();
            backgroundMusic.UnPause();
            current = backgroundMusic;
        }
    }
}
