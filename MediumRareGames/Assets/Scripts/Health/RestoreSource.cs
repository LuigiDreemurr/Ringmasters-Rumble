/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   DamageSource
       - A simple class used for controlling restore to a 'Health' and tracking
         where that restore came from

   Details:
       - Health can be modified after construction. Source cannot
       - Can be cast to a DamageSource
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Health
{
    public class RestoreSource
    {
        private float m_health; //How much health to restore
        private GameObject m_source; //Where the restore is coming from

        #region Properties
        /// <summary>The health to restore</summary>
        public float Health
        {
            get { return m_health; }
            set { m_health = value; }
        }

        /// <summary>Returns the source of the damage</summary>
        public GameObject Source { get { return m_source; } }
        #endregion

        /// <summary>Constructs a RestoreSource</summary>
        /// <param name="_Damage">How much damage will be restored</param>
        /// <param name="_Source">Where the restore is coming from</param>
        public RestoreSource(float _Health, GameObject _Source)
        {
            m_health = _Health;
            m_source = _Source;
        }

        /// <summary>Explicit conversion to a damage source. \nPositive restore is negative damage. \nNegative restore is positive damage</summary>
        /// <param name="_RestoreSource">The RestoreSource being converted</param>
        public static explicit operator DamageSource(RestoreSource _RestoreSource)
        {
            return new DamageSource(_RestoreSource.Health * -1, _RestoreSource.Source);
        }
    }

}
