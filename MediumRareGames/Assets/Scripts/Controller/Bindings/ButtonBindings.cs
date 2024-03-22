/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   ButtonBindings
       - A class that represents the different keys/mousebuttons bound to a
         controller's buttons

   Details:
       - Contains bindings for both keycodes and mouse buttons
-----------------------------------------------------------------------------
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    [Serializable]
    public class ButtonBindings
    {
        #region Classes
        /// <summary>Wrapper class for Binding using Button and KeyCode</summary>
        [Serializable]
        public class KeyBinding : Binding<Button, KeyCode>
        {
            /// <summary>Constructs a KeyBinding</summary>
            /// <param name="_Button">The controller button to be bound</param>
            public KeyBinding(Button _Button) : base(_Button) { }
        }

        /// <summary>Wrapper class for Binding using Button and MouseButton</summary>
        [Serializable]
        public class MouseBinding : Binding<Button, MouseButton>
        {
            /// <summary>Constructs a MouseBinding</summary>
            /// <param name="_Button">The controller button to be bound</param>
            public MouseBinding(Button _Button) : base(_Button) { }
        }
        #endregion

        [SerializeField] private List<KeyBinding> m_keyBindings; //The keybindings for a controller
        [SerializeField] private List<MouseBinding> m_mouseBindings; //The mousebindings for a controller

        /// <summary>Returns the KeyBindings</summary>
        public List<KeyBinding> KeyBindings { get { return m_keyBindings; } }

        /// <summary>Returns the MouseBindings</summary>
        public List<MouseBinding> MouseBindings { get { return m_mouseBindings; } }
    }
}