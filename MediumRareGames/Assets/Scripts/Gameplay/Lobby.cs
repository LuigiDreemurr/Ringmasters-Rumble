/*
-----------------------------------------------------------------------------
       Created By Chris Knight
-----------------------------------------------------------------------------
   Lobby
       - Keeps track of all connected controllers and handle lobbying them for a game

   Details:
       - Tracks all controls registered to the lobby
       - Detects connecting/disconnecting controllers
       - Allows for altering individual "players" as controllers technical details within the lobby
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviour
{
    public string gameScene;

    public static string[] connectedJoysticks = new string[4];
    private static List<Player> players = new List<Player>(); //all connected players
    private Player currentHost;

    public static Player[] GetLobbiedPlayers()
    {
        return players.ToArray();
    }

    #region Public Methods/Properties
    /// <summary>
    /// Gets the player specified by their joystick number. Returns null if no player with that joystick number is found.
    /// </summary>
    /// <param name="controllerIndex">Joystick number of the player your trying to find.</param>
    /// <returns></returns>
    public Player GetPlayer(int controllerIndex) { return FindPlayerByIndex(controllerIndex); }

    /// <summary>
    /// Gets a list of all players in the lobby that are not yet ready. Returns a list of 0 elements if no unready players are found.
    /// </summary>
    public Player[] GetUnreadyPlayers() { return FindUnreadyPlayers(); }

    /// <summary>
    /// Gets the number of players currently in the lobby.
    /// </summary>
    public int PlayersInLobby
    {
        get { return players.Count; }
    }

    /// <summary>
    /// Checks if every player in the lobby is ready. Returns false if 1 or more player(s) are not ready.
    /// </summary>
    public bool AllPlayersReady
    {
        get { return (GetUnreadyPlayers().Length == 0); }
    }

    /// <summary>
    /// Gets the player who is currently the game host.
    /// </summary>
    public Player PlayerGameHost
    {
        get { return currentHost; }
    }
    #endregion

    #region Public Commands
    public void ClearLobby()
    {
        players.Clear();
    }

    /// <summary>
    /// Connects the specified player by joystick number to the lobby.
    /// </summary>
    /// <param name="controllerIndex">Joystick number of the player you are trying to connect to the lobby.</param>
    public void JoinLobby(int controllerIndex)
    {
        if (GetPlayer(controllerIndex) == null) //check if player is already in lobby
        {
            //add player to lobby
            players.Add(new Player(controllerIndex));
            Debug.Log("Player " + controllerIndex + " has joined the lobby.");
        }
        else
        {
            Debug.Log("Player " + controllerIndex + " is already in the lobby.");
        }
    }

    /// <summary>
    /// Reassigns the game host title to the specified player by joystick number.
    /// </summary>
    /// <param name="controllerIndex">Joystick number of the player your trying to set as the new host.</param>
    public void SetGameHost(int controllerIndex)
    {
        FindPlayerByIndex(controllerIndex).SetAsGameHost(ref currentHost);
        Debug.Log("Player " + controllerIndex + " is now the Game Host.");
    }
    #endregion

    #region Helper Functions
    /// <summary>
    /// Finds the player by their joystick number (index)
    /// </summary>
    /// <param name="index">Joystick number of the player in question.</param>
    /// <returns></returns>
    Player FindPlayerByIndex(int index)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].controller == index) { return players[i]; }
        }

        return null;
    }

    /// <summary>
    /// Finds players with "isReady" set false (returns array of "unready" players)
    /// </summary>
    Player[] FindUnreadyPlayers()
    {
        List<Player> unreadyPlayers = new List<Player>();

        for (int i = 0; i < players.Count; i++)
        {
            if (!players[i].isReady) { unreadyPlayers.Add(players[i]); }
        }

        return unreadyPlayers.ToArray();
    }
    #endregion

    private void FixedUpdate()
    {
        //Get the last 4 (relevate) inputs from JoystickNames
        if(Input.GetJoystickNames().Length <= 4)
        {
            connectedJoysticks = Input.GetJoystickNames();
        }
        else
        {
            for(int i = Input.GetJoystickNames().Length - 4; i < Input.GetJoystickNames().Length; i++)
            {
                for(int e = 0; e < 4; e++)
                {
                    connectedJoysticks[e] = Input.GetJoystickNames()[i];
                }
            }
        }

        //make sure theres always a host (in case the current one disconnects)
        if (players.Count > 0)
        {
            if (currentHost == null) { SetGameHost(players[0].controller); }
            else if(!currentHost.isHost)
            {
                //search all connected players
                for(int i = 0; i < players.Count; i++)
                {
                    //set the host to the top-most connected player
                    if (players[i].connected) { SetGameHost(players[i].controller); }
                }
            }
        }

        //check the connection of every player
        foreach (Player player in players)
        {
            player.SetJoystickName(this, connectedJoysticks[player.controller - 1]); //get the joystick name of the controller

            if (player.connected)
            {
                if (string.IsNullOrEmpty(player.JoystickName)) //if the joystick name is lost...
                {
                    player.Disconnected();
                    print(player.playerName + " has disconnected!");
                }
            }
            else //player no longer connected
            {
                if (!string.IsNullOrEmpty(player.JoystickName)) //if the joystick name is found...
                {
                    player.Reconnected();
                    print(player.playerName + " has reconnected!");
                }
            }
        }
    }
}

[System.Serializable]
public class Player
{
    public string playerName; //Player (Joystick number)
    public int controller; //joystick number
    public bool isReady;
    public bool isHost;

    public Color baseColor; //set when the player connects/reconnects, the color of their background slot

    #region Properties
    public bool connected { get; protected set; }
    public string JoystickName { get; protected set; }

    /// <summary>
    /// Set the joystick name internally (should only be allowed to be done by this script...)
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="name"></param>
    public void SetJoystickName(object sender, string name)
    {
        //only if the sender is this script, allow setting...
        if (sender.GetType() == typeof(Lobby))
        {
            JoystickName = name;
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Constructor - setup the players data based on the joystick number.
    /// </summary>
    /// <param name="controllerIndex">Joystick number of this player/controller.</param>
    public Player(int controllerIndex)
    {
        playerName = "Player " + controllerIndex;
        controller = controllerIndex;

        connected = true;
        JoystickName = Input.GetJoystickNames()[controllerIndex - 1];
    }

    /// <summary>
    /// Reassign the current game host to this player/controller.
    /// </summary>
    /// <param name="currentHost">Reference to the current game host to reset.</param>
    public void SetAsGameHost(ref Player currentHost)
    {
        isHost = true;
        if (currentHost == null) { currentHost = this; return; } //if no previous host, become the new host automatically

        //assign self as the new host (and unassign the previous host)
        currentHost.isHost = false;
        currentHost = this;
    }

    /// <summary>
    /// If the player has disconnected their controller, disconnect them from the lobby.
    /// If the player was the host, they will automatically be resigned and a new host will be assigned.
    /// </summary>
    public void Disconnected()
    {
        connected = false;
        isReady = false;
        isHost = false;

        JoystickName = null;
    }

    /// <summary>
    /// If the player has reconnected their controller, reconnect them to the lobby.
    /// If the player was the host, they do not get their title reassigned.
    /// </summary>
    public void Reconnected()
    {
        connected = true;

        JoystickName = Input.GetJoystickNames()[controller - 1];
    }

    /// <summary>
    /// Toggles this controllers ready status.
    /// </summary>
    public void ToggleReady()
    {
        isReady = !isReady;
    }
    #endregion
}
