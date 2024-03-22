/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   Weapon.Carrier
       - The weapon script attached to the 'Player' that holds the weapon

   Details:
       - Handles what weapon is currently held
       - Can pickup/discard weapons
-----------------------------------------------------------------------------
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class Carrier : MonoBehaviour
    {
        #region Data Members
        [SerializeField] private Transform m_weaponParent;
        [SerializeField][ReadOnly] private Weapon m_weapon; //Current weapon
        [SerializeField] [ReadOnly] private Type m_weaponType; //The current type

        private Settings.GeneralWeapon m_settings;
        #endregion

        #region Properties
        /// <summary>The current weapon</summary>
        public Weapon Weapon { get { return m_weapon; } }

        /// <summary>The current weapon type</summary>
        public Type WeaponType { get { return m_weaponType; } }
        #endregion

        /// <summary>Initialization</summary>
        private void Start ()
        {
            m_settings = GlobalSettings.Get.Weapon.General;
            PickupWeapon(m_settings.DefaultWeapon);
	    }

        #region Public Methods
        /// <summary>Picks up a weapon (adds the right component)</summary>
        /// <param name="_WeaponType">The weapon to be picked up</param>
        public void PickupWeapon(Type _WeaponType)
        {
            //Discard any existing weapon
            if (m_weapon != null)
                m_weapon.DestroyWeapon();

            //Pickup specified weapon
            CreateWeapon(_WeaponType);
        }

        /// <summary>Discard the current weapon and then pickup the default weapon</summary>
        public void DiscardWeapon()
        {
            //Don't allow discarding of default weapon
            if (m_weaponType == m_settings.DefaultWeapon)
                return;

            //Remove the weapon component
            if(m_weapon != null)
                m_weapon.DestroyWeapon();

            //Pickup default weapon
            CreateWeapon(m_settings.DefaultWeapon);
        }
        #endregion

        #region Private Methods
        /// <summary>Instantiates a weapon for the carrier</summary>
        /// <param name="_Type">The type of weapon being made</param>
        private void CreateWeapon(Type _Type)
        {
            //Choose the correct weapon prefab based on type
            GameObject weaponPrefab = null;
            if (_Type == Type.GumballLauncher)
                weaponPrefab = m_settings.GumballLauncherPrefab;
            else if (_Type == Type.SharkZooka)
                weaponPrefab = m_settings.SharkZookaPrefab;
            else if (_Type == Type.WallGun)
                weaponPrefab = m_settings.WallInACanPrefab;

            //Instansiate the weapon and get the weapon component
            m_weapon = Instantiate(weaponPrefab, m_weaponParent, false).GetComponent<Weapon>();
            m_weapon.Carrier = this;
            m_weaponType = _Type;
        }
        #endregion
    }
}
