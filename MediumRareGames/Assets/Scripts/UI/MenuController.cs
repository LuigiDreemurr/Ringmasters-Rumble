/*
 -----------------------------------------------------------------------------
        Created By Brandon Vout
 -----------------------------------------------------------------------------
    MenuController
        - Basic menu control

    Details:
        - Show/hide menus
        - Change scenes
        - Supports multiple controllers via editing EventSystems
        - Pause game
        - Tracks which player paused the game & only lets them use the menu
        - Close application
        - Change text on dedicated Message Boxes
        - Stores previous and forward menus
 -----------------------------------------------------------------------------
 */

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Controller;

[RequireComponent(typeof(EventSystem))]
public class MenuController : MonoBehaviour
{

    #region Classes



    #endregion  // Classes

    //---------------------------------------------------------------------------
    #region Variables

    #region Constants

    const int NO_JOYSTICK = -1;

    #endregion  // Constants

    #region Public



    #endregion  // Public

    #region Private

    [SerializeField] [Lockable] private EventSystem eventSystem;

    [Tooltip("Disable to have to set manually with Btn function (does not auto-store on Hide Menu functions due to problems).")]
    [SerializeField]
    [Lockable]
    private bool autoStorePreviousMenus = false;    // Disable to have to set manually with Btn function (does not auto-store on Hide Menu functions due to problems)
    [Tooltip("Disable to have to set manually with Btn function.")]
    [SerializeField]
    [Lockable]
    private bool autoStoreForwardMenus = false;     // Disable to have to set manually with Btn function

    [Tooltip("Set to false for menu scenes (title screen, codex, etc.).")]
    [SerializeField]
    [Lockable]
    private bool isGameplayScene = true;    // Is this a gameplay scene? If not it is a menu scene (e.g. main menu)
    [Tooltip("Disable for Dark Souls-style menu, game does not pause.")]
    [SerializeField]
    [Lockable]
    private bool pauseOnMenuOpen = true;    // Set to false for Dark Souls style menu, no stopping time

    [Tooltip("Menu parent objects (empties, panels, etc.)")]
    [SerializeField]
    private GameObject[] menus = new GameObject[3];    // Menus to navigate (use empty/panel parent objects to hold each menu)

    [SerializeField] private GameObject[] players = new GameObject[1];  // Player game objects
    private PlayerController[] playerControllers;                       // Player controller array
    private Controller.Controller[] controllers;                        // Controller array

    private bool disableMenu = false;   // Disable the player's ability to open the pause menu, only used in gameplay scenes
    private bool menuOpen;              // Tell if "paused", only used in gameplay scenes
    private string messageBoxText;      // Text to be printed on messageBox

    [Tooltip("Object focused on Start (menu scene) or on Pause (gameplay scene).")]
    [SerializeField]
    private GameObject defaultFocus;   // GameObject to focus on when opening pause menu or staring menu scene
    [Tooltip("Index of menu that opens on Start (menu scene) or Pause (gameplay scene).")]
    [SerializeField]
    private int defaultMenuIndex = 0;  // Index of menu that opens on pause
    private GameObject defaultMenu;                     // The menu shown on pause (set to equal menu[0])
    Stack<bool[]> previousMenus = new Stack<bool[]>();  // Stores previous menus visited
    Stack<bool[]> forwardMenus = new Stack<bool[]>();   // Stores forward menus after opening previous menus

    private int currentJoystick;

    #endregion  // Private

    #endregion  // Variables

    //---------------------------------------------------------------------------
    #region MonoBehaviour

    /// <summary> Use this for initialization </summary>
    private void Awake()
    {
        menuOpen = false;
        messageBoxText = "";
        if (defaultMenuIndex < 0)
            defaultMenuIndex = 0;                   // Do not have negative index
        else if (defaultMenuIndex >= menus.Length && menus.Length > 0)
            defaultMenuIndex = menus.Length - 1;    // Do not go outside array length
    }

    /// <summary> Use this for initialization, after Awake </summary>
    void Start()
    {
        if (menus.Length > defaultMenuIndex)
            defaultMenu = menus[defaultMenuIndex];

        if (players.Length > 0)
            InitializeControllers();

        if (isGameplayScene)
            Btn_ResumeGame();
        else    // Is a Menu Scene
            Btn_ShowOnlyDefaultMenu(true);

        Btn_ClearForwardMenus();
        Btn_ClearPreviousMenus();

        SetCurrentPlayerIndex(1);   // Player 1 controls menu scenes

        // If menu scene, focus on initial control
        if (!isGameplayScene)
        {
            if (defaultFocus != null)
                Btn_SelectObject(defaultFocus);
        }
        else    // If gameplay scene, set current joystick to non-exiastant
        {
            currentJoystick = NO_JOYSTICK;
        }
    }

