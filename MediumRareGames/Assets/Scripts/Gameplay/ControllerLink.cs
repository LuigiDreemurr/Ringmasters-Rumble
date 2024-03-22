/*
-----------------------------------------------------------------------------
       Created By Chris Knight
-----------------------------------------------------------------------------
   ControllerLink
       - Sets up the players controllers for gameplay, based on Lobby settings.

   Details:
       - Spawns players
       - Links the spawned player to the controller from the Lobby
-----------------------------------------------------------------------------
*/

using ConsoleLogging;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerLink : MonoBehaviour
{
    public GameObject playerPrefab;

    public GameObject[] players;

    public MenuController menuController;

<<<<<<< HEAD
    // Use this for initialization
    void Awake()
=======
    private void Awake()
>>>>>>> 3a9fa6156bf973d233c242c1f98d1255f96947c7
    {
        players = new GameObject[4];
    }

    // Use this for initialization
    void Start()
    {
        SetupPlayers();
    }

    /// <summary>
    /// Setup the players for gameplay and link them to their corresponding controllers.
    /// </summary>
    public void SetupPlayers()
    {
        Player[] lobbiedPlayers = Lobby.GetLobbiedPlayers();
        GameObject[] playerGameObjects = new GameObject[lobbiedPlayers.Length];

        //Get spawn points once before the loop
        List<Level.Tile> spawnPoints = DeathHandler.GetSpawnPoints();

        for (int i = 0; i < 4; i++)
        {
            try
            {
                if (!string.IsNullOrEmpty(Lobby.connectedJoysticks[i])) //player connected to loby
                {
                    //spawn player
                    GameObject player = (GameObject)Instantiate(playerPrefab);
                    playerGameObjects[i] = player;

                    //link player to controller
                    player.transform.GetChild(0).GetComponent<Controller.Controller>().Joystick = lobbiedPlayers[i].controller;
                    player.name = lobbiedPlayers[i].playerName;

                    //Position the player at a random spawn point
                    //Note: May run into issues of multiple players spawning on the same point
                    Vector3 spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)].transform.position;
                    player.GetComponent<DeathHandler>().Spawn(spawnPoint);

                    //set the color of the player to their background color from lobby
                    player.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = lobbiedPlayers[i].baseColor;

                    players[i] = player;
                    Log.Message(this, "<color=green>(" + lobbiedPlayers[i].playerName + " is now linked)</color>");
                }
                else //player disconnected from lobby
                {
                    Log.Warning(this, "<color=grey>(Player " + (i + 1) + " is not connected)</color>");
                }
            }
            catch (IndexOutOfRangeException) //player was never in the lobby
            {
                Log.Warning(this, "<color=grey>(Player " + (i + 1) + " is not connected)</color>");
            }
        }

        if (menuController != null)
            menuController.SetPlayers(playerGameObjects);

        Log.Message(this, "All controllers linked to their players.");
    }
}
