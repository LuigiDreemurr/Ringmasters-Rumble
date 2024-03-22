using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundEndMenu : Menu
{
    [SerializeField] private GameObject m_winDisplayPrefab;
    [SerializeField] private Transform m_winDisplayContainer;

    [SerializeField] private AnnouncerChatter announcerChatter;

    private Dictionary<string, WinDisplay> winDisplays = new Dictionary<string, WinDisplay>();

    public override void OnShow()
    {
        WinDisplay winDisplay;

        foreach (PlayerInfo info in LobbyMenu.Players)
        {
            //Instantiate a win display for every player
            winDisplay = Instantiate(m_winDisplayPrefab, m_winDisplayContainer).GetComponent<WinDisplay>();

            winDisplay.Name = info.Name;
            winDisplay.Wins = MatchHandler.WinTracker[info.Name];
            winDisplay.Color = info.Color;

            winDisplays.Add(info.Name, winDisplay);
        }

        if (announcerChatter != null)
            announcerChatter.Play_Outro();
    }

    public void MatchEnd()
    {
        int goldWins = 0;
        int silverWins = 0;
        int bronzeWins = 0;

        foreach (PlayerInfo info in LobbyMenu.Players)
        {
            if (goldWins == 0)
            {
                goldWins = MatchHandler.WinTracker[info.Name];
            }
            else
            {
                if (MatchHandler.WinTracker[info.Name] > goldWins)
                {
                    bronzeWins = silverWins;
                    silverWins = goldWins;
                    goldWins = MatchHandler.WinTracker[info.Name];
                }
                else
                {
                    if (silverWins == 0)
                    {
                        silverWins = MatchHandler.WinTracker[info.Name];
                    }
                    else
                    {
                        if (MatchHandler.WinTracker[info.Name] > silverWins)
                        {
                            bronzeWins = silverWins;
                            silverWins = MatchHandler.WinTracker[info.Name];
                        }
                        else
                        {
                            if (bronzeWins == 0)
                            {
                                bronzeWins = MatchHandler.WinTracker[info.Name];
                            }
                            else
                            {
                                if (MatchHandler.WinTracker[info.Name] > bronzeWins)
                                {
                                    bronzeWins = MatchHandler.WinTracker[info.Name];
                                }
                            }
                        }
                    }
                }
            }
        }

        foreach (PlayerInfo info in LobbyMenu.Players)
        {
            if (MatchHandler.WinTracker[info.Name] == goldWins)
            {
                winDisplays[info.Name].AwardMedal(WinDisplay.MedalType.Gold);
            }
            else if (MatchHandler.WinTracker[info.Name] == silverWins)
            {
                winDisplays[info.Name].AwardMedal(WinDisplay.MedalType.Silver);
            }
            else if (MatchHandler.WinTracker[info.Name] == bronzeWins)
            {
                winDisplays[info.Name].AwardMedal(WinDisplay.MedalType.Bronze);
            }
        }
    }
}
