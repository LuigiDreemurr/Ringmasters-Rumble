using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInput;
using XInputDotNetPure;

public class PauseHandler : MonoBehaviour
{
    [SerializeField] private Button m_pauseButton = Button.Start;
    [SerializeField] private Menu m_pauseMenu;

	// Use this for initialization
	void Start ()
    {
        ControllerManager controllerManager = ControllerManager.Instance;
        foreach(PlayerInfo info in LobbyMenu.Players)
        {
            controllerManager.GetController(info.Index).Subscribe(m_pauseButton, PlayerPause);
        }

        MenuManager menuManager = MenuManager.Instance;
        menuManager.OnFirstMenuShow += () => { ControllerManager.Instance.SendControllerEvents(false); };

        menuManager.OnLastMenuHide += () => 
        {
            ControllerManager.Instance.SendControllerEvents(true);
            menuManager.MenuController = controllerManager.GetController(LobbyMenu.Players[0].Index);
        };

    }

    private void PlayerPause(XInput.Controller _Controller, ButtonArgs _Args)
    {
        MenuManager.Instance.MenuController = _Controller;
        MenuManager.Instance.ShowMenu(m_pauseMenu);
    }
	
}
