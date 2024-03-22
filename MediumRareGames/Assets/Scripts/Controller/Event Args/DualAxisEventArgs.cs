/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   DualAxisEventArgs
       - EventArgs to represent a controller dual axis event

   Details:
       - Contains what axis was used and the value of the axis (Vector2/XY)
-----------------------------------------------------------------------------
*/

using System;
using UnityEngine;

namespace Controller
{
    public class DualAxisEventArgs : EventArgs
    {
        private DualAxis m_axis; //The axis used
        private Vector2 m_value; //The values of the axis used

        /// <summary>Returns the axis used</summary>
        public DualAxis Axis { get { return m_axis; } }

        /// <summary>Returns the values of the axis used</summary>
        public Vector2 Value { get { return m_value; } }

        /// <summary>Constructs a DualAxisEventArgs</summary>
        /// <param name="_Axis">The axis used</param>
        /// <param name="_Value">The values of the axis used</param>
        public DualAxisEventArgs(DualAxis _Axis, Vector2 _Value)
        {
            m_axis = _Axis;
            m_value = _Value;
        }
    }
}
