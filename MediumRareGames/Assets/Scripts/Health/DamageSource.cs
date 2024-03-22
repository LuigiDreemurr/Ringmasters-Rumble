/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   DamageSource
       - A simple class used for controlling damage to a 'Health' and tracking
         where that damage came from

   Details:
       - Damage can be modified after construction. Source cannot
       - Can be cast to a RestoreSource
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Health
{
    public class DamageSource
    {
        private float m_damage; //How much damage
        private GameObject m_source; //Where the damage is coming from

        #region Properties
        /// <summary>The damage to deal</summary>
        public float Damage
        {
            get { return m_damage; }
            set { m_damage = value; }
        }

        /// <summary>Returns the source of the damage</summary>
        public GameObject Source { get { return m_source; } }
        #endregion

        /// <summary>Constructs a DamageSource</summary>
        /// <param name="_Damage">How much damage will be dealt</param>
        /// <param name="_Source">Where the damage is coming from</param>
        public DamageSource(float _Damage, GameObject _Source)
        {
            m_damage = _Damage;
            m_source = _Source;
        }

        /// <summary>Explicit conversion to a restore source. \nPositive damage is negative restore. \nNegative damage is positive restore</summary>
        /// <param name="_DamageSource">The RestoreSource being converted</param>
        public static explicit operator RestoreSource(DamageSource _DamageSource)
        {
            return new RestoreSource(_DamageSource.Damage * -1, _DamageSource.Source);
        }
    }

}
