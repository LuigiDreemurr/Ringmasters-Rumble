using System;
using System.Collections;
using System.Collections.Generic;
using Health;
using UnityEngine;
using ConsoleLogging;
using System.Linq;

[RequireComponent(typeof(GamemodeController))]
public class KingOfTheHill : Gamemode
{
    #region Data Members

    private Dictionary<GameObject, RoundTimer> m_playerTimers; // Dictionary representing each player's timers
    private Dictionary<GameObject, int> m_playersPoints; // Dictionary representing each player's captured points

    private Settings.KingOfTheHill m_settings;

    #endregion

    #region Private Methods

    /// <summary> Returns a list of players with no points left to capture. </summary>
    /// <returns></returns>
    private List<GameObject> WinningPlayers()
    {
        List<GameObject> winners = new List<GameObject>();
        if (m_playersPoints.Count > 0)
            foreach (var playerPoints in m_playersPoints)
            {
                if (playerPoints.Value <= 0)
                    winners.Add(playerPoints.Key);
            }
        return winners;
    }

    #endregion

    #region Gamemode Implementation

    /// <summary> King of the Hill is considered over when a player has captured all the points they need. </summary>
    /// <returns> Returns true if there is at least one player with no points to capture. </returns>
    public override bool IsRoundOver()
    {
        // Find how many players have no points to capture
        if (WinningPlayers().Count == 0)
        {
            gameObject.GetComponent<GamemodeController>()?.NextPoint();
            ResetAllTimers();
        }

        return WinningPlayers().Count > 0;
    }

    /// <summary> The winner of King of the Hill is based on whoever has captured all required points </summary>
    /// <returns> The player with no time left. Or null </returns>
    public override GameObject GetWinner()
    {
        List<GameObject> winners = WinningPlayers();

        if (winners.Count > 0)
        {
            GameObject winner = winners[0];

            if (winners.Count > 1)
            {
                Log.Error(this, "Somehow tried to get a winner with multiple players having captured the required points.");

                // Loop through possible winners
                foreach (GameObject player in winners)
                {
                    // Skip the winner that was set
                    if (player == winner)
                        continue;

                    // Take points left
                    if (m_playersPoints[player] < m_playersPoints[winner])
                        winner = player;
                }

                // With multiple possible winners we need to check if there is a tie
                foreach (GameObject player in winners)
                {
                    if (player == winner)
                        continue;

                    // Take Time finished
                    if (m_playersPoints[player] == m_playersPoints[winner])
                        winner = null;
                }
            }

            return winner;
        }
        else    // No winner
        {
            return null;
        }
    }

    /// <summary> Respawn the player. </summary>
    /// <param name="_Health"> The player health </param>
    /// <param name="_DamageSource"> The source of the death </param>
    protected override void OnDeath(Health.Health _Health, DamageSource _DamageSource)
    {
        GameObject parent = _Health.transform.parent.gameObject;

        // Respawn
        parent.GetComponentInChildren<RoundTimer>().Btn_StopTimer();            // Stop Timer
        parent.GetComponent<DeathHandler>().Respawn(m_settings.RespawnTime);    // Respawn player
    }

    /// <summary> Setup the player timer tracking </summary>
    protected override void Initialize()
    {
        m_settings = GlobalSettings.Get.Gamemode.KingOfTheHill;

        // Set Timer Events & Timer Objects
        m_playerTimers = new Dictionary<GameObject, RoundTimer>();
        m_playersPoints = new Dictionary<GameObject, int>();
        if (GamemodeController.Players != null)
            foreach (GameObject player in GamemodeController.Players)
            {
                if (player != null)
                {
                    m_playersPoints.Add(player, m_settings.PointsToCapture);
                    m_playerTimers.Add(player, player.GetComponent<RoundTimer>());
                    m_playerTimers[player].SetTimeRemaining(m_settings.CountdownTime);
                    m_playerTimers[player].SetStartTime(m_settings.CountdownTime);
                    m_playerTimers[player].TimeIsUp += Time_Up;
                    m_playerTimers[player].ShowTimer();
                }
            }
    }

    #endregion

    private void Time_Up(object _Sender, EventArgs _Args)
    {
        RoundTimer pTimer = (RoundTimer)_Sender;
        GameObject sender = null;
        if (m_playerTimers.Count > 0)
            foreach (var playerTimers in m_playerTimers)
            {
                if (pTimer == playerTimers.Value)
                    sender = playerTimers.Key;
            }

        // If sent from valid sender
        if (sender != null)
            m_playersPoints[sender]--;
        StopAllTimers();    // Game is over, disable timers
        GameObject.FindGameObjectWithTag("Announcer")?.GetComponent<AnnouncerChatter>()?.Play_PointCaptured();
        // Need to handle checking the win condition from the gamemode controller
        GamemodeController.CheckWinCondition();
    }

    private void StopAllTimers()
    {
        if (GamemodeController.Players != null)
            foreach (GameObject player in GamemodeController.Players)
            {
                if (player != null)
                    m_playerTimers[player]?.Btn_StopTimer();
            }
    }

    /// <summary> Reset all timers to starting time. </summary>
    private void ResetAllTimers()
    {
        if (GamemodeController.Players != null)
            foreach (GameObject player in GamemodeController.Players)
                if (player != null)
                    m_playerTimers[player]?.ResetTimer();
    }
}