
using System;
using UnityEngine;

public class MainMenu_UiManager : MonoBehaviour
{
    public GameObject mainMenuContainer;
    public GameObject loadMenuContainer;
    public GameObject optionsMenuContainer;
    
    public GameObject _currentMenu;
    public bool mute;
public AudioSource uiAudioSource;

    private void Awake()
    {
        _currentMenu = mainMenuContainer;
        mute = false;
        uiAudioSource.mute = mute;
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

    public void ToggleMute()
    {
        mute = !mute;
        uiAudioSource.mute = mute;
    }
}
