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
    public class Controller : MonoBehaviour
    {
        #region Static Members
        private static void InitEventDict<InputType, ArgsType>(out Dictionary<InputType, Event<Controller, ArgsType>> _Dict, List<InputType> _List)
        {
            _Dict = new Dictionary<InputType, Event<Controller, ArgsType>>(_List.Count);
            foreach (InputType inputType in _List)
                _Dict.Add(inputType, new Event<Controller, ArgsType>());
        }

        private static Dictionary<Button, Func<GamePadState, ButtonState>> RetrieveButton = new Dictionary<Button, Func<GamePadState, ButtonState>>()
    {
        { Button.A, (gamePadState) => gamePadState.Buttons.A },
        { Button.B, (gamePadState) => gamePadState.Buttons.B },
        { Button.Y, (gamePadState) => gamePadState.Buttons.Y },
        { Button.X, (gamePadState) => gamePadState.Buttons.X },
        { Button.DPad_Down, (gamePadState) => gamePadState.DPad.Down },
        { Button.DPad_Right, (gamePadState) => gamePadState.DPad.Right },
        { Button.DPad_Up, (gamePadState) => gamePadState.DPad.Up },
        { Button.DPad_Left, (gamePadState) => gamePadState.DPad.Left },
        { Button.LeftBumper, (gamePadState) => gamePadState.Buttons.LeftShoulder },
        { Button.RightBumper, (gamePadState) => gamePadState.Buttons.RightShoulder },
        { Button.LeftStick, (gamePadState) => gamePadState.Buttons.LeftStick },
        { Button.RightStick, (gamePadState) => gamePadState.Buttons.RightStick },
        { Button.Back, (gamePadState) => gamePadState.Buttons.Back },
        { Button.Start, (gamePadState) => gamePadState.Buttons.Start }
    };

        private static Dictionary<Axis, Func<GamePadState, float>> RetrieveAxis = new Dictionary<Axis, Func<GamePadState, float>>()
    {
        { Axis.LeftStick_X, (gamePadState) => gamePadState.ThumbSticks.Left.X },
        { Axis.LeftStick_Y, (gamePadState) => gamePadState.ThumbSticks.Left.Y },
        { Axis.RightStick_X, (gamePadState) => gamePadState.ThumbSticks.Right.X },
        { Axis.RightStick_Y, (gamePadState) => gamePadState.ThumbSticks.Right.Y },
        { Axis.LeftTrigger, (gamePadState) => gamePadState.Triggers.Left},
        { Axis.RightTrigger, (gamePadState) => gamePadState.Triggers.Right }
    };

        private static Dictionary<DualAxis, Func<GamePadState, Vector2>> RetrieveDualAxis = new Dictionary<DualAxis, Func<GamePadState, Vector2>>()
    {
        { DualAxis.LeftStick, (gamePadState) => new Vector2(gamePadState.ThumbSticks.Left.X, gamePadState.ThumbSticks.Left.Y) },
        { DualAxis.RightStick, (gamePadState) => new Vector2(gamePadState.ThumbSticks.Right.X, gamePadState.ThumbSticks.Right.Y) },
    };
        #endregion

        public void Init(float _DeadZone, bool _AxesRepeat, List<Button> _ButtonEvents, List<Axis> _AxisEvents, List<DualAxis> _DualAxisEvents)
        {
            m_deadZone = _DeadZone;
            m_sendRepeated = _AxesRepeat;

            InitEventDict(out m_buttonEvents, _ButtonEvents);
            InitEventDict(out m_axisEvents, _AxisEvents);
            InitEventDict(out m_dualAxisEvents, _DualAxisEvents);
        }

        /// <summary>Update the controller's states and send events</summary>
        private void LateUpdate()
        {
            //Update states
            m_prevState = m_currState;
            m_currState = GamePad.GetState(m_index);

            //Handle connection events
            if (m_prevState.IsConnected && !m_currState.IsConnected)
                ControllerManager.Instance.ControllerDisconnected(this);
            else if (!m_prevState.IsConnected && m_currState.IsConnected)
                ControllerManager.Instance.ControllerConnected(this);

            if (!m_currState.IsConnected || !m_prevState.IsConnected)
                return;

            if (!m_sendInputEvents)
            {
                TestDualAxis(DualAxis.RightStick);
                return;
            }

            //Button events
            foreach (Button button in m_buttonEvents.Keys)
                TestButton(button);

            //Axis events
            foreach (Axis axis in m_axisEvents.Keys)
                TestAxis(axis);

            //Dual axis events
            foreach (DualAxis axis in m_dualAxisEvents.Keys)
                TestDualAxis(axis);
        }


        #region Data Members

        #region Events
        private Dictionary<Button, Event<Controller, ButtonArgs>> m_buttonEvents;
        private Dictionary<Axis, Event<Controller, AxisArgs<float>>> m_axisEvents;
        private Dictionary<DualAxis, Event<Controller, AxisArgs<Vector2>>> m_dualAxisEvents;
        #endregion

        //State of the game pad last frame
        private GamePadState m_prevState;

        //State of the game pad this frame
        private GamePadState m_currState;

        //What controller this is
        [SerializeField] private PlayerIndex m_index;

        //What low axis values are ignored
        private float m_deadZone;

        //Controller vibration
        [SerializeField] [ReadOnly] private Vector2 m_vibration;

        //Are axis/dual axis events sent repeatedly
        private bool m_sendRepeated;

        [SerializeField] private bool m_sendInputEvents = true;

        public GamePadState PrevState { get { return m_prevState; } }
        public GamePadState CurrState { get { return m_currState; } }
        public PlayerIndex Index { get { return m_index; } }
        public float DeadZone { get { return m_deadZone; } }
        public bool SendInputEvents
        {
            get { return m_sendInputEvents; }
            set { m_sendInputEvents = value; }
        }

        /// <summary>Set/Get the controller vibration (x: left. y: right)</summary>
        public Vector2 Vibration
        {
            get { return m_vibration; }
            private set
            {
                m_vibration = value;
                GamePad.SetVibration(m_index, m_vibration.x, m_vibration.y);
            }
        }

        public bool IsConnected { get { return m_currState.IsConnected; } }
        #endregion

        #region Private Methods
        /// <summary>Will try to send a button event</summary>
        /// <param name="_Button">The button being tested</param>
        private void TestButton(Button _Button)
        {
            ButtonArgs args = GetButton(_Button);
            if (args.Action != ButtonAction.Released)
                m_buttonEvents[_Button].Invoke(this, args);
        }

        /// <summary>Will try to send an axis event</summary>
        /// <param name="_Button">The axis being tested</param>
        private void TestAxis(Axis _Axis)
        {
            AxisArgs<float> args = GetAxis(_Axis);

            if (m_sendRepeated || args.Delta != 0)
                m_axisEvents[_Axis].Invoke(this, args);
        }

        /// <summary>Will try to send a dual axis event</summary>
        /// <param name="_Button">The dual axis being tested</param>
        private void TestDualAxis(DualAxis _Axis)
        {
            AxisArgs<Vector2> args = GetDualAxis(_Axis);

            if (m_sendRepeated || args.Delta != Vector2.zero)
                m_dualAxisEvents[_Axis].Invoke(this, args);
        }

        /// <summary>Utilizes the dead zone to set small values to zero</summary>
        /// <param name="_Axis">The value to process</param>
        /// <returns>The processed value</returns>
        private float ProcessDeadZone(float _Axis)
        {
            return _Axis < m_deadZone ? 0f : _Axis;
        }

        /// <summary>Utilizes the dead zone to set small values to zero</summary>
        /// <param name="_Axis">The value to process</param>
        /// <returns>The processed value</returns>
        private Vector2 ProcessDeadZone(Vector2 _Axis)
        {
            return new Vector2(ProcessDeadZone(_Axis.x), ProcessDeadZone(_Axis.y));
        }

        private IEnumerator VibrateRoutine(Vector2 _Change, float _Duration)
        {
            Vibration += _Change;

            yield return new WaitForSecondsRealtime(_Duration);
            //yield return new WaitForSeconds(_Duration);

            Vibration -= _Change;
        }
        #endregion

        #region Public Methods

        public void Vibrate(Vector2 _Change, float _Duration)
        {
            StartCoroutine(VibrateRoutine(_Change, _Duration));
        }

        /// <summary>Stop the controller vibration (set to Vector2.zero)</summary>
        public void StopVibration()
        {
            Vibration = Vector2.zero;
        }

        #region Input Getters
        /// <summary>Get a button's states/action</summary>
        /// <param name="_Button">The button to retrieve</param>
        /// <returns>The button's information</returns>
        public ButtonArgs GetButton(Button _Button)
        {
            ButtonState prev = RetrieveButton[_Button](m_prevState);
            ButtonState curr = RetrieveButton[_Button](m_currState);
            ButtonAction action = ButtonAction.Released;

            if (prev == ButtonState.Pressed && curr == ButtonState.Pressed)
                action = ButtonAction.Held;
            else if (prev == ButtonState.Released && curr == ButtonState.Pressed)
                action = ButtonAction.Down;
            else if (prev == ButtonState.Pressed && curr == ButtonState.Released)
                action = ButtonAction.Up;

            return new ButtonArgs(prev, curr, action);
        }

        /// <summary>Get an axis' states/delta</summary>
        /// <param name="_Axis">The axis to retrieve</param>
        /// <returns>The axis' information</returns>
        public AxisArgs<float> GetAxis(Axis _Axis)
        {
            float prev = RetrieveAxis[_Axis](m_prevState);
            float curr = RetrieveAxis[_Axis](m_currState);
            float delta = curr - prev;

            float rawPrev = ProcessDeadZone(prev);
            float rawCurr = ProcessDeadZone(curr);
            float rawDelta = rawCurr - rawPrev;

            return new AxisArgs<float>(rawPrev, rawCurr, rawDelta, prev, curr, delta);
        }

        /// <summary>Get a dual axis' states/delta</summary>
        /// <param name="_Axis">The dual axis to retrieve</param>
        /// <returns>The dual axis' information</returns>
        public AxisArgs<Vector2> GetDualAxis(DualAxis _Axis)
        {
            Vector2 prev = RetrieveDualAxis[_Axis](m_prevState);
            Vector2 curr = RetrieveDualAxis[_Axis](m_currState);
            Vector2 delta = curr - prev;

            Vector2 rawPrev = ProcessDeadZone(prev);
            Vector2 rawCurr = ProcessDeadZone(curr);
            Vector2 rawDelta = rawCurr - rawPrev;

            return new AxisArgs<Vector2>(rawPrev, rawCurr, rawDelta, prev, curr, delta);
        }
        #endregion

        #region Subscribing
        /// <summary>Subscibe to a button event</summary>
        /// <param name="_Button">The button to subscibe to</param>
        /// <param name="_Subscriber">The handler to subscibe</param>
        public void Subscribe(Button _Button, Event<Controller, ButtonArgs>.Handler _Subscriber)
        {
            if (!m_buttonEvents.ContainsKey(_Button))
                throw new Exception("Controller " + m_index.ToString() + " has no button event for " + _Button.ToString());

            m_buttonEvents[_Button].Subscribe(_Subscriber);
        }

        /// <summary>Subscibe to an axis event</summary>
        /// <param name="_Axis">The axis to subscibe to</param>
        /// <param name="_Subscriber">The handler to subscibe</param>
        public void Subscribe(Axis _Axis, Event<Controller, AxisArgs<float>>.Handler _Subscriber)
        {
            if (!m_axisEvents.ContainsKey(_Axis))
                throw new Exception("Controller " + m_index.ToString() + " has no axis event for " + _Axis.ToString());

            m_axisEvents[_Axis].Subscribe(_Subscriber);
        }

        /// <summary>Subscibe to a dual axis event</summary>
        /// <param name="_DualAxis">The dual axis to subscibe to</param>
        /// <param name="_Subscriber">The handler to subscibe</param>
        public void Subscribe(DualAxis _DualAxis, Event<Controller, AxisArgs<Vector2>>.Handler _Subscriber)
        {
            if (!m_dualAxisEvents.ContainsKey(_DualAxis))
                throw new Exception("Controller " + m_index.ToString() + " has no dual axis event for " + _DualAxis.ToString());

            m_dualAxisEvents[_DualAxis].Subscribe(_Subscriber);
        }
        #endregion

        #region Unsubscribing
        /// <summary>Unsubscribe from a button event</summary>
        /// <param name="_Button">The button to unsubscribe from</param>
        /// <param name="_Subscriber">The handler to unsubscribe</param>
        public void UnSubscribe(Button _Button, Event<Controller, ButtonArgs>.Handler _Subscriber)
        {
            if (!m_buttonEvents.ContainsKey(_Button))
                throw new Exception("Controller " + m_index.ToString() + " has no button event for " + _Button.ToString());

            m_buttonEvents[_Button].UnSubscribe(_Subscriber);
        }

        /// <summary>Unsubscribe from an axis event</summary>
        /// <param name="_Axis">The axis to unsubscribe from</param>
        /// <param name="_Subscriber">The handler to unsubscribe</param>
        public void UnSubscribe(Axis _Axis, Event<Controller, AxisArgs<float>>.Handler _Subscriber)
        {
            if (!m_axisEvents.ContainsKey(_Axis))
                throw new Exception("Controller " + m_index.ToString() + " has no axis event for " + _Axis.ToString());

            m_axisEvents[_Axis].UnSubscribe(_Subscriber);
        }

        /// <summary>Unsubscribe from a dual axis event</summary>
        /// <param name="_DualAxis">The axis to unsubscribe from</param>
        /// <param name="_Subscriber">The handler to unsubscribe</param>
        public void UnSubscribe(DualAxis _DualAxis, Event<Controller, AxisArgs<Vector2>>.Handler _Subscriber)
        {
            if (!m_dualAxisEvents.ContainsKey(_DualAxis))
                throw new Exception("Controller " + m_index.ToString() + " has no dual axis event for " + _DualAxis.ToString());

            m_dualAxisEvents[_DualAxis].UnSubscribe(_Subscriber);
        }
        #endregion

        #endregion
    }

    #region Controller Input Event Args
    /// <summary>Class representing the information passed through a button event</summary>
    public class ButtonArgs
    {
        #region Data Members
        private ButtonState m_prevState;
        private ButtonState m_currState;
        private ButtonAction m_action;

        /// <summary>The previous button state</summary>
        public ButtonState PrevState { get { return m_prevState; } }

        /// <summary>The current button state</summary>
        public ButtonState CurrState { get { return m_currState; } }

        /// <summary>The button action (relationship between the previous and current state)</summary>
        public ButtonAction Action { get { return m_action; } }
        #endregion

        public ButtonArgs(ButtonState _Prev, ButtonState _Curr, ButtonAction _Action)
        {
            m_prevState = _Prev;
            m_currState = _Curr;
            m_action = _Action;
        }
    }

    /// <summary>Class representing the information passed through an axis event</summary>
    public class AxisArgs<T>
    {
        #region Data Members
        private T m_rawPrevState;
        private T m_rawCurrState;
        private T m_rawDelta;

        private T m_prevState;
        private T m_currState;
        private T m_delta;

        /// <summary>The raw previous state</summary>
        public T RawPrevState { get { return m_rawPrevState; } }

        /// <summary>The raw current state</summary>
        public T RawCurrState { get { return m_rawCurrState; } }

        /// <summary>The difference between the raw previous and raw current state</summary>
        public T RawDelta { get { return m_rawDelta; } }

        /// <summary>The previous state</summary>
        public T PrevState { get { return m_prevState; } }

        /// <summary>The current state</summary>
        public T CurrState { get { return m_currState; } }

        /// <summary>The difference between the previous and current state</summary>
        public T Delta { get { return m_delta; } }
        #endregion

        public AxisArgs(T _RawPrev, T _RawCurr, T _RawDelta, T _Prev, T _Curr, T _Delta)
        {
            m_rawPrevState = _RawPrev;
            m_rawCurrState = _RawCurr;
            m_rawDelta = _RawDelta;
            m_prevState = _Prev;
            m_currState = _Curr;
            m_delta = _Delta;
        }
    }
    #endregion
}