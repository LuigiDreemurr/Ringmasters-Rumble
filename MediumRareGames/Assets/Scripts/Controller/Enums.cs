/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   Details:
       - Relevent enums for the controller namespace/system
-----------------------------------------------------------------------------
*/

namespace Controller
{
    /// <summary>Available controller buttons</summary>
    public enum Button
    {
        A = 0,
        B,
        X,
        Y,
        LeftBumper,
        RightBumper,
        Back,
        Start,
        LeftStick,
        RightStick
    }

    /// <summary>Available button states (controller, keyboard, and mouse)</summary>
    public enum ButtonInputState
    {
        Down, Held, Up
    }

    /// <summary>Available controller axes (single)</summary>
    public enum Axis
    {
        LeftStick_X = 0,
        LeftStick_Y = 1,
        RightStick_X = 4,
        RightStick_Y = 5,
        DPad_X = 6,
        DPad_Y = 7,
        LeftTrigger = 9,
        RightTrigger = 10
    }

    /// <summary>Available controller axes (dual)</summary>
    public enum DualAxis
    {
        LeftStick,
        RightStick,
        DPad
    }

    /// <summary>Available mouse buttons (left/right click)</summary>
    public enum MouseButton
    {
        Left = 0, Right
    }
}