    #endregion  // MonoBehaviour

    //---------------------------------------------------------------------------
    #region Functions

    #region Public

    public bool GetMenuOpen() { return menuOpen; }
    public void EnableMenu() { disableMenu = false; }   // Allow players to pause gameplay / open menu
    public void DisableMenu() { disableMenu = true; }   // Do now allow players to pause gameplay / open menu

    public void SetPlayers(GameObject[] _Players)
    {
        players = _Players;
        InitializeControllers();
    }

    public void Pause(bool _Pause)
    {
        Time.timeScale = _Pause ? 0 : 1;
        SetControllersActive(!_Pause);
    }

    #region Buttons

    /// <summary> Set new selected GameObject, need to use this for controller support </summary>
    /// <param name="_Object"></param>
    public void Btn_SelectObject(GameObject _Object)
    {
        if (_Object != null)
            EventSystem.current.SetSelectedGameObject(_Object, null);
    }

    /// <summary>  Show Menu Parent </summary>
    /// <param name="_Menu"> Menu empty parent object </param>
    public void Btn_ShowMenu(GameObject _Menu)
    {
        if (_Menu != null)
        {
            if (autoStorePreviousMenus)
                Btn_StorePreviousMenus();
            _Menu.SetActive(true);
        }
    }

    /// <summary> Hide All Menus usinf Linked List </summary>
    public void Btn_HideAllMenus()
    {
        if (menus != null)
        {
            //bool b = autoStorePreviousMenus;

            //if (b)
            //{
            //    Btn_StorePreviousMenus();
            //    autoStorePreviousMenus = false;
            //}
            for (int i = 0; i < menus.Length; i++)
                Btn_HideMenu(menus[i]);
            //if (b)
            //    autoStorePreviousMenus = true;
        }
    }

    /// <summary> Hide Menu Parent </summary>
    /// <param name="_Menu"> Menu empty parent object </param>
    public void Btn_HideMenu(GameObject _Menu)
    {
        if (_Menu != null)
        {
            //if (autoStorePreviousMenus)
            //    Btn_StorePreviousMenus();
            _Menu.SetActive(false);
        }
    }

    /// <summary> Enable/disable button if disabled/enabled, does not auto-store previous menus </summary>
    /// <param name="_Menu"> Menu empty parent object </param>
    public void Btn_ToggleMenu(GameObject _Menu)
    {
        if (_Menu != null)
        {
            if (autoStorePreviousMenus && !_Menu.activeSelf)
                Btn_StorePreviousMenus();
            _Menu.SetActive((_Menu.activeSelf) ? false : true);
        }
    }

    /// <summary> Hide all menus, show selected menu </summary>
    /// <param name="_Menu"> Menu empty parent object </param>
    public void Btn_ChangeMenu(GameObject _Menu)
    {
        //bool b = autoStorePreviousMenus;

        //if (b)
        //{
        //    Btn_StorePreviousMenus();
        //    autoStorePreviousMenus = false;
        //}
        Btn_HideAllMenus();
        Btn_ShowMenu(_Menu);
        //if (b)
        //    autoStorePreviousMenus = true;
    }

    /// <summary> Hide all menus, show default menu </summary>
    public void Btn_ShowOnlyDefaultMenu(bool _ClearPrevious)
    {
        bool b = autoStorePreviousMenus;

        if (_ClearPrevious)
        {
            Btn_ClearPreviousMenus();
            if (b)
                autoStorePreviousMenus = false;
        }
        Btn_ChangeMenu(defaultMenu);
        if (b)
            autoStorePreviousMenus = true;
    }

    /// <summary> Open new scene using name of new scene as a string input </summary>
    /// <param name="_SceneName"> Scene name as string </param>
    public void Btn_OpenScene(string _SceneName)
    {
        SceneManager.LoadScene(_SceneName);
    }

    /// <summary> Close Application </summary>
    public void Btn_ExitApplication()
    {
        Application.Quit();
    }

    /// <summary> Freeze time, show cursor, and show default menu </summary>
    public void Btn_PauseGame()
    {
        if (pauseOnMenuOpen)
        {
            Time.timeScale = 0;
            SetControllersActive(false);
        }
        menuOpen = true;
        //Cursor.visible = true;
        Btn_ShowOnlyDefaultMenu(true);
        Btn_ClearForwardMenus();
        Btn_SelectObject(defaultFocus);
    }

    /// <summary> Unfreeze time, hide cursor, and hide all menus </summary>
    public void Btn_ResumeGame()
    {
        if (pauseOnMenuOpen)
        {
            Time.timeScale = 1;
            SetControllersActive(true);
        }
        menuOpen = false;
        //Cursor.visible = false;
        Btn_HideAllMenus();
        Btn_ClearPreviousMenus();
        Btn_ClearForwardMenus();
    }

