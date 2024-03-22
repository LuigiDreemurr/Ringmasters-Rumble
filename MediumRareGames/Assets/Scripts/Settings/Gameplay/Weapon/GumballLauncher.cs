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
    [CreateAssetMenu(fileName = "GumballLauncherSettings", menuName = GumballLauncher.Directory)]
    public class GumballLauncher : Settings.WeaponSetting
    {
        /// <summary>The appropriate directory for this weapon setting in the asset creation menu</summary>
        public new const string Directory = WeaponSetting.Directory + "GumballLauncher";

        [SerializeField] private int m_bounceAmount = 3;
        [SerializeField] private float m_speed = 10;
        [SerializeField] private float m_damage = 20;
        [SerializeField] private float m_fireRate = 2f;

        public int BounceAmount { get { return m_bounceAmount; } }
        public float Speed { get { return m_speed; } }
        public float Damage { get { return m_damage; } }
        public float FireRate { get { return m_fireRate; } }
    }
}
