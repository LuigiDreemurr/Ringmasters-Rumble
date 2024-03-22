using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class WallInaCan : Weapon
    {

        #region Variables
        private float fireRate;
        private float time;
        public Transform Wall_Barrel;

        #endregion

        #region Unity Messages
        /// <summary>Initialization</summary>
        private void Start()
        {
            //Debug.Log("Created gumball");

            // set variables for instance of Gumball Launcher
            time = 0.0f;
            
            fireRate = GlobalSettings.Get.Weapon.WallGun.FireRate;
            Init(GlobalSettings.Get.Weapon.WallGun.WeaponDuration);

        }

        /// <summary>Called once per frame</summary>
        private void Update()
        {
            time += Time.deltaTime;
        }
        #endregion

        protected override void Activate()
        {
            //Instead of instantiating use GumballPool
            Wall wall = WallPool.Spawn(Wall_Barrel.position, Carrier.transform.rotation);
            wall.Weapon = this;

            //Start some fire cooldown
            time = 0;
        }

        protected override bool CanActivate()
        {
            return time > fireRate;// && Ammo > 0;
        }


    }

}
