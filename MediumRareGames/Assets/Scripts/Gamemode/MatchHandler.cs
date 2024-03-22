/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
----------------------------------------------------------------------------- 
   MatchHandler
       - The script that handles keeping track of rounds/player wins.

   Details:
       - Keeps track of player wins through a static variable so it survives 
         scene loading.
       - Loads certain menus based on whether the round is over, or the match
         is over. 
-----------------------------------------------------------------------------
*/

//Note to self: Does this even need to be monobehaviour? Could get away with just a static class

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ConsoleLogging;

public class MatchHandler : MonoBehaviour
{
    #region Static Memebers
    private static int m_winsNeeded;
    private static bool m_longRound;
    private static bool m_shortRound;
    private static int m_longRounds;    // Track number of long and short rounds
    private static int m_fallOff;
    private static bool m_fellOff;
    private static int m_fallOffRounds;
    private static int m_playerDeaths;
    private static GameObject m_winner = null;
    private static Dictionary<string, int> m_winTracker;

    #region Long/Short Rounds
    public static int LongRounds { get { return m_longRounds; } }
    public static void LongRound()
    {
        m_longRound = true;
        m_longRounds++;
    }
    public static bool LongCheck()
    {
        if (m_longRound)
        {
            m_shortRound = false;
            m_longRound = false;
            return true;
        }
        else
            return false;
    }
    public static void ShortRound()
    {
        m_shortRound = true;
        m_longRounds++;
    }
    public static void ResetLongRounds()
    {
        m_shortRound = false;
        m_longRound = false;
        m_longRounds = 0;
    }
    public static bool ShortCheck()
    {
        if (m_shortRound)
        {
            m_shortRound = false;
            m_longRound = false;
            return true;
        }
        else
            return false;
    }
    #endregion

    #region Fall
    public static void ResetFall()
    {
        m_fallOff = 0;
        m_fallOffRounds = 0;
    }
    public static void FellOff()
    {
        m_fallOff++;
        if (m_fallOff >= LobbyMenu.Players.Count - 1)
        {
            m_fellOff = true;
            m_fallOff = 0;
            FellOffRound();
        }
    }
    public static void FellOffRound() { m_fallOffRounds++; }
    public static int FallOff { get { return m_fallOff; } }
    public static int FallOffRounds { get { return m_fallOff; } }
    public static bool FallCheck()
    {
        if (m_fellOff)
        {
            m_fellOff = false;
            return true;
        }
        else
            return false;
    }
    #endregion

    #region Deaths

    public static int DeathCount { get { return m_playerDeaths; } }
    public static void PlayerDeath() { m_playerDeaths++; }
    public static void ResetDeaths() { m_playerDeaths = 0; }

    #endregion

    /// <summary>How many round wins does a player need to win the match</summary>
    public static int WinsNeeded { get { return m_winsNeeded; } }

    /// <summary>The winner of the match, or the last round</summary>
    public static GameObject Winner { get { return m_winner; } }

    /// <summary>The dictionary tracking player round wins based on player name</summary>
    public static Dictionary<string, int> WinTracker { get { return m_winTracker; } }

    /// <summary>Clears/Resets the MatchHandler's static members (winner and win tracking). 
    /// Note: Non static to allow its use in UI buttons</summary>
    public void Clear()
    {
        m_winner = null;
        m_winTracker = null;
        ResetDeaths();
        ResetFall();
        ResetLongRounds();
    }

    public void ReloadScene()
    {
        m_playerDeaths = 0;
        m_fellOff = false;
        m_longRound = false;
        m_shortRound = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion

    [SerializeField] private Menu m_roundEndMenu;
    [SerializeField] private Menu m_matchEndMenu;
    [Space]
    [SerializeField]
    private Vector2 m_vibrate = new Vector2(0.5f, 0.5f);
    [SerializeField] private float m_vibrateDuration = 0.6f;

    /// <summary>Initialization</summary>
    private void Start()
    {
        if (m_winTracker == null)
        {
            m_winTracker = new Dictionary<string, int>();

            m_winsNeeded = GlobalSettings.Get.Match.RoundsToWin;

            foreach (PlayerInfo info in LobbyMenu.Players)
            {
                m_winTracker.Add(info.Name, 0);
            }

        }
    }

    /// <summary>Handles incrementing the win counter for the player</summary>
    /// <param name="_Player">The player who won the round</param>
    public void PlayerWon(GameObject _Player)
    {
        Menu nextMenu = m_roundEndMenu;
        m_winner = null; //Default winner of round to null (for ties)

        if (_Player != null)
        {
            //Player name is used to keep track of wins
            string pName = _Player.name;

            //Increment wins
            m_winTracker[pName]++;
            m_winner = _Player; //Set the winner of the match/round

            if (m_winTracker[pName] == m_winsNeeded)
                nextMenu = m_matchEndMenu;

            _Player.GetComponent<DeathHandler>().Player.GetComponent<PlayerController>().Input.Vibrate(m_vibrate, m_vibrateDuration);
        }

        MenuManager.Instance.ShowMenu(nextMenu);
        if (nextMenu == m_matchEndMenu)
        {
            ResetLongRounds();
            m_matchEndMenu.GetComponent<RoundEndMenu>()?.MatchEnd();
        }
    }

}
