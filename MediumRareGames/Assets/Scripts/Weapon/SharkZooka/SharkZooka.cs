using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class SharkZooka : Weapon
    {
        #region Variables
        private float fireRate;
        private float time;
        public Transform Shark_Barrel;

        #endregion

        #region Unity Messages
        /// <summary>Initialization</summary>
        private void Start()
        {
            //Debug.Log("Created Shark");

            // set variables for instance of Shark Launcher
            time = 0.0f;

            fireRate = GlobalSettings.Get.Weapon.SharkZooka.FireRate;
            Init(GlobalSettings.Get.Weapon.SharkZooka.WeaponDuration);
        }

        /// <summary>Called once per frame</summary>
        private void Update()
        {
            time += Time.deltaTime;
        }
        #endregion

        protected override void Activate()
        {
            //Instead of instantiating use SharkPool
            Shark Shark = SharkPool.Spawn(Shark_Barrel.position, Carrier.transform.rotation);
            Shark.Weapon = this;
            Shark.SetOwner(Carrier.transform.GetComponent<Carrier>());

            //Start some fire cooldown
            time = 0;
        }

        protected override bool CanActivate()
        {
            return time > fireRate;// && Ammo > 0;
        }


    }

}
