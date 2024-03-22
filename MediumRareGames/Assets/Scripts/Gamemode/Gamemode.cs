/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
----------------------------------------------------------------------------- 
   Gamemode (abstract)
       - The abstract class for all gamemode types (LMS/KotH)

   Details:
       - Derived classes should not implement their own MonoBehaviour Start(),
         they should override Initialize() instead
       - Gamemodes should call the GamemodeController.CheckWinConditions() in their
         own OnDeath implementation
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Health;
using System;

public abstract class Gamemode : MonoBehaviour
{
    public enum Type { LMS, KotH }

    private GamemodeController m_gamemodeController;
    public GamemodeController GamemodeController { get { return m_gamemodeController; } }

    /// <summary>Initialization</summary>
    protected void Start()
    {
        m_gamemodeController = GetComponent<GamemodeController>();

        Initialize();

        //Subscribe to each player's death event
        if (GamemodeController.Players != null)
            foreach (GameObject player in GamemodeController.Players)
            {
                if (player != null)
                    player.GetComponent<DeathHandler>().Health.OnDeath += OnDeath;
            }
    }

    /// <summary>Is the round done according to the specific gamemode</summary>
    /// <returns>Returns true if the round is over</returns>
    public abstract bool IsRoundOver();

    /// <summary>The winner of the round in its current state</summary>
    /// <returns>Returns the player who 'won' the round</returns>
    public abstract GameObject GetWinner();

    /// <summary>Gamemode Initialization (use instead of Start())</summary>
    protected abstract void Initialize();

    /// <summary>What the gamemode does when a player dies</summary>
    /// <param name="_Health">The player health that died</param>
    /// <param name="_DamageSource">The damage source of player death</param>
    protected abstract void OnDeath(Health.Health _Health, DamageSource _DamageSource);
}