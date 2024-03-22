/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   Settings.SharkZooka
       - 

   Details:
       - Copy and namechange of Settings.GumballLauncher for now
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "SharkZookaSettings", menuName = SharkZooka.Directory)]
    public class SharkZooka : Settings.WeaponSetting
    {
        /// <summary>The appropriate directory for this weapon setting in the asset creation menu</summary>
        public new const string Directory = WeaponSetting.Directory + "SharkZooka";

        [SerializeField] private float m_speed = 10;
        [SerializeField] private float m_damage = 20;
        [SerializeField] private float m_fireRate = 1f;

        public float Speed { get { return m_speed; } }
        public float Damage { get { return m_damage; } }
        public float FireRate { get { return m_fireRate; } }
    }
}
