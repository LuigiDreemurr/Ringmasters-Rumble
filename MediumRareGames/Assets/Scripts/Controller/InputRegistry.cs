/*
-----------------------------------------------------------------------------
       Created By Chris Knight
-----------------------------------------------------------------------------
   InputRegistry
       - Registers input from the controller, using Controller.cs for sending & reciving events/events data.

   Details:
       - Subscribes to the input events of Controller.cs
       - Registers functions based on input provided from events.

    Possible Problems:
        - Reconnecting multiple controllers constantly with Unity open can populate Input.GetJoystickNames with blank entries never filled or cleared by Unity.
        - This could possibly make the array return numbers larger than 4, and always set connected/reconnected controlls to a new last index.
        - The problem with this, is that it cant be edited, and will keep the index blank for the disconnected controller so it can re-populate the index when
        connected again.
-----------------------------------------------------------------------------
*/

using ConsoleLogging;
using Controller;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputRegistry : MonoBehaviour
{
    #region Binds for Icon Display
    public Button prevMask = Button.LeftBumper;
    public Button nextMask = Button.RightBumper;
    #endregion

    Controller.Controller input;
    Lobby lobby;
    LobbyCommands lobbyCommands;
    ControllerUI ui;
    LobbyMapSelect mapSelect;

    int joystick;
    Player thisController;

    #region Controller Input Registry
    // Use this for initialization
    void Start()
    {
        input = GetComponent<Controller.Controller>();
        lobby = FindObjectOfType<Lobby>();
        ui = FindObjectOfType<ControllerUI>();
        mapSelect = FindObjectOfType<LobbyMapSelect>();

        joystick = input.Joystick;

        //Reserve this controllers slot in the static controller array
        ControllerUI.controllers[joystick - 1] = this;

        #region Input Subscription
        //Subscribe to each button
        input.Subscribe(Button.A, LobbyButtonPressA);
        input.Subscribe(Button.Start, LobbyButtonPressStart);

        input.Subscribe(Button.LeftBumper, LobbyButtonPressLB);
        input.Subscribe(Button.RightBumper, LobbyButtonPressRB);
        #endregion

        ////Subscribe to each button
        //foreach (Button button in Enum.GetValues(typeof(Button)))
        //{
        //    input.Subscribe(button, ButtonPress);
        //}

        //Subscribe to each single axis
        foreach (Axis axis in Enum.GetValues(typeof(Axis)))
        {
            input.Subscribe(axis, SingleAxis);
        }

        //Subscribe to each dual axis
        foreach (DualAxis dualAxis in Enum.GetValues(typeof(DualAxis)))
        {
            input.Subscribe(dualAxis, DualAxis);
        }

    }

    void LobbyButtonPressA(object s, ButtonEventArgs args)
    {
        if (lobby != null) //Player is in Lobby, these buttons will be registered.
        {
            if (args.State == ButtonInputState.Down)
            {
                //Lobby Player
                if (thisController != null)
                {
                    //Ready/Unready
                    thisController.ToggleReady();
                }
                else
                {
                    //Join lobby
                    lobby.JoinLobby(joystick);
                    thisController = lobby.GetPlayer(joystick);
                    lobbyCommands = new LobbyCommands(lobby);
                    lobbyCommands.thisController = thisController;
                }
            }
        }
    }

    void LobbyButtonPressStart(object s, ButtonEventArgs args)
    {
        if (lobby != null) //Player is in Lobby, these buttons will be registered.
        {
            if (args.State == ButtonInputState.Down)
            {
                //Register other inputs once the controller is connected to the lobby
                if (thisController != null)
                {
                    if (thisController.isReady) //inputs that should ONLY be registered if the player IS ready.
                    {
                        //Start Game
                        if (ui.GetCurrentNavigationCanvas() == ControllerUI.Navigation.Menu)
                        {
                            lobbyCommands.SelectLevel(ui);
                        }
                        else if (ui.GetCurrentNavigationCanvas() == ControllerUI.Navigation.MapSelect)
                        {
                            lobbyCommands.StartGame();
                        }
                    }
                }
                //Player not connected to lobby while trying to register keys...
                else { print("Player " + joystick + ": Press A to join the lobby."); }
            }
        }
    }

    void LobbyButtonPressLB(object s, ButtonEventArgs args)
    {
        if (lobby != null) //Player is in Lobby, these buttons will be registered.
        {
            if (args.State == ButtonInputState.Down)
            {
                //Register other inputs once the controller is connected to the lobby
                if (thisController != null)
                {
                    if (!thisController.isReady) //inputs that should ONLY be registered if the player is NOT ready.
                    {
                        //Navigate Mask Selection (Prev)
                        if (args.Button == Button.LeftBumper)
                        {
                            if (ui.GetCurrentNavigationCanvas() == ControllerUI.Navigation.Menu)
                            {
                                ui.BtnPrevMask(joystick);
                            }
                        }
                    }
                    else //player IS ready
                    {
                        if (ui.GetCurrentNavigationCanvas() == ControllerUI.Navigation.MapSelect && thisController.isHost)
                        {
                            mapSelect.SelectMap(false);
                        }
                    }
                }
                //Player not connected to lobby while trying to register keys...
                else { print("Player " + joystick + ": Press A to join the lobby."); }
            }
        }
    }

    void LobbyButtonPressRB(object s, ButtonEventArgs args)
    {
        if (lobby != null) //Player is in Lobby, these buttons will be registered.
        {
            if (args.State == ButtonInputState.Down)
            {
                //Register other inputs once the controller is connected to the lobby
                if (thisController != null)
                {
                    if (!thisController.isReady) //inputs that should ONLY be registered if the player is NOT ready.
                    {
                        //Navigate Mask Selection (Next)
                        if (args.Button == Button.RightBumper)
                        {
                            if (ui.GetCurrentNavigationCanvas() == ControllerUI.Navigation.Menu)
                            {
                                ui.BtnNextMask(joystick);
                            }
                        }
                    }
                    else //player IS ready
                    {
                        if (ui.GetCurrentNavigationCanvas() == ControllerUI.Navigation.MapSelect && thisController.isHost)
                        {
                            mapSelect.SelectMap(true);
                        }
                    }
                }
                //Player not connected to lobby while trying to register keys...
                else { print("Player " + joystick + ": Press A to join the lobby."); }
            }
        }
    }

    ////Button event registry
    //void ButtonPress(object s, ButtonEventArgs args)
    //{
    //    if (lobby != null) //Player is in Lobby, these buttons will be registered.
    //    {
    //        if (args.State == ButtonInputState.Down)
    //        {
    //            //Lobby Player
    //            if (args.Button == Button.A)
    //            {
    //                if(thisController != null)
    //                {
    //                    //Ready/Unready
    //                    thisController.ToggleReady();
    //                }
    //                else
    //                {
    //                    //Join lobby
    //                    lobby.JoinLobby(joystick);
    //                    thisController = lobby.GetPlayer(joystick);
    //                    lobbyCommands = new LobbyCommands(lobby);
    //                    lobbyCommands.thisController = thisController;
    //                }
    //            }

    //            //Register other inputs once the controller is connected to the lobby
    //            if (thisController != null)
    //            {
    //                if (thisController.isReady) //inputs that should ONLY be registered if the player IS ready.
    //                {
    //                    //Start Game
    //                    if (args.Button == Button.Start)
    //                    {
    //                        lobbyCommands.StartGame(lobby.gameScene);
    //                    }
    //                }
    //                else //inputs that should ONLY be registered if the player is NOT ready.
    //                {
    //                    //Navigate Mask Selection
    //                    if (args.Button == Button.LeftBumper)
    //                    {
    //                        ui.BtnPrevMask(joystick);
    //                    }
    //                    else if (args.Button == Button.RightBumper)
    //                    {
    //                        ui.BtnNextMask(joystick);
    //                    }
    //                }
    //                //inputs that should be registered regardless if the player is ready or not.

    //                //Become Lobby Host
    //                if (args.Button == Button.Y)
    //                {
    //                    //lobbyCommands.ToggleHostSettings();
    //                    lobby.SetGameHost(joystick);
    //                }
    //            }
    //            //Player not connected to lobby while trying to register keys...
    //            else { print("Player " + joystick + ": Press A to join the lobby."); }

    //            //Debug.Log(joystick + " " + args.Button.ToString());
    //        }
    //    }
    //}

    //Single axis event registry
    void SingleAxis(object s, AxisEventArgs args)
    {
        if (Mathf.Abs(args.Value) > 0)
        {
            //Debug.Log(joystick + " " + args.Axis.ToString() + " : " + args.Value.ToString());
        }
    }

    //Dual axis event registry
    void DualAxis(object s, DualAxisEventArgs args)
    {
        if (args.Value.magnitude > 0)
        {
            //Debug.Log(joystick + " " + args.Axis.ToString() + " : " + args.Value.ToString());
        }
    }
    #endregion

    class LobbyCommands
    {
        public Player thisController;
        public Lobby lobby;

        public LobbyCommands(Lobby currentLobby)
        {
            lobby = currentLobby;
        }

        /// <summary>
        /// Start the game from the lobby.
        /// Only the Game Host can start the game.
        /// </summary>
        public void StartGame()
        {
            if (lobby.PlayerGameHost == thisController) //if this controller is the game host...
            {
                if (lobby.PlayersInLobby >= 2)
                {
                    if (lobby.AllPlayersReady) //start the game
                    {
                        print("Host (" + thisController.playerName + ") has started the game!");
                        SceneManager.LoadScene(LobbyMapSelect.selectedMap.sceneName);
                    }
                    else //report players not ready yet
                    {
                        print("A game cannot start until all players are ready:");
                        foreach (Player player in lobby.GetUnreadyPlayers())
                        {
                            print(player.playerName + " is not ready!");
                        }
                    }
                }
                else //less than 2 people in the lobby
                {
                    print("Not enough players to start a game. Minium Players Required: 2");
                }
            }
            else //if this controller is NOT the game host...
            {
                print("You must be the game host to start the game!");
            }
        }

        /// <summary>
        /// Display the level/stage/map selection screen.
        /// Only the Game Host can select levels.
        /// </summary>
        public void SelectLevel(ControllerUI ui)
        {
            if (lobby.PlayerGameHost == thisController) //if this controller is the game host...
            {
                if (lobby.PlayersInLobby >= 2)
                {
                    if (lobby.AllPlayersReady) //start the game
                    {
                        print("Host (" + thisController.playerName + ") has started the game!");
                        //SceneManager.LoadScene(targetScene);

                        ui.NavigateTo(ControllerUI.Navigation.MapSelect);
                    }
                    else //report players not ready yet
                    {
                        print("A game cannot start until all players are ready:");
                        foreach (Player player in lobby.GetUnreadyPlayers())
                        {
                            print(player.playerName + " is not ready!");
                        }
                    }
                }
                else //less than 2 people in the lobby
                {
                    print("Not enough players to start a game. Minium Players Required: 2");
                }
            }
            else //if this controller is NOT the game host...
            {
                print("You must be the game host to start the game!");
            }
        }

        public void ToggleHostSettings()
        {
            print("Host: " + lobby.PlayerGameHost.playerName);
        }
    }
}
