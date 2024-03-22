using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class Shark : Bullet, IPooled
    {
        private new Rigidbody rigidbody;
        private SimpleTimer timer;

        /// <summary>Initialize</summary>
        public void OnInit()
        {
            rigidbody = GetComponent<Rigidbody>();
            timer = GetComponent<SimpleTimer>();
            timer.OnEnd += (SimpleTimer _Timer) => { SharkPool.Kill(this); };
        }

        /// <summary>Reset/Clear needed values</summary>
        public void OnKill()
        {
            timer.Stop(); //Stop the alive timer
            rigidbody.velocity = Vector2.zero; //Reset velocity
        }

        /// <summary>Set needed values</summary>
        public void OnSpawn()
        {
            //Add force
            rigidbody.AddRelativeForce(Vector3.forward * GlobalSettings.Get.Weapon.SharkZooka.Speed * GlobalSettings.Get.Builder.TileSize.x);

            //Begin kill timer
            timer.Begin();
        }


        public void SetOwner(Carrier owner)
        {
            //set the bullet owner, so it wont ever chase the person who fired the shark
            transform.GetChild(0).GetComponent<SharkAI>().owner = owner;
        }

        /// <summary>When this object enters a collider</summary>
        /// <param name="c">Collision information</param>
        private void OnCollisionEnter(Collision c)
        {
            //Ignore self
            if (Weapon.Carrier.gameObject == c.gameObject)
                return;

            //When hitting anything with health
            Health.Health health = c.gameObject.GetComponent<Health.Health>();
            if (health != null)
            {
                health.Damage(new Health.DamageSource(GlobalSettings.Get.Weapon.SharkZooka.Damage, Weapon.Carrier.gameObject));
                SharkPool.Kill(this);
            }

        }
    }
}
