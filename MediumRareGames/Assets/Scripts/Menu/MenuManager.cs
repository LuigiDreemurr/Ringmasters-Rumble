/*
 -----------------------------------------------------------------------------
        Created By Wesley Ducharme
 -----------------------------------------------------------------------------
    MenuManager
        - The script handling the hiding and showing of menus

    Details:
        - Singleton
 -----------------------------------------------------------------------------
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour 
{
    #region Singleton
    //Instance of MenuManager
    static private MenuManager s_instance;

    /// <summary>Get the only instance of MenuManager</summary>
    static public MenuManager Instance { get { return s_instance; } }

    /// <summary>Singleton initialization</summary>
    protected void Awake()
    {
        if (s_instance == null)
            s_instance = this;
        else
        {
            Debug.LogError("Two MenuManager exist (Destroying): " + gameObject.name);
            Destroy(gameObject);
        }
    }
    #endregion

    #region Data Members
    //The menu that will be initially shown
    [SerializeField] private Menu m_initialMenu;

    //Does having a menu open set the timeScale
    [SerializeField] private bool m_handlePause = true;
    
    //The stack of all menus
    private Stack<Menu> m_menus;

    //Function that returns the controller who controls the menus
    private Func<XInput.Controller> m_getMenuController;
    private XInput.Controller m_menuController;

    //Called when the first menu is shown (only one menu)
    public event Action OnFirstMenuShow;

    //Called when the last menu is hidden (no more menus)
    public event Action OnLastMenuHide;

    public Func<XInput.Controller> GetMenuController
    {
        get { return m_getMenuController; }
        set
        {
            m_getMenuController = value;
        }
    }

    public XInput.Controller MenuController
    {
        get { return m_menuController; }
        set
        {
            m_menuController = value;

            if(m_menuController != null)
                XInput.ControllerMouseInputModule.Instance.Controller = m_menuController;
        }
    }
    #endregion

    #region Unity Messages
    /// <summary>Initialization</summary>
    private void Start () 
    {
        m_getMenuController = XInput.ControllerManager.Instance.GetFirstConnected;

        //Init menu stack
        m_menus = new Stack<Menu>();

        //Show initial menu
        if(m_initialMenu != null)
            ShowMenu(m_initialMenu);
	}

    private void Update()
    {
        if(m_menuController == null)
            XInput.ControllerMouseInputModule.Instance.Controller = GetMenuController();
    }
    #endregion

    #region Public Methods
    /// <summary>Hide the current menu, showing the next menu (_Menu)</summary>
    /// <param name="_Menu">The menu to be shown</param>
    public void ShowMenu(Menu _Menu)
    {
        //Disable previous
        if(m_menus.Count > 0)
        {
			Menu prev = m_menus.Peek();
			prev.gameObject.SetActive(false);
			prev.OnHide();         
        }
        else
        {
            if(m_handlePause)
                SetTimeScale(true);

            OnFirstMenuShow?.Invoke();
        }

        //Enable next
        _Menu.gameObject.SetActive(true);
        _Menu.OnShow();
        m_menus.Push(_Menu);
        UpdateEventSelection();
    }

    /// <summary>Hide the current menu, showing the previous</summary>
    public void HideMenu()
    {
        //Disable previous
        Menu prev = m_menus.Pop();
        prev.gameObject.SetActive(false);
        prev.OnHide();

        //Enable next
        if(m_menus.Count > 0)
        {
			Menu next = m_menus.Peek();
			next.gameObject.SetActive(true);
			next.OnShow();

            UpdateEventSelection();
        }
        else
        {
            if (m_handlePause)
                SetTimeScale(false);

            OnLastMenuHide?.Invoke();
        }
    }
    #endregion

    #region Private Methods
    private void SetTimeScale(bool _Pause)
    {
        Time.timeScale = _Pause ? 0f : 1f;
    }

    /// <summary>Will update the EventSystem's selected game object based on the current menu</summary>
    private void UpdateEventSelection()
    {
        EventSystem.current.firstSelectedGameObject = m_menus.Peek().InitialSelect;
        EventSystem.current.SetSelectedGameObject(m_menus.Peek().InitialSelect);
    }
    #endregion
}
