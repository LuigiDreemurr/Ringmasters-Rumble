/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   ModeSelection
       - A simple script that allows a gamemode to be selected and displayed

   Details:
       - Sets a static variable to the chosen gamemode
       - Changes gamemode based on the controller's X button

       - TEMP SCRIPT. The only thing needed from the script is the static Gamemode
         type variable. Should be redone later on
-----------------------------------------------------------------------------
*/

using UnityEngine;
using UnityEngine.UI;
using Controller;

public class ModeSelection : MonoBehaviour
{
    static private Gamemode.Type m_type = Gamemode.Type.LMS;
    static public Gamemode.Type Type { get { return m_type; } }

    [SerializeField] private Text m_displayText;

	/// <summary>Initialization</summary>
	void Start ()
    {
        //Set initial game mode text
        m_displayText.text = m_type.ToString();

        //Hook up controller with ability to change gamemode
        GetComponent<Controller.Controller>().Subscribe(Controller.Button.X, SwitchGamemode);
	}

    /// <summary>Switch the gamemode type on button press</summary>
    /// <param name="_Sender">Where the button event came from</param>
    /// <param name="_Args">The button event information</param>
    private void SwitchGamemode(object _Sender, ButtonEventArgs _Args)
    {
        if(_Args.State == ButtonInputState.Down)
        {
            m_type = (Gamemode.Type)(((int)m_type + 1) % 2);
            m_displayText.text = m_type.ToString();
        }
    }
}
