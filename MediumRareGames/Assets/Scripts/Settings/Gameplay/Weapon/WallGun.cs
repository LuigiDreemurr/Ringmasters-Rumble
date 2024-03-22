/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   Settings.GumballLauncher
       - All settings relevant to the gumball launcher

   Details:
       - Whatever in here is temporary and should not be considered functional
       - Use this as a template for implementing a settings asset
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "WallGunSettings", menuName = WallGun.Directory)]
    public class WallGun : Settings.WeaponSetting
    {
        /// <summary>The appropriate directory for this weapon setting in the asset creation menu</summary>
        public new const string Directory = WeaponSetting.Directory + "WallGun";
        
        [SerializeField] private float m_speed = 10;
        [SerializeField] private float m_initDamage = 20;
        [SerializeField] private float m_pinnedDamage = 10;
        [SerializeField] private float m_pushForce = 0.25f;
        [SerializeField] private float m_fireRate = 2f;
        
        public float Speed { get { return m_speed; } }
        public float InitialDamage { get { return m_initDamage; } }
        public float PinnedDamage { get { return m_pinnedDamage; } }
        public float PushForce { get { return m_pushForce; } }
        public float FireRate { get { return m_fireRate; } }
    }
}
