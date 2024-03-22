/*
 -----------------------------------------------------------------------------
        Created By Wesley Ducharme
 -----------------------------------------------------------------------------
    ControllerMouseInputModule
        - A custom input module that allows for xinput controllers to traverse
          UI. Also support mouse events

    Details:
        - Need this so we can easily use UI with input that does not come from
          Unity's input manager
        - A lot of this code is taken directly from the 'Standalone Input Module'
          source I found using ILSpy. Did this because I wanted to replicate
          its functionality but just use different input with it
        - Not well documented for the fact that a lot was taken directly from ILSpy
 -----------------------------------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using XInputDotNetPure;

namespace XInput
{
    public class ControllerMouseInputModule : PointerInputModule
    {
        #region Singleton
        private static ControllerMouseInputModule s_instance;
        public static ControllerMouseInputModule Instance { get { return s_instance; } }


        protected override void Awake()
        {
            base.Awake();

            if (s_instance == null)
                s_instance = this;
            else
            {
                Debug.LogError("Second ControllerMouseInputModule exists...Destroying " + gameObject.name);
                Destroy(gameObject);
            }
        }

        #endregion

        #region Data Members
        //Controller button for submitting
        [SerializeField] private Button m_submitButton = Button.A;

        //Controller button for cancelling
        [SerializeField] private Button m_cancelButton = Button.B;

        //Controller stick for menu traversal
        [SerializeField] private DualAxis m_traverseAxis = DualAxis.LeftStick;

        //What controller is in control
        [SerializeField] private PlayerIndex m_controllerIndex;
        [Space]
        [SerializeField]
        private float m_actionsPerSecond = 10.0f;
        [SerializeField] private float m_repeatDelay = 0.5f;
        [Space]
        [SerializeField]
        private bool m_forceModuleActive;

        private ControllerManager m_controllerManager;
        private Controller m_controller;

        private Vector2 m_lastMousePosition;
        private Vector2 m_mousePosition;

        private int m_consecutiveMoveCount = 0;
        private float m_prevActionTime;
        private Vector2 m_lastMoveVector;
        private GameObject m_currentFocus;

        #region Properties
        public GameObject MouseFocus
        {
            get { return m_currentFocus; }
        }

        public PlayerIndex ControllerIndex
        {
            get { return m_controllerIndex; }
            set
            {
                m_controllerIndex = value;
                m_controller = m_controllerManager.GetController(m_controllerIndex);
            }
        }

        public Controller Controller
        {
            get { return m_controller; }
            set
            {
                m_controller = value;

                if (m_controller != null)
                    m_controllerIndex = m_controller.Index;
                else
                    m_controller = m_controllerManager.GetController(PlayerIndex.One);
            }
        }

        public Button SubmitButton
        {
            get { return m_submitButton; }
            set { m_submitButton = value; }
        }

        public Button CancelButton
        {
            get { return m_cancelButton; }
            set { m_cancelButton = value; }
        }

        public DualAxis TraverseAxis
        {
            get { return m_traverseAxis; }
            set { m_traverseAxis = value; }
        }
        #endregion

        #endregion

        #region Overrides
        /// <summary>Initialization</summary>
        protected override void Start()
        {
            base.Start();
            //Get reference to controller manager and a controller
            m_controllerManager = ControllerManager.Instance;
            m_controller = m_controllerManager.GetController(m_controllerIndex);
        }

        public override void UpdateModule()
        {
            if (!eventSystem.isFocused)
                return;

            //Cache mouse positions
            m_lastMousePosition = m_mousePosition;
            m_mousePosition = input.mousePosition;
        }

        /// <summary>When should this module activate</summary>
        public override bool ShouldActivateModule()
        {
            if (!base.ShouldActivateModule())
                return false;

            bool forceModuleActive = m_forceModuleActive;

            //Activate if submit/cancel is down
            forceModuleActive |= m_controller.GetButton(m_submitButton).Action == ButtonAction.Down;
            forceModuleActive |= m_controller.GetButton(m_cancelButton).Action == ButtonAction.Down;

            //Activate if traversal axis is not zero
            forceModuleActive |= m_controller.GetDualAxis(m_traverseAxis).CurrState.sqrMagnitude > 0;

            //Activate if mouse button is down
            forceModuleActive |= input.GetMouseButtonDown(0);

            return forceModuleActive;
        }

        /// <summary>What to do when activating the module</summary>
        public override void ActivateModule()
        {
            if (!eventSystem.isFocused)
                return;

            base.ActivateModule();
            m_mousePosition = input.mousePosition;
            m_lastMousePosition = input.mousePosition;

            GameObject selected = eventSystem.currentSelectedGameObject;
            if (selected == null)
                selected = eventSystem.firstSelectedGameObject;

            eventSystem.SetSelectedGameObject(selected, GetBaseEventData());

        }

        /// <summary>What to do when deactivating the module</summary>
        public override void DeactivateModule()
        {
            base.DeactivateModule();
            ClearSelection();
        }

        public override void Process()
        {
            if (!eventSystem.isFocused)
                return;

            bool flag = SendUpdateEvent();

            if (eventSystem.sendNavigationEvents)
            {
                if (!flag)
                    flag |= SendMoveEvent();
                if (!flag)
                    flag |= SendSubmitEvent();
            }

            if (input.mousePresent)
                ProcessMouseEvent(0);
        }
        #endregion

        #region Private Methods
        /// <summary>Handle sending basic update events (taken from StandaloneInputModule)</summary>
        private bool SendUpdateEvent()
        {
            if (eventSystem.currentSelectedGameObject == null)
                return false;

            BaseEventData eventData = GetBaseEventData();
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, eventData, ExecuteEvents.updateSelectedHandler);
            return eventData.used;
        }

        /// <summary>Handle sending move events (taken from StandaloneInputModule)</summary>
        private bool SendMoveEvent()
        {
            float unscaledTime = Time.unscaledTime;
            Vector2 moveVector = m_controller.GetDualAxis(m_traverseAxis).CurrState;

            if (Mathf.Approximately(moveVector.x, 0f) && Mathf.Approximately(moveVector.y, 0f))
            {
                m_consecutiveMoveCount = 0;
                return false;
            }

            bool flag2 = Vector2.Dot(moveVector, m_lastMoveVector) > 0;
            bool flag = ((!flag2 || this.m_consecutiveMoveCount != 1) ?
                (unscaledTime > m_prevActionTime + 1f / m_actionsPerSecond) :
                (unscaledTime > m_prevActionTime + m_repeatDelay));

            if (!flag)
                return false;

            AxisEventData eventData = GetAxisEventData(moveVector.x, moveVector.y, 0.6f);
            if (eventData.moveDir != MoveDirection.None)
            {
                if (eventSystem.currentSelectedGameObject == null)
                    eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);

                ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, eventData, ExecuteEvents.moveHandler);
                if (!flag2)
                    m_consecutiveMoveCount = 0;

                m_consecutiveMoveCount++;
                m_prevActionTime = unscaledTime;
                m_lastMoveVector = moveVector;
            }
            else
            {
                m_consecutiveMoveCount = 0;
            }

            return eventData.used;
        }

        /// <summary>Try to send submit/cancel events (taken from StandaloneInputModule)</summary>
        private bool SendSubmitEvent()
        {
            if (eventSystem.currentSelectedGameObject == null)
            {
                eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
                return false;
            }

            BaseEventData eventData = GetBaseEventData();

            if (m_controller.GetButton(m_submitButton).Action == ButtonAction.Down)
                ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, eventData, ExecuteEvents.submitHandler);
            if (m_controller.GetButton(m_cancelButton).Action == ButtonAction.Down)
                ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, eventData, ExecuteEvents.cancelHandler);

            return eventData.used;
        }

        /// <summary>Process all mouse events (taken from StandaloneInputModule)</summary>
        /// <param name="_ID">Mouse button</param>
        private void ProcessMouseEvent(int _ID)
        {
            MouseState pointerEventData = GetMousePointerEventData(_ID);
            MouseButtonEventData eventData = pointerEventData.GetButtonState(PointerEventData.InputButton.Left).eventData;

            m_currentFocus = eventData.buttonData.pointerCurrentRaycast.gameObject;

            ProcessMousePress(eventData);
            ProcessMove(eventData.buttonData);
            ProcessDrag(eventData.buttonData);

            ProcessMousePress(pointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData);
            ProcessDrag(pointerEventData.GetButtonState(PointerEventData.InputButton.Right).eventData.buttonData);

            ProcessMousePress(pointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData);
            ProcessDrag(pointerEventData.GetButtonState(PointerEventData.InputButton.Middle).eventData.buttonData);

            if (!Mathf.Approximately(eventData.buttonData.scrollDelta.sqrMagnitude, 0))
            {
                GameObject eventHandler = ExecuteEvents.GetEventHandler<IScrollHandler>(eventData.buttonData.pointerCurrentRaycast.gameObject);
                ExecuteEvents.Execute(eventHandler, eventData.buttonData, ExecuteEvents.scrollHandler);
            }

        }

        /// <summary>Handle processing mouse presses (taken from StandaloneInputModule)</summary>
        private void ProcessMousePress(MouseButtonEventData _Data)
        {
            PointerEventData buttonData = _Data.buttonData;
            GameObject gameObject = buttonData.pointerCurrentRaycast.gameObject;

            if (_Data.PressedThisFrame())
            {
                buttonData.eligibleForClick = true;
                buttonData.delta = Vector2.zero;
                buttonData.dragging = false;
                buttonData.useDragThreshold = true;
                buttonData.pressPosition = buttonData.position;
                buttonData.pointerPressRaycast = buttonData.pointerCurrentRaycast;

                DeselectIfSelectionChanged(gameObject, buttonData);

                GameObject gameObject2 = ExecuteEvents.ExecuteHierarchy(gameObject, buttonData, ExecuteEvents.pointerDownHandler);
                if (gameObject2 == null)
                    gameObject2 = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);

                float unscaledTime = Time.unscaledTime;
                if (gameObject2 == buttonData.lastPress)
                {
                    float num = unscaledTime - buttonData.clickTime;

                    if (num < 0.3f)
                        buttonData.clickCount++;
                    else
                        buttonData.clickCount = 1;

                    buttonData.clickTime = unscaledTime;
                }
                else
                {
                    buttonData.clickCount = 1;
                }

                buttonData.pointerPress = gameObject2;
                buttonData.rawPointerPress = gameObject;
                buttonData.clickTime = unscaledTime;
                buttonData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(gameObject);
                if (buttonData.pointerDrag != null)
                    ExecuteEvents.Execute(buttonData.pointerDrag, buttonData, ExecuteEvents.initializePotentialDrag);
            }

            if (_Data.ReleasedThisFrame())
            {
                ExecuteEvents.Execute(buttonData.pointerPress, buttonData, ExecuteEvents.pointerUpHandler);
                GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject);

                if (buttonData.pointerPress == eventHandler && buttonData.eligibleForClick)
                {
                    ExecuteEvents.Execute(buttonData.pointerPress, buttonData, ExecuteEvents.pointerClickHandler);
                }
                else if (buttonData.pointerDrag != null && buttonData.dragging)
                {
                    ExecuteEvents.ExecuteHierarchy(gameObject, buttonData, ExecuteEvents.dropHandler);
                }

                buttonData.eligibleForClick = false;
                buttonData.pointerPress = null;
                buttonData.rawPointerPress = null;

                if (buttonData.pointerDrag != null && buttonData.dragging)
                {
                    ExecuteEvents.Execute(buttonData.pointerDrag, buttonData, ExecuteEvents.endDragHandler);
                }

                buttonData.dragging = false;
                buttonData.pointerDrag = null;

                if (gameObject != buttonData.pointerEnter)
                {
                    HandlePointerExitAndEnter(buttonData, null);
                    HandlePointerExitAndEnter(buttonData, gameObject);
                }

            }
        }
        #endregion
    }
}