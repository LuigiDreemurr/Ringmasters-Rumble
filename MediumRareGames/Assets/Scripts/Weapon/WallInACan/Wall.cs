using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class Wall : Bullet, IPooled
    {
        private new Rigidbody rigidbody;
        private SimpleTimer timer;

        /// <summary>Initialize</summary>
        public void OnInit()
        {
            rigidbody = GetComponent<Rigidbody>();
            timer = GetComponent<SimpleTimer>();
            timer.OnEnd += (SimpleTimer _Timer) => { WallPool.Kill(this); };
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
            rigidbody.AddRelativeForce(Vector3.forward * GlobalSettings.Get.Weapon.WallGun.Speed * GlobalSettings.Get.Builder.TileSize.x);

            //Begin kill timer
            timer.Begin();
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
            if(health != null)
            {
                health.Damage(new Health.DamageSource(GlobalSettings.Get.Weapon.WallGun.InitialDamage, Weapon.Carrier.gameObject));
            }

        }

        public void DealPushDamage(Collision c)
        {
            Health.Health health = c.gameObject.GetComponent<Health.Health>();
            if (health != null)
            {
                health.Damage(new Health.DamageSource(GlobalSettings.Get.Weapon.WallGun.PinnedDamage, Weapon.Carrier.gameObject));
            }
        }

        public void PushPinnedPlayer(Collision c)
        {
            Vector3 push = transform.forward;
            push.z += GlobalSettings.Get.Weapon.WallGun.PushForce;
            c.transform.GetComponent<Rigidbody>().AddForce(push, ForceMode.Impulse);
        }

        public void DealAdditionalDamage()
        {
            WallPool.Kill(this);
        }
    }
}
