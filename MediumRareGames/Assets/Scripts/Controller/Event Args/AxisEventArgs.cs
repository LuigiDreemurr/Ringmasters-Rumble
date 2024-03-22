/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   AxisEventArgs
       - EventArgs to represent a controller single axis event

   Details:
       - Contains what axis was used and the value of the axis
-----------------------------------------------------------------------------
*/

using System;

namespace Controller
{
    public class AxisEventArgs : EventArgs
    {
        private Axis m_axis; //What axis was use
        private float m_value; //The value of that axis

        /// <summary>Returns the axis used</summary>
        public Axis Axis { get { return m_axis; } }

        /// <summary>Returns the value of the axis used</summary>
        public float Value { get { return m_value; } }

        /// <summary>Constructs an AxisEventArgs</summary>
        /// <param name="_Axis">The axis used</param>
        /// <param name="_Value">The value of the axus used</param>
        public AxisEventArgs(Axis _Axis, float _Value)
        {
            m_axis = _Axis;
            m_value = _Value;
        }
    }
}
