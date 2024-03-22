/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   ButtonEventArgs
       - EventArgs to represent a controller button event

   Details:
       - Contains what button was pressed and the state of the given button
-----------------------------------------------------------------------------
*/

using System;

namespace Controller
{
    public class ButtonEventArgs : EventArgs
    {
        private Button m_button; //What button was pressed
        private ButtonInputState m_state; //What state is that button in

        /// <summary>Returns the button pressed</summary>
        public Button Button { get { return m_button; } }

        /// <summary>Returns the state of the button pressed</summary>
        public ButtonInputState State { get { return m_state; } }

        /// <summary>Constructs a ButtonEventArgs</summary>
        /// <param name="_Button">The button pressed</param>
        /// <param name="_State">The state of the button pressed</param>
        public ButtonEventArgs(Button _Button, ButtonInputState _State)
        {
            m_button = _Button;
            m_state = _State;
        }
    }
}
