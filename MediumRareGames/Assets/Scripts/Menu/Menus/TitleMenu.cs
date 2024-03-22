/*
 -----------------------------------------------------------------------------
        Created By Wesley Ducharme
 -----------------------------------------------------------------------------
    TitleMenu
        - Menu script that handles allowing any button press to go to the main
          menu

    Details:
        - Handles subscribing/unscubscribing on hide/show
 -----------------------------------------------------------------------------
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMenu : Menu<TitleMenu> 
{
    [SerializeField] private Menu m_mainMenu;

    //temp
    private void Update()
    {
        if (Input.anyKeyDown)
            ShowMainMenu();
    }

    public override void OnShow()
    {
        //TODO: Subscribe all controller buttons
    }

    public override void OnHide()
    {
        //TODO: Unsubscribe all controller buttons
    }

    private void ShowMainMenu()
    {
        MenuManager.Instance.ShowMenu(m_mainMenu);
    }
}
