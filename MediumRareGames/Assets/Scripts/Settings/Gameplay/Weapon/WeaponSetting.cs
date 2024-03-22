/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   Settings.Weapon
       - A abstract class that different weapon settings can inherit from

   Details:
       - Shared settings that all weapons use is bullet prefab and ammo
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    public abstract class WeaponSetting : Settings.Asset
    {
        /// <summary>The weapons settings directory in the asset creation menu</summary>
        public new const string Directory = Asset.Directory + "Weapons/";

        //Each weapon has a bullet prefab it can instantiate
        [SerializeField] private GameObject m_bulletPrefab;

        //Weapons are based on a duration, weapon will be discarded when time is up
        [SerializeField] private float m_weaponDuration;

        //Each weapon has a default starting ammo
        //[SerializeField] private int m_ammo;

        /// <summary>The bullet prefab for this weapon</summary>
        public GameObject BulletPrefab { get { return m_bulletPrefab; } }

        public float WeaponDuration { get { return m_weaponDuration; } }

        /// <summary>The initial ammo for this weapon</summary>
        //public int Ammo { get { return m_ammo; } }
    }
}
