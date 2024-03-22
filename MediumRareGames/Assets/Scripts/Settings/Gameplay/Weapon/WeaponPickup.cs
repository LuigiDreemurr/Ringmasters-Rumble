/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   Settings.WeaponPickup
       - The settings used for weapon pickups

   Details:
       - 
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "WeaponPickupSettings", menuName = WeaponPickup.Directory)]
    public class WeaponPickup : Settings.Asset
    {
        /// <summary>The appropriate directory for this weapon setting in the asset creation menu</summary>
        public new const string Directory = WeaponSetting.Directory + "Pickup";

        [System.Serializable]
        public class WeaponAvailability
        {
            [SerializeField] private Weapon.Type m_type;
            [SerializeField] private GameObject m_prefab;
            [SerializeField] private bool m_available;

            public Weapon.Type Type { get { return m_type; } }
            public GameObject Prefab { get { return m_prefab; } }
            public bool Available { get { return m_available; } }

            public WeaponAvailability(Weapon.Type _Type, GameObject _Prefab, bool _Available)
            {
                m_type = _Type;
                m_prefab = _Prefab;
                m_available = _Available;
            }
        }

        [SerializeField] private List<WeaponAvailability> m_weapons = new List<WeaponAvailability>(3)
        {
            new WeaponAvailability(Weapon.Type.GumballLauncher, null, true),
            new WeaponAvailability(Weapon.Type.SharkZooka, null, true),
            new WeaponAvailability(Weapon.Type.WallGun, null, true),
        };
        [Space]
        [SerializeField] private Vector3 m_pickupSpawnOffset = new Vector3(0, 20, 0);
        [SerializeField] private float m_initialSpawnDelay = 3.0f;
        [SerializeField] private float m_spawnRate = 3.0f;
        [Space]
        [SerializeField] private float m_minChestRot = -45;
        [SerializeField] private float m_maxChestRot = 45;


        public Vector3 PickupSpawnOffset { get { return m_pickupSpawnOffset; } }
        public float InitialSpawnDelay { get { return m_initialSpawnDelay; } }
        public float SpawnRate { get { return m_spawnRate; } }
        public float MinChestRot { get { return m_minChestRot; } }
        public float MaxChestRot { get { return m_maxChestRot; } }

        public List<WeaponAvailability> AvailableWeapons
        {
            get
            {
                List<WeaponAvailability> availableWeapons = new List<WeaponAvailability>();
                
                foreach(WeaponAvailability weapon in m_weapons)
                {
                    if (weapon.Available && weapon.Type != GlobalSettings.Get.Weapon.General.DefaultWeapon)
                        availableWeapons.Add(weapon);
                }

                return availableWeapons;

            }
        }

    }
}
