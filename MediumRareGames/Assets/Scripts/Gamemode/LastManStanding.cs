using System;
using System.Collections;
using System.Collections.Generic;
using Health;
using UnityEngine;
using ConsoleLogging;

public class LastManStanding : Gamemode
{
    #region Data Members

    private Dictionary<GameObject, int> m_playerLives; // Dictionary representing each player's lives

    private Settings.LastManStanding m_settings;

    #endregion

    #region Private Methods
    /// <summary> Gets all the players that still have lives left </summary>
    /// <returns> The list of players who have at least 1 life </returns>
    private List<GameObject> AlivePlayers()
    {
        List<GameObject> playersAlive = new List<GameObject>(4);
        foreach (var playerLives in m_playerLives)
        {
            if (playerLives.Value > 0)
                playersAlive.Add(playerLives.Key);
        }
        return playersAlive;
    }
    #endregion


    #region Gamemode Implementation
    /// <summary> Last man standing is considered over when there is 1 player alive </summary>
    /// <returns> Returns true if there is 1 player alive </returns>
    public override bool IsRoundOver()
    {
        // Find how many players are still alive
        int aliveCount = 0;
        foreach (var playerLives in m_playerLives)
            if (playerLives.Value > 0)
                aliveCount++;

        // Round is over when there is only 1 player left alive
        return aliveCount == 1;
    }

    /// <summary> The winner of last man standing is based on whoever has the most lives left </summary>
    /// <returns> The player with the most lives. Or null </returns>
    public override GameObject GetWinner()
    {
        List<GameObject> alive = AlivePlayers();

        if (alive.Count == 0)
            Log.Error(this, "Somehow tried to get a winner with 0 players still alive");

        GameObject winner = alive[0];

        // Loop through players still alive
        foreach (GameObject player in alive)
        {
            // Skip the winner that was set
            if (player == winner)
                continue;

            // Find player with the most lives
            if (m_playerLives[player] > m_playerLives[winner])
                winner = player;
        }

        // With a player with the most lives we need to check if there is a tie
        foreach (GameObject player in alive)
        {
            if (player == winner)
                continue;

            // Find player with the most lives
            if (m_playerLives[player] == m_playerLives[winner])
                winner = null;
        }

        return winner;
    }

    /// <summary> Respawn the player if they have any lives left </summary>
    /// <param name="_Health"> The player health </param>
    /// <param name="_DamageSource"> The source of the death </param>
    protected override void OnDeath(Health.Health _Health, DamageSource _DamageSource)
    {
        GameObject parent = _Health.transform.parent.gameObject;
        m_playerLives[parent]--; // Decrement lives

        // If there are lives left, respawn
        if (m_playerLives[parent] > 0)
            parent.GetComponent<DeathHandler>().Respawn(m_settings.RespawnTime); // Respawn player
        // No lives left, just disable all together
        else
            parent.SetActive(false);

        // Need to handle checking the win condition from the gamemode controller
        GamemodeController.CheckWinCondition();
    }

    /// <summary> Setup the player lives tracking </summary>
    protected override void Initialize()
    {
        m_settings = GlobalSettings.Get.Gamemode.LastManStanding;
        gameObject.GetComponent<GamemodeController>().SetRoundTimer(m_settings.TimeLimit);
        // TODO: Does not change timer until after countdown, might need a way to add placeholder text

        // Setup player lives dictionary (all players start with m_startingLives)
        m_playerLives = new Dictionary<GameObject, int>();
        if (GamemodeController.Players != null)
            foreach (GameObject player in GamemodeController.Players)
            {
                if (player != null)
                {
                    player.GetComponentInChildren<RoundTimer>()?.KillSwitch();
                    player.GetComponentInChildren<RoundTimer>()?.HideTimer();
                    m_playerLives.Add(player, m_settings.Lives);
                }
            }
    }
    #endregion
}