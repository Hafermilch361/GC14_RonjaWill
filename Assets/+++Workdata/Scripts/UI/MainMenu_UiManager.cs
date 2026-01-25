
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

    private void Awake()
    {
        _currentMenu = mainMenuContainer;
        mute = false;
        clickSound.mute = mute;
        startSound.mute = mute;
        backgroundMusic.mute = mute;
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
    }
}
