/*
 -----------------------------------------------------------------------------
        Created By Wesley Ducharme
 -----------------------------------------------------------------------------
    Controller
        - A wrapper for XInputDotNet

    Details:
        - Controller referenced by its index
        - Can fire off events for buttons, axes, and dual axes
        - Can check input information on any button, axis or dual axis
 -----------------------------------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

namespace XInput
{
    public class ControllerManager : MonoBehaviour
    {
        #region Data Members

        #region Singleton
        static private ControllerManager s_instance;
        static public ControllerManager Instance { get { return s_instance; } }
        #endregion

        #region Input setup variables
        [SerializeField] [HideInInspector] private List<Button> m_usedButtons = new List<Button>(14);
        [SerializeField] [HideInInspector] private List<Button> m_unusedButtons = new List<Button>(14)
        {
            Button.A, Button.B, Button.Y, Button.X,
            Button.DPad_Down, Button.DPad_Right, Button.DPad_Up, Button.DPad_Left,
            Button.LeftBumper, Button.RightBumper,
            Button.LeftStick, Button.RightStick,
            Button.Back, Button.Start
        };

        [SerializeField] [HideInInspector] private List<Axis> m_usedAxes = new List<Axis>(6);
        [SerializeField] [HideInInspector] private List<Axis> m_unusedAxes = new List<Axis>(6)
        {
            Axis.LeftStick_X, Axis.LeftStick_Y,
            Axis.RightStick_X, Axis.RightStick_Y,
            Axis.LeftTrigger, Axis.RightTrigger
        };

        [SerializeField] [HideInInspector] private List<DualAxis> m_usedDualAxes = new List<DualAxis>(2);
        [SerializeField] [HideInInspector] private List<DualAxis> m_unusedDualAxes = new List<DualAxis>(2)
        {
            DualAxis.LeftStick, DualAxis.RightStick
        };

        public List<Button> AvailableButtons { get { return m_usedButtons; } }
        public List<Axis> AvailabledAxes { get { return m_usedAxes; } }
        public List<DualAxis> AvailableDualAxes { get { return m_usedDualAxes; } }
        #endregion

        #region Events
        public delegate void ConnectionEvent(Controller _Controller);

        //Invoked when a controller connects
        public event ConnectionEvent OnConnect;

        //Invoked when a controller disconnects
        public event ConnectionEvent OnDisconnect;
        #endregion
    
        //Will controllers send repeated axis events
        [SerializeField] private bool m_axesRepeat = true;

        //The controllers' dead zone
        [SerializeField] private float m_deadZone = 0.3f;

        [SerializeField] private Controller[] m_controllers = new Controller[4];

        public bool AxesRepeat { get { return m_axesRepeat; } }
        public float DeadZone { get { return m_deadZone; } }

        #endregion

        #region Unity Messages
        /// <summary>Initialization</summary>
        private void Awake()
        {
            #region Singleton
            if (s_instance == null)
                s_instance = this;
            else
            {
                Debug.LogError("Two ControllerManagers exist (Destroying): " + gameObject.name);
                Destroy(gameObject);
            }
            #endregion

            for(int i=0; i<4; i++)
            {
                if(m_controllers[i] == null)
                {
                    Debug.LogError("ControllerManager has no reference to controller " + ((PlayerIndex)i).ToString());
                    continue;
                }

                if((int)m_controllers[i].Index != i)
                {
                    Debug.LogError("ControllerManager index reference does not match: " + i.ToString() + " - " + m_controllers[i].Index.ToString());
                    continue;
                }

                m_controllers[i].Init(m_deadZone, m_axesRepeat, AvailableButtons, AvailabledAxes, AvailableDualAxes);
            }
        }
        #endregion

        #region Public Methods
        public void SendControllerEvents(bool _Sending)
        {
            foreach (Controller controller in m_controllers)
                controller.SendInputEvents = _Sending;
        }

        /// <summary>Find the first controller connected</summary>
        /// <returns>The first connected controller it finds</returns>
        public Controller GetFirstConnected()
        {
            foreach (Controller controller in m_controllers)
                if (controller.IsConnected)
                    return controller;

            return null;
        }

        /// <summary>Get the controller for the index</summary>
        /// <param name="_Index">The index of the controller wanted</param>
        /// <returns>The controller with _Index</returns>
        public Controller GetController(PlayerIndex _Index)
        {
            return m_controllers[(int)_Index];
        }

        /// <summary>Invokes the OnConnect event</summary>
        /// <param name="_Controller">The controller that was connected</param>
        public void ControllerConnected(Controller _Controller)
        {
            OnConnect?.Invoke(_Controller);
        }

        /// <summary>Invokes the OnDisconnect event</summary>
        /// <param name="_Controller">The controller that was disconnected</param>
        public void ControllerDisconnected(Controller _Controller)
        {
            OnDisconnect?.Invoke(_Controller);
        }
        #endregion
    }

    #region Enums
    /// <summary>All available controller buttons</summary>
    public enum Button
    {
        A, B, Y, X,
        DPad_Down, DPad_Right, DPad_Up, DPad_Left,
        LeftBumper, RightBumper,
        LeftStick, RightStick,
        Back, Start
    }

    /// <summary>Controller button actions</summary>
    public enum ButtonAction
    {
        Down, Up, Held, Released
    }

    /// <summary>All available controller axes</summary>
    public enum Axis
    {
        LeftStick_X, LeftStick_Y,
        RightStick_X, RightStick_Y,
        LeftTrigger, RightTrigger
    }

    /// <summary>All available controller dual axes</summary>
    public enum DualAxis
    {
        LeftStick, RightStick
    }
    #endregion
}
