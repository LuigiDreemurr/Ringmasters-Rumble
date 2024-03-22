using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class GumballLauncher : Weapon
    {

        #region Variables
        private float fireRate;
        private float time;
        public Transform Gumball_Barrel;
        public Rigidbody Gumball;

        #endregion

        #region Unity Messages
        /// <summary>Initialization</summary>
        private void Start()
        {
            //Debug.Log("Created gumball");

            // set variables for instance of Gumball Launcher
            time = 0.0f;
            
            fireRate = GlobalSettings.Get.Weapon.GumballLauncher.FireRate;
            Init(GlobalSettings.Get.Weapon.GumballLauncher.WeaponDuration);
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
            Gumball gumball = GumballPool.Spawn(Gumball_Barrel.position, Carrier.transform.rotation);
            gumball.Weapon = this;

            PlayerController pController = Carrier.GetComponent<PlayerController>();
            PlayerIdentifier pIdentifier = pController.Container.Container.GetComponent<PlayerIdentifier>();
            Color pColor = pIdentifier.Info.Color;

            MeshRenderer renderer = gumball.GetComponent<MeshRenderer>();
            renderer.material.color = pColor;

            gumball.ParticleController.SetColor(pColor);

            //Start some fire cooldown
            time = 0;
        }

        protected override bool CanActivate()
        {
            return time > fireRate; //&& Ammo > 0;
        }


    }

}
