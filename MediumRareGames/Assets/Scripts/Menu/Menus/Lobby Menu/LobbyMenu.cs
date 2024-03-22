using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInput;
using XInputDotNetPure;

public class LobbyMenu : Menu<LobbyMenu>
{
    private static List<PlayerInfo> s_players;
    public static List<PlayerInfo> Players { get { return s_players; } }

    [SerializeField] private Button m_joinButton = Button.A;
    [SerializeField] private Button m_leaveButton = Button.B;
    [SerializeField] private Button m_scrollLeftButton = Button.LeftBumper;
    [SerializeField] private Button m_scrollRightButton = Button.RightBumper;
    [SerializeField] private Button m_startButton = Button.Start;
    [Space]
    [SerializeField] private LobbyPlayerSlot[] m_slots = new LobbyPlayerSlot[4];
    [SerializeField] private List<CharacterHead> m_availableCharacters = new List<CharacterHead>(5);
    [SerializeField] private UnityEngine.UI.Text m_displayText;
    [SerializeField] private Menu m_nextMenu;
    [Space]
    [SerializeField] private int m_readyCountRequired = 2;
    private int m_readyCount;
    private int m_lobbyCount;
    [Space]
    [SerializeField] private Vector2 m_nonReadyVibration = new Vector2(0.3f, 0.3f);
    [SerializeField] private float m_nonReadyVirbrateTime = 0.3f;

    public Button JoinButton { get { return m_joinButton; } }
    public Button LeaveButton { get { return m_leaveButton; } }
    public Button ScrollLeftButton { get { return m_scrollLeftButton; } }
    public Button ScrollRightButton { get { return m_scrollRightButton; } }
    public List<CharacterHead> AvailableCharacters { get { return m_availableCharacters; } }

    public int ReadyCount
    {
        get { return m_readyCount; }
        set { m_readyCount = value; }
    }

    public int LobbyCount
    {
        get { return m_lobbyCount; }
        set { m_lobbyCount = value; }
    }

    public void RemoveCharacter(CharacterHead _Character)
    {
        m_availableCharacters.Remove(_Character);

        foreach(LobbyPlayerSlot slot in m_slots)
        {
            if (!slot.IsLocked && slot.PlayerInfo.Character == _Character)
                slot.RandomizeCharacter();
        }
    }

    public void AddCharacter(CharacterHead _Character)
    {
        m_availableCharacters.Add(_Character);
    }

    public override void OnShow()
    {
        ControllerManager controllerManager = ControllerManager.Instance;

        controllerManager.OnConnect += ControllerConnected;
        controllerManager.OnDisconnect += ControllerDisconnected;

        //Check for already connected controllers
        foreach(PlayerIndex playerIndex in Enum.GetValues(typeof(PlayerIndex)))
        {
            XInput.Controller controller = controllerManager.GetController(playerIndex);
            if (controller.IsConnected)
                m_slots[(int)controller.Index].Init();
        }
    }

    public override void OnHide()
    {
        ControllerManager controllerManager = ControllerManager.Instance;

        controllerManager.OnConnect -= ControllerConnected;
        controllerManager.OnDisconnect -= ControllerDisconnected;

        //Check for connected controllers when hiding menu to unhook
        foreach (PlayerIndex playerIndex in Enum.GetValues(typeof(PlayerIndex)))
        {
            XInput.Controller controller = controllerManager.GetController(playerIndex);
            if (controller.IsConnected)
                m_slots[(int)controller.Index].Clean(false);
        }
    }

    private void ControllerConnected(XInput.Controller _Controller)
    {
        m_slots[(int)_Controller.Index].Init();
    }

    private void ControllerDisconnected(XInput.Controller _Controller)
    {
        m_slots[(int)_Controller.Index].Clean(true);
    }

    private bool CanPlay()
    {
        //All lobbied players are locked in. And there are enough players
        return m_lobbyCount == m_readyCount && m_readyCount >= m_readyCountRequired;
    }

    private void Update()
    {
        if(CanPlay())
        {
            XInput.Controller inControl = GetFirstLocked();
            m_displayText.text = "Player " + inControl.Index.ToString() + " press Start to continue";
            if (inControl.GetButton(m_startButton).Action == ButtonAction.Down)
            {
                s_players = GetLockedPlayers();
                MenuManager.Instance.ShowMenu(m_nextMenu);
                MenuManager.Instance.MenuController = inControl;
            }
        }
        else
        {
            m_displayText.text = "Waiting for players";

            XInput.Controller inControl = GetFirstLocked();
            if(inControl != null)
            {
                if (inControl.GetButton(m_startButton).Action == ButtonAction.Down)
                {
                    foreach(LobbyPlayerSlot slot in m_slots)
                    {
                        if (!slot.IsLocked && slot.InLobby)
                            slot.Controller.Vibrate(m_nonReadyVibration, m_nonReadyVirbrateTime);
                    }
                }
            }
        }
    }

    private XInput.Controller GetFirstLocked()
    {
        foreach(LobbyPlayerSlot slot in m_slots)
        {
            if (slot.IsLocked)
                return ControllerManager.Instance.GetController(slot.PlayerInfo.Index);
        }

        return null;
    }

    private List<PlayerInfo> GetLockedPlayers()
    {
        List<PlayerInfo> players = new List<PlayerInfo>(m_readyCount);

        foreach (LobbyPlayerSlot slot in m_slots)
            if (slot.IsLocked)
                players.Add(slot.PlayerInfo);

        return players;
    }
}
