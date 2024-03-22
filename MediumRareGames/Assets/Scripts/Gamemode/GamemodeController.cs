/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
----------------------------------------------------------------------------- 
   GamemodeController
       - Handles adding the appropriate gamemode and sending information
         to the MatchHandler

   Details:
       - Will add the appropriate gamemode component based on the type from
         ModeSelection
-----------------------------------------------------------------------------
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Health;

public class GamemodeController : MonoBehaviour
{
    #region Data Members

    #region Static
    /// <summary>Simple dictionary that returns a function that adds the appropriate gamemode component based on gamemode type</summary>
    static private Dictionary<Gamemode.Type, Func<GameObject, Gamemode>> CreateGamemode = new Dictionary<Gamemode.Type, Func<GameObject, Gamemode>>()
    {
        { Gamemode.Type.LMS, (GameObject _GMController) => { return _GMController.AddComponent<LastManStanding>(); } },
        { Gamemode.Type.KotH, (GameObject _GMController) => { return _GMController.AddComponent<KingOfTheHill>(); } }
    };
    #endregion

    [SerializeField] private MatchHandler m_matchHandler; //Handles keeping track of player wins
    [SerializeField] private RoundTimer m_roundTimer;   // So we can end the round when the timer is up
    [SerializeField] private Countdown m_countdown;     // So we can start the round when counting is done
    [SerializeField] private GameObject m_pointPrefab;  // KotH point prefab

    private GameObject[] m_players; //List of players
    private Gamemode m_gamemode; //Current gamemode
    private Gamemode.Type m_gamemodeType; //Current gamemode type (LMS or KotH)
    public Gamemode.Type gamemodeType { get { return m_gamemodeType; } }

    private List<GameObject> m_points = new List<GameObject>(); // KotH points
    private int m_currentPointIndex = -1;   // Current KotH point

    #region Properties
    public GameObject[] Players { get { return m_players; } }
    public Gamemode Gamemode { get { return m_gamemode; } }
    public Gamemode.Type GamemodeType { get { return m_gamemodeType; } }
    #endregion

    #endregion

    /// <summary>Initialization</summary>
    public void Initialize(GameObject[] _Players)
    {
        //Get references to available players
        m_players = _Players;

        //Set the type
        m_gamemodeType = GamemodeSelectionMenu.GamemodeType;

        //Add appropriate gamemode component based on type
        m_gamemode = CreateGamemode[m_gamemodeType](gameObject);

        if (m_roundTimer != null)
        {
            m_roundTimer.KillSwitch(m_gamemodeType == Gamemode.Type.KotH);
            if (m_gamemodeType == Gamemode.Type.KotH)
                m_roundTimer.HideTimer();
            else
            {
                m_roundTimer.ShowTimer();
                //Subscribe to timer end event
                m_roundTimer.TimeIsUp += OnTimerEnd;
            }
        }

        if (m_gamemodeType == Gamemode.Type.KotH)
            InitializePoints();

        //Subscribe to countdown end event
        if (m_countdown != null)
            m_countdown.CountdownFinished += OnCountdownEnd;
    }
    
    /// <summary> Disable current point, show next point. </summary>
    public void NextPoint()
    {
        if (m_currentPointIndex >= 0)
        {
            m_points[m_currentPointIndex]?.GetComponent<KingOfTheHillPoint>().Deactivate();
            m_currentPointIndex++;
            if (m_currentPointIndex >= m_points.Count)
                m_currentPointIndex = 0;
        }
        else    // First time activating a point
            m_currentPointIndex = 0;
        m_points[m_currentPointIndex].GetComponent<KingOfTheHillPoint>().Activate();
    }

    /// <summary> Fill stack of KotH point prefabs. </summary>
    private void InitializePoints()
    {
        GameObject _TempPoint;
        Settings.KingOfTheHill _Settings = GlobalSettings.Get.Gamemode.KingOfTheHill;
        //Find KotH points
        List<Level.Tile> kothPoints = GetKOTHPoints();

        for (int i = 0; i < _Settings.PossiblePoints; i++)
        {
            _TempPoint = Instantiate(m_pointPrefab, transform.position, transform.rotation);
            if (_TempPoint.GetComponent<KingOfTheHillPoint>() != null)
            {
                _TempPoint.transform.position = PointPosition(kothPoints);   // Places point on arena
                _TempPoint.GetComponent<KingOfTheHillPoint>().Deactivate();
                m_points.Add(_TempPoint);
            }
        }
    }

    private Vector3 PointPosition(List<Level.Tile> _KothPoints)
    {
        Vector3 newPosition = new Vector3(0,0,0);
        System.Random rnd = new System.Random();    // Generate seed (System.Random is more random than UnityEngine.Random)
        int r;

        // Assign newPosition to position on map
        if (_KothPoints.Count < GlobalSettings.Get.Gamemode.KingOfTheHill.PossiblePoints)
        {
            // Set new position if there are more possible points than actual points
            r = rnd.Next(0, _KothPoints.Count); // Get random int
            newPosition = _KothPoints[r].transform.position;
            // TODO: Add buffers to stop points from being used twice in a row
        }
        else    // Else, don't repeat points
        {
            bool endLoop;
            while (true)
            {
                endLoop = true;
                r = rnd.Next(0, _KothPoints.Count); // Get random int
                newPosition = _KothPoints[r].transform.position;
                for (int i = 0; i < m_points.Count; i++)
                {
                    if (m_points[i].transform.position == newPosition)
                    {
                        endLoop = false;
                        break;  // End for loop
                    }
                }
                if (endLoop)
                    break;
            }
        }

        return newPosition;
    }

    /// <summary>Finds the available KotH points</summary>
    /// <returns>A list of tile KotH points</returns>
    static private List<Level.Tile> GetKOTHPoints()
    {
        return Level.Utility.FindFilteredTiles((Level.Tile _Tile) => { return _Tile.KothSpawnPoint; });
    }

    /// <summary>Checks if the gamemode round is over. If so end the round</summary>
    public void CheckWinCondition()
    {
        if (m_gamemode.IsRoundOver())
            EndRound();
    }

    public void SetRoundTimer(float _Timer)
    {
        m_roundTimer.SetTimeRemaining(_Timer);
        m_roundTimer.SetStartTime(_Timer);
    }

    #region Helper Methods
    /// <summary>Ends the round by sending the gamemode winner to the match handler</summary>
    private void EndRound()
    {
        if (m_roundTimer?.GetTimeElapsed() <= m_roundTimer?.GetWarningTime() && gamemodeType == Gamemode.Type.LMS)
            MatchHandler.ShortRound();
        GameObject winner = m_gamemode.GetWinner();
        m_matchHandler.PlayerWon(winner);
    }

    /// <summary>When the round timer is up, end the round</summary>
    /// <param name="_Sender">Where the event came from</param>
    /// <param name="_Args">The event arguments</param>
    private void OnTimerEnd(object _Sender, EventArgs _Args)
    {
        MatchHandler.LongRound();
        EndRound();
    }

    /// <summary>When the countdown is up, start the round</summary>
    /// <param name="_Sender">Where the event came from</param>
    /// <param name="_Args">The event arguments</param>
    private void OnCountdownEnd(object _Sender, EventArgs _Args)
    {
        m_countdown.CountdownFinished -= OnCountdownEnd;
        //foreach (var playerTimer in m_playerTimers)
        //{
        //    playerTimer.Value.KillSwitch(false);
        //}
        if (m_gamemodeType == Gamemode.Type.KotH)
            NextPoint();    // Activate first point
    }
    #endregion
}