    public void Btn_EnableButton(UnityEngine.UI.Button _Button)
    {
        SetButtonActive(true, _Button);
    }

    public void Btn_DisableButton(UnityEngine.UI.Button _Button)
    {
        SetButtonActive(false, _Button);
    }

    #endregion  // Buttons

    #region PreviousAndForward

    /// <summary> Add previous menu to top of stack, call before showing or hiding menus </summary>
    public void Btn_StorePreviousMenus()
    {
        bool[] b = GetActiveMenus();

        if (previousMenus.Count > 0)
        {
            if (b != previousMenus.Peek())
                previousMenus.Push(b);
        }
        else
            previousMenus.Push(b);

        if (autoStoreForwardMenus)
            Btn_ClearForwardMenus();
    }

    /// <summary> Add forward menu to top of stack, call before showing or hiding menus </summary>
    public void Btn_StoreForwardMenus()
    {
        forwardMenus.Push(previousMenus.Peek());
    }

    /// <summary> Clear previous menus stack </summary>
    public void Btn_ClearPreviousMenus()
    {
        previousMenus.Clear();
    }

    /// <summary> Clear forward menus stack </summary>
    public void Btn_ClearForwardMenus()
    {
        forwardMenus.Clear();
    }

    /// <summary> Opens previous menus, removes top of stack </summary>
    public void Btn_OpenPreviousMenus()
    {
        if (previousMenus.Count > 0)
        {
            bool[] activeMenus = previousMenus.Peek();

            if (autoStoreForwardMenus)
                Btn_StoreForwardMenus();
            for (int i = 0; i < menus.Length; i++)
                menus[i].SetActive(activeMenus[i]);

            Btn_PopPreviousMenus();
        }
    }

    /// <summary> Opens forward menus, removes top of stack </summary>
    public void Btn_OpenForwardMenus()
    {
        if (forwardMenus.Count > 0)
        {
            bool[] activeMenus = forwardMenus.Peek();

            for (int i = 0; i < menus.Length; i++)
                menus[i].SetActive(activeMenus[i]);

            Btn_PopForwardMenus();
        }
    }

    /// <summary> Remove previous menus from top of stack, Call this if you use HideMenu functions to close menus to avoid filling the back button with useless menus </summary>
    public void Btn_PopPreviousMenus()
    {
        if (previousMenus.Count > 0)
            previousMenus.Pop();
    }

    /// <summary> Remove forward menus from top of stack </summary>
    public void Btn_PopForwardMenus()
    {
        if (forwardMenus.Count > 0)
            forwardMenus.Pop();
    }

    #endregion  // PreviousAndForward

    #region MessageBoxes

    /// <summary> Open message box without auto storing in previous menu stack </summary>
    public void Btn_Msg_ShowMessageBox(GameObject _Menu)
    {
        bool b = autoStorePreviousMenus;

        if (b)
        {
            Btn_StorePreviousMenus();
            autoStorePreviousMenus = false;
        }

        Btn_ShowMenu(_Menu);

        if (b)
            autoStorePreviousMenus = true;
    }

    /// <summary> Open messagebox without auto storing message box in previous menu stack, hides other menus (stores if auto-store on) </summary>
    public void Btn_Msg_ShowOnlyMessageBox(GameObject _Menu)
    {
        bool b = autoStorePreviousMenus;

        if (b)
        {
            Btn_StorePreviousMenus();
            autoStorePreviousMenus = false;
        }

        Btn_ChangeMenu(_Menu);

        if (b)
            autoStorePreviousMenus = true;
    }

    /// <summary> write string value to message </summary>
    /// <param name="_MessageText"> Message box text object </param>
    public void Btn_Msg_SetMessageText(Text _MessageText)
    {
        _MessageText.text = messageBoxText;
    }

    /// <summary> Set messageBoxText string </summary>
    /// <param name="_Message"></param>
    public void Btn_Msg_SetMessageString(string _Message)
    {
        messageBoxText = _Message;
    }

    /// <summary>
    /// For message box templates with multiple button/control sets (to let you switch between YesNo message boxes and Close message boxes)
    /// </summary>
    /// <param name="_Buttons"> Buttons holder (empty, panel, etc.) to enable </param>
    public void Btn_Msg_ShowButtons(GameObject _Buttons)
    {
        Btn_ShowMenu(_Buttons);
    }

    /// <summary>
    /// For message box templates with multiple button/control sets (to let you switch between YesNo message boxes and Close message boxes)
    /// </summary>
    /// <param name="_Buttons"> Buttons holder (empty, panel, etc.) to disable </param>
    public void Btn_Msg_HideButtons(GameObject _Buttons)
    {
        Btn_HideMenu(_Buttons);
    }

