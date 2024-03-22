using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public abstract class Bullet : MonoBehaviour
    {
        private Weapon m_weapon; //Weapon stores reference to its carrier

        /// <summary>The Carrier for this weapon</summary>
        public Weapon Weapon
        {
            get { return m_weapon; }
            set { m_weapon = value; }
        }
    }
}
