/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   Weapon.Weapon
       - A abstract class that works as a base for all weapons

   Details:
       - Abstract unity messages (to force implementation)
       - Abstract CanActivate/Activate to handle firing the weapon. Brought 
         together inside Fire method
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    /// <summary>Enum representing the three weapons</summary>
    public enum Type { GumballLauncher, SharkZooka, WallGun }

    public abstract class Weapon : MonoBehaviour
    {
        private Carrier m_carrier; //Weapon stores reference to its carrier

        /// <summary>The Carrier for this weapon</summary>
        public Carrier Carrier
        {
            get { return m_carrier; }
            set { m_carrier = value; }
        }

        protected void Init(float _Duration)
        {
            if (GlobalSettings.Get.Weapon.General.DefaultWeapon != m_carrier.WeaponType)
                StartCoroutine(WeaponDurationRoutine(_Duration));
        }



        #region Abstract/Virtual Methods
        /// <summary>What happens when a weapon is activated</summary>
        protected abstract void Activate();

        /// <summary>Can a weapon be activated</summary>
        /// <returns>Returns True if the weapon can be activated</returns>
        protected abstract bool CanActivate();
        #endregion

        /// <summary>Will activate the weapon if it can be activated</summary>
        public void Fire()
        {
            //Only activate when it can
            if (CanActivate())
            {
                Activate();
            }
        }

        /// <summary>Will destroy the current weapon</summary>
        public void DestroyWeapon()
        {
            Destroy(gameObject);
        }

        private IEnumerator WeaponDurationRoutine(float _Duration)
        {
            yield return new WaitForSeconds(_Duration);

            m_carrier.DiscardWeapon();
        }
    }

}
