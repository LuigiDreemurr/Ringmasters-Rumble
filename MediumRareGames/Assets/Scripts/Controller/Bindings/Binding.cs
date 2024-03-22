/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   Binding
       - A simple class representing a pair where the 'link' is bound to the 
         'origin'

   Details:
       - Does not really enforce any constraints on the whole 'Binding' aspect.
         It can be treated as a pair with different naming conventions
-----------------------------------------------------------------------------
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Controller
{
    [Serializable]
    public class Binding<A, B>
    {
        [SerializeField] private A m_origin;
        [SerializeField] private B m_link; //What is bound to the origin

        /// <summary>Returns the binding origin</summary>
        public A Origin { get { return m_origin; } }

        /// <summary>Returns what is bound to the origin (the link)</summary>
        public B Link { get { return m_link; } }

        /// <summary>Constructs a binding</summary>
        /// <param name="_Origin">What is going to have something bound to it</param>
        public Binding(A _Origin)
        {
            m_origin = _Origin;
        }
    }
}
