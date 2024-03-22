using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamemodeSelectionMenu : Menu<GamemodeSelectionMenu> 
{
    private static Gamemode.Type s_gamemode;
    public static Gamemode.Type GamemodeType { get { return s_gamemode; } }

    public void ChooseLMS()
    {
        s_gamemode = Gamemode.Type.LMS;
    }

    public void ChooseKOTH()
    {
        s_gamemode = Gamemode.Type.KotH;
    }
}
