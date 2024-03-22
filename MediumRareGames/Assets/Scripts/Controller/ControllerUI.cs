/*
-----------------------------------------------------------------------------
       Created By Chris Knight
-----------------------------------------------------------------------------
   ControllerUI
       - Listens to the Lobby connected players and updates the UI accordingly.

   Details:
       - Setup the Inspector (after attaching the script to something, the object doesnt matter)
       - The UI will update itself with the given paramaters from the Inspector, as controller input is registered/changed in the Lobby.

-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum XboxIcons
{
    A = 2,
    B = 3,
    X = 4,
    Y = 5,
    LB = 8,
    RB = 9,
    LT = 6,
    RT = 7,
    Menu = 1,
    Start = 0,
    DPad_Left = 12,
    DPad_Right = 13,
    DPad_Up = 14,
    DPad_Down = 11,
    RS_Press = 18,
    RS_Left = 20,
    RS_Right = 21,
    RS_Up = 22,
    RS_Down = 19,
    RS_UpDown = 24,
    RS_LeftRight = 25,
    LS_Press = 10,
    LS_Left = 28,
    LS_Right = 29,
    LS_Up = 30,
    LS_Down = 27,
    LS_UpDown = 32,
    LS_LeftRight = 33
};

public class ControllerUI : MonoBehaviour {

    public Canvas menu, mapSelect;
    public enum Navigation { None, Menu, MapSelect };
    private Canvas currentCanvas;

    public static InputRegistry[] controllers = new InputRegistry[4];

    public Lobby lobby;
    public Text lobbyDisplay;
    public Sprite[] characterMasks;

    [Space]

    public ControllerHUD[] connections = new ControllerHUD[4];
    public Sprite unregistered, connected, disconnected;
    public Color defaultBGColor, defaultStatusColor, readyColor, unreadyColor;

    private static Sprite[] xboxIcons;

    [System.Serializable]
    public class ControllerHUD
    {
        public Color BGColor;
        public Text name, readyStatus;
        public Image bg, lobbyIcon, connectionStatus;
        public Button prevMask, nextMask;
        public ButtonIcon prevIcon, nextIcon;

        [HideInInspector]
        public int maskIndex;
        [HideInInspector]
        public bool maskSelected;
    }

    public void NavigateTo(Navigation nav)
    {
        menu.enabled = false;
        mapSelect.enabled = false;

        if(nav == Navigation.Menu) { menu.enabled = true; currentCanvas = menu; }
        else if (nav == Navigation.MapSelect) { mapSelect.enabled = true; currentCanvas = mapSelect; }
    }

    public Navigation GetCurrentNavigationCanvas()
    {
        if(currentCanvas == menu) { return Navigation.Menu; }
        else if (currentCanvas == mapSelect) { return Navigation.MapSelect; }

        return Navigation.None;
    }

    /// <summary>
    /// Get the corresponding icon relative to the Xbox Icon button passed.
    /// </summary>
    /// <param name="buttonIcon">The Xbox Icon button to get the icon for.</param>
    /// <returns></returns>
    public static Sprite GetIcon(XboxIcons buttonIcon)
    {
        return xboxIcons[(int)buttonIcon];
    }

    private void Awake()
    {
        xboxIcons = Resources.LoadAll<Sprite>("Controller Icons/xbox icons");
    }

    private void Start()
    {
        lobby.ClearLobby();
        NavigateTo(Navigation.Menu);
    }

    void Update () {
        //update lobby status
        lobbyDisplay.text = "Players in Lobby: " + lobby.PlayersInLobby + "\n";

        if (lobby.PlayersInLobby >= 2) //if lobby has 2+ players, display relevant lobby status
        {
           lobbyDisplay.text += (lobby.AllPlayersReady) ? "All Players <b>READY</b>!" : "Waiting for players to ready up...";
        }
        else //otherwise, wait for 2+ players
        {
            lobbyDisplay.text += "(Waiting for players...)";
        }

        //update UI panels for each connected controller
        for(int i = 0; i < connections.Length; i++)
        {
            int controllerIndex = i + 1;

            //if not connected...
            if (lobby.GetPlayer(controllerIndex) == null || string.IsNullOrEmpty(Lobby.connectedJoysticks[i]))
            {
                connections[i].bg.color = defaultBGColor;
                connections[i].readyStatus.color = defaultStatusColor;
                connections[i].name.text = "NOT CONNECTED";
                connections[i].readyStatus.text = "SLOT AVAILABLE";
                connections[i].lobbyIcon.sprite = unregistered;
                connections[i].connectionStatus.sprite = disconnected;

                connections[i].prevMask.interactable = false;
                connections[i].nextMask.interactable = false;
                connections[i].prevIcon.visible = false;
                connections[i].nextIcon.visible = false;
            }
            //if connected...
            else
            {
                connections[i].bg.color = connections[i].BGColor;
                connections[i].readyStatus.color = (lobby.GetPlayer(controllerIndex).isReady) ? readyColor: unreadyColor;
                connections[i].name.text = lobby.GetPlayer(controllerIndex).playerName;
                connections[i].readyStatus.text = (lobby.GetPlayer(controllerIndex).isReady) ? "READY" : "NOT READY";
                if (lobby.GetPlayer(controllerIndex).isHost) { connections[i].readyStatus.text += "\n(HOST)"; }
                if (!connections[i].maskSelected) { SelectRandomMask(ref connections[i]); }
                connections[i].connectionStatus.sprite = (lobby.GetPlayer(controllerIndex).connected) ? connected : disconnected;

                connections[i].prevMask.interactable = true;
                connections[i].nextMask.interactable = true;
                connections[i].prevIcon.ChangeButtonIcon(ButtonIcon.ConvertInput(controllers[controllerIndex].prevMask), true, "");
                connections[i].nextIcon.ChangeButtonIcon(ButtonIcon.ConvertInput(controllers[controllerIndex].nextMask), true, "");

                lobby.GetPlayer(controllerIndex).baseColor = connections[i].BGColor;
            }
        }
	}

    void SelectRandomMask(ref ControllerHUD player)
    {
        player.maskIndex = Random.Range(0, characterMasks.Length);
        player.lobbyIcon.sprite = characterMasks[player.maskIndex];
        player.maskSelected = true;
    }

    /// <summary>
    /// Select the previous mask for this controller.
    /// </summary>
    /// <param name="joystick">Joystick to update this UI's mask.</param>
    public void BtnPrevMask(int joystick)
    {
        joystick -= 1; //keep joystick in a 0-based index

        //naviagte to previous mask index
        if (connections[joystick].maskIndex > 0)
        {
            connections[joystick].maskIndex--;
        }
        //loop index to end if no previous masks
        else
        {
            connections[joystick].maskIndex = characterMasks.Length - 1;
        }
        
        connections[joystick].lobbyIcon.sprite = characterMasks[connections[joystick].maskIndex];
    }

    /// <summary>
    /// Select the next mask for this controller.
    /// </summary>
    /// <param name="joystick">Joystick to update this UI's mask.</param>
    public void BtnNextMask(int joystick)
    {
        joystick -= 1; //keep joystick in a 0-based index

        //naviagte to next mask index
        if (connections[joystick].maskIndex < characterMasks.Length - 1)
        {
            connections[joystick].maskIndex++;
        }
        //loop index to start if no next masks
        else
        {
            connections[joystick].maskIndex = 0;
        }

        connections[joystick].lobbyIcon.sprite = characterMasks[connections[joystick].maskIndex];
    }
}
