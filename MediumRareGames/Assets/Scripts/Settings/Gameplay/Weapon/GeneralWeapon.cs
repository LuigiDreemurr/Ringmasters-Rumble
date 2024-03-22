/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   Settings.GeneralWeapon
       - The general settings for weapons

   Details:
       - 
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "GeneralWeaponSettings", menuName = GeneralWeapon.Directory)]
    public class GeneralWeapon : Settings.Asset
    {
        /// <summary>The appropriate directory for this weapon setting in the asset creation menu</summary>
        public new const string Directory = WeaponSetting.Directory + "General";

        [SerializeField] private Weapon.Type m_defaultWeapon;
        [SerializeField] private GameObject m_gumballLauncherPrefab;
        [SerializeField] private GameObject m_sharkZookaPrefab;
        [SerializeField] private GameObject m_wallInACanPrefab;

        public Weapon.Type DefaultWeapon { get { return m_defaultWeapon; } }
        public void SetWeapon(Weapon.Type _Weapon) { m_defaultWeapon = _Weapon; }
        public GameObject GumballLauncherPrefab { get { return m_gumballLauncherPrefab; } }
        public GameObject SharkZookaPrefab { get { return m_sharkZookaPrefab; } }
        public GameObject WallInACanPrefab { get { return m_wallInACanPrefab; } }
    }
}
