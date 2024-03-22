using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XInput;

public class LobbyPlayerSlot : MonoBehaviour
{
    [SerializeField] private LobbyMenu m_lobbyMenu;
    [SerializeField] private PlayerInfo m_playerInfo;
    [Space]
    [SerializeField] private Text m_displayText;
    [SerializeField] private Image m_lockImage;
    [SerializeField] private Image m_charImage;
    [SerializeField] private Image m_backImage;
    [Space]
    [SerializeField] private float m_charSwitchVibrate = 0.15f;
    [SerializeField] private float m_charSwitchVibrateTime = 0.1f;

    private int m_characterIndex;
    private bool m_inLobby;
    private bool m_isLocked;

    private XInput.Controller m_controller;

    public bool InLobby { get { return m_inLobby; } }
    public bool IsLocked { get { return m_isLocked; } }
    public PlayerInfo PlayerInfo { get { return m_playerInfo; } }
    public XInput.Controller Controller { get { return m_controller; } }

    private void Start()
    {
        m_backImage.color = m_playerInfo.Color;
    }


    #region Public Methods
    public void RandomizeCharacter()
    {
        m_characterIndex = Random.Range(0, m_lobbyMenu.AvailableCharacters.Count);
        UpdateCharacterDisplay();
    }

    // Use this for initialization
    public void Init()
    {
        m_controller = ControllerManager.Instance.GetController(m_playerInfo.Index);

        m_controller.Subscribe(m_lobbyMenu.JoinButton, JoinHandler);
    }

    public void Clean(bool _ForceLeave)
    {
        if (_ForceLeave)
        {
            if (IsLocked)
                Unlock();

            if (InLobby)
                Leave();
        }

        m_controller.UnSubscribe(m_lobbyMenu.JoinButton, JoinHandler);
    }
    #endregion

    #region Private Methods
    private void ClearCharacterDisplay()
    {
        m_playerInfo.Character = null;
        m_charImage.sprite = null;
        m_charImage.enabled = false;
    }

    private void UpdateCharacterDisplay()
    {
        m_playerInfo.Character = m_lobbyMenu.AvailableCharacters[m_characterIndex];
        m_charImage.sprite = m_playerInfo.Character.Sprite;
        m_charImage.enabled = true;
    }

    private int WrapIndex(int _Index, int _ArraySize)
    {
        return (_Index % _ArraySize + _ArraySize) % _ArraySize;
    }

    private void Join()
    {
        m_controller.UnSubscribe(m_lobbyMenu.JoinButton, JoinHandler);

        m_controller.Subscribe(m_lobbyMenu.JoinButton, LockHandler);
        m_controller.Subscribe(m_lobbyMenu.LeaveButton, LeaveHandler);
        m_controller.Subscribe(m_lobbyMenu.ScrollLeftButton, ScrollLeftHandler);
        m_controller.Subscribe(m_lobbyMenu.ScrollRightButton, ScrollRightHandler);

        m_inLobby = true;
        RandomizeCharacter();

        m_lobbyMenu.LobbyCount++;
    }

    private void Leave()
    {
        m_lobbyMenu.LobbyCount--;
        ClearCharacterDisplay();
        m_inLobby = false;

        m_controller.UnSubscribe(m_lobbyMenu.ScrollRightButton, ScrollRightHandler);
        m_controller.UnSubscribe(m_lobbyMenu.ScrollLeftButton, ScrollLeftHandler);
        m_controller.UnSubscribe(m_lobbyMenu.JoinButton, LockHandler);
        m_controller.UnSubscribe(m_lobbyMenu.LeaveButton, LeaveHandler);

        m_controller.Subscribe(m_lobbyMenu.JoinButton, JoinHandler);
    }

    private void Lock()
    {
        m_controller.UnSubscribe(m_lobbyMenu.ScrollRightButton, ScrollRightHandler);
        m_controller.UnSubscribe(m_lobbyMenu.ScrollLeftButton, ScrollLeftHandler);
        m_controller.UnSubscribe(m_lobbyMenu.JoinButton, LockHandler);
        m_controller.UnSubscribe(m_lobbyMenu.LeaveButton, LeaveHandler);

        m_controller.Subscribe(m_lobbyMenu.LeaveButton, UnlockHandler);

        m_isLocked = true;
        m_lockImage.enabled = true;
        m_lobbyMenu.RemoveCharacter(m_playerInfo.Character);

        m_lobbyMenu.ReadyCount++;
    }

    private void Unlock()
    {
        m_isLocked = false;
        m_lockImage.enabled = false;
        m_lobbyMenu.AddCharacter(m_playerInfo.Character);

        m_controller.Subscribe(m_lobbyMenu.ScrollRightButton, ScrollRightHandler);
        m_controller.Subscribe(m_lobbyMenu.ScrollLeftButton, ScrollLeftHandler);
        m_controller.Subscribe(m_lobbyMenu.JoinButton, LockHandler);
        m_controller.Subscribe(m_lobbyMenu.LeaveButton, LeaveHandler);

        m_controller.UnSubscribe(m_lobbyMenu.LeaveButton, UnlockHandler);

        m_lobbyMenu.ReadyCount--;
    }

    private void Scroll(int _Change)
    {
        m_characterIndex = WrapIndex(m_characterIndex + _Change, m_lobbyMenu.AvailableCharacters.Count);
        UpdateCharacterDisplay();
    }

    #region Controller Handlers
    private void LockHandler(XInput.Controller _Controller, ButtonArgs _Args)
    {
        //Return if already locked
        if (_Args.Action != ButtonAction.Down)
            return;

        Lock();
    }

    private void UnlockHandler(XInput.Controller _Controller, ButtonArgs _Args)
    {
        //Return if not locked
        if (_Args.Action != ButtonAction.Down)
            return;

        Unlock();
    }

    private void JoinHandler(XInput.Controller _Controller, ButtonArgs _Args)
    {
        if (_Args.Action != ButtonAction.Down)
            return;

        Join();
    }

    private void LeaveHandler(XInput.Controller _Controller, ButtonArgs _Args)
    {
        //Return if not in lobby
        if (_Args.Action != ButtonAction.Down)
            return;

        Leave();
    }

    private void ScrollLeftHandler(XInput.Controller _Controller, ButtonArgs _Args)
    {
        if (_Args.Action != ButtonAction.Down)
            return;

        m_controller.Vibrate(new Vector2(m_charSwitchVibrate, 0), m_charSwitchVibrateTime);
        Scroll(-1);
    }

    private void ScrollRightHandler(XInput.Controller _Controller, ButtonArgs _Args)
    {
        if (_Args.Action != ButtonAction.Down)
            return;

        m_controller.Vibrate(new Vector2(0, m_charSwitchVibrate), m_charSwitchVibrateTime);
        Scroll(1);
    }
    #endregion

    #endregion
}