    #endregion  // MessageBoxes

    #endregion  // Public

    #region Private

    /// <summary> Add controller scripts from players game objects(s) to controller/player controller arrays.
    /// Subscribe Pause and Cancel events to Controllers. </summary>
    private void InitializeControllers()
    {
        //if (isGameplayScene)
        //playerControllers = new PlayerController[players.Length];
        controllers = new Controller.Controller[players.Length];

        for (int i = 0; i < players.Length; i++)
        {
            if (isGameplayScene)
                if (players[i].gameObject.GetComponent<PlayerController>() != null)
                    playerControllers[i] = players[i].gameObject.GetComponent<PlayerController>();
            if (players[i].gameObject.GetComponent<Controller.Controller>() != null)
            {
                controllers[i] = players[i].gameObject.GetComponent<Controller.Controller>();
                controllers[i].Subscribe(Controller.Button.Start, Pause);
                controllers[i].Subscribe(Controller.Button.B, Cancel);
            }
        }
    }

    /// <summary> Enable or disable all player controllers </summary>
    /// <param name="_Enabled"></param>
    private void SetControllersActive(bool _Enabled)
    {
        if (playerControllers != null)
            for (int i = 0; i < playerControllers.Length; i++)
                playerControllers[i].enabled = _Enabled;
    }

    /// <summary> Sets active joystick index and changes EventSystem input module values to allow them control. </summary>
    /// <param name="_Joystick"> Controller Joystick Number </param>
    private void SetCurrentPlayerIndex(int _Joystick)
    {
        currentJoystick = _Joystick;
        eventSystem.GetComponent<StandaloneInputModule>().horizontalAxis = "J" + _Joystick + " Axis LeftStick_X";
        eventSystem.GetComponent<StandaloneInputModule>().verticalAxis = "J" + _Joystick + " Axis LeftStick_Y";
        eventSystem.GetComponent<StandaloneInputModule>().submitButton = "J" + _Joystick + " Button A";
        eventSystem.GetComponent<StandaloneInputModule>().cancelButton = "J" + _Joystick + " Button B";
    }

    /// <summary> Check and store all menu enabled bools </summary>
    /// <returns></returns>
    private bool[] GetActiveMenus()
    {
        bool[] activeMenus = new bool[menus.Length];

        for (int i = 0; i < menus.Length; i++)
            activeMenus[i] = menus[i].activeSelf;

        return activeMenus;
    }

    /// <summary> Enable or disable button </summary>
    /// <param name="_Enabled"> Enable or Disable button </param>
    private void SetButtonActive(bool _Enabled, UnityEngine.UI.Button _Button)
    {
        _Button.enabled = _Enabled;
    }

    #endregion  // Private

    #endregion  // Functions

    //---------------------------------------------------------------------------
    #region EventsAndHandlers

    #region EventHandlers



    #endregion  // Event Handlers

    #region Events

    /// <summary> Pause button pressed, if in gameplay scene open or close pause menu </summary>
    /// <param name="_Sender"></param>
    /// <param name="_Args"></param>
    private void Pause(object _Sender, ButtonEventArgs _Args)
    {
        Controller.Controller ctrl = (Controller.Controller)_Sender;

        // Handle pausing and unpausing in gameplay scene
        if (isGameplayScene && !disableMenu && _Args.State == ButtonInputState.Up && (currentJoystick == NO_JOYSTICK || currentJoystick == ctrl.Joystick))
        {
            if (menuOpen)
            {
                currentJoystick = NO_JOYSTICK;
                Btn_ResumeGame();
            }
            else
            {
                SetCurrentPlayerIndex(ctrl.Joystick);
                Btn_PauseGame();
            }
        }
    }

    /// <summary> If pause menu is open go back to previous menu or default menu, or resume play </summary>
    /// <param name="_Sender"></param>
    /// <param name="_Args"></param>
    private void Cancel(object _Sender, ButtonEventArgs _Args)
    {
        Controller.Controller ctrl = (Controller.Controller)_Sender;

        if (isGameplayScene && !disableMenu && menuOpen && _Args.State == ButtonInputState.Up && currentJoystick == ctrl.Joystick)
        {
            if (defaultMenu.activeSelf) // If in default menu, resume game
            {
                currentJoystick = NO_JOYSTICK;
                Btn_ResumeGame();
            }
            else                        // Else, return to previous/default menu
            {
                if (previousMenus.Count > 0)
                    Btn_OpenPreviousMenus();
                else
                    Btn_ShowOnlyDefaultMenu(true);
            }
        }
    }

    #endregion  // Events

    #endregion  // Events & Handlers
}