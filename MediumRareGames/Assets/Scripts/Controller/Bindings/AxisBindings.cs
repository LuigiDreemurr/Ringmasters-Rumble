/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   AxisBindings
       - A class that represents the different axes bound to a controller

   Details:
       - Contains bindings for both individual axes (IE a trigger or a 1D stick)
         and dual axes (IE X&Y together)
-----------------------------------------------------------------------------
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    [Serializable]
    public class AxisBindings
    {
        #region Classes
        /// <summary>A simple class that pairs together two strings representing X/Y axis names</summary>
        [Serializable]
        public class DualAxisPair
        {
            //Axis name for a X axis
            [SerializeField] private string m_XAxis;

            //Axis name for a Y axis 
            [SerializeField] private string m_YAxis;

            /// <summary>Returns the axis name for the X axis</summary>
            public string XAxis { get { return m_XAxis; } }

            /// <summary>Returns the axis name for the Y axis</summary>
            public string YAxis { get { return m_YAxis; } }
        }

        /// <summary>Wrapper class for Binding using Axis and string</summary>
        [Serializable]
        public class AxisBinding : Binding<Axis, string>
        {
            /// <summary>Constructs an AxisBinding</summary>
            /// <param name="_Axis">The controller axis to be bound</param>
            public AxisBinding(Axis _Axis) : base(_Axis) { }
        }

        /// <summary>Wrapper class for Binding using DualAxis and DualAxisPair</summary>
        [Serializable]
        public class DualAxisBinding : Binding<DualAxis, DualAxisPair>
        {
            /// <summary>Constructs a DualAxisBinding</summary>
            /// <param name="_DualAxis">The controller axis to be bound</param>
            public DualAxisBinding(DualAxis _DualAxis) : base(_DualAxis) { }
        }
        #endregion

        [SerializeField] private List<DualAxisBinding> m_dualBindings; //The axes names bound to dual controller axes
        [SerializeField] private List<AxisBinding> m_singleBindings; //The axis name bound to a controller axis

        /// <summary>Returns the DualAxisBindings</summary>
        public List<DualAxisBinding> DualBindings { get { return m_dualBindings; } }

        /// <summary>Returns the single AxisBindings</summary>
        public List<AxisBinding> SingleBindings { get { return m_singleBindings; } }
    }
}
