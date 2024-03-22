using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class Gumball : Bullet, IPooled
    {
        private new Rigidbody rigidbody;
        private SimpleTimer timer;
        private int bounceAmount;

        [SerializeField] private GumballParticleController m_particleController;

        public GumballParticleController ParticleController { get { return m_particleController; } }

        /// <summary>Initialize</summary>
        public void OnInit()
        {
            rigidbody = GetComponent<Rigidbody>();
            timer = GetComponent<SimpleTimer>();
            timer.OnEnd += (SimpleTimer _Timer) => { GumballPool.Kill(this); };
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
            //Reset bounce amount
            bounceAmount = GlobalSettings.Get.Weapon.GumballLauncher.BounceAmount;

            //Add force
            rigidbody.AddRelativeForce(Vector3.forward * GlobalSettings.Get.Weapon.GumballLauncher.Speed * GlobalSettings.Get.Builder.TileSize.x);

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

            m_particleController.Play();

            //When hitting anything with health
            Health.Health health = c.gameObject.GetComponent<Health.Health>();
            if(health != null)
            {
                health.Damage(new Health.DamageSource(GlobalSettings.Get.Weapon.GumballLauncher.Damage, Weapon.Carrier.gameObject));
                GumballPool.Kill(this);
            }

            //Already bounced max amount, destroy
            if (bounceAmount <= 0)
                GumballPool.Kill(this);
            //Decrement bounce amount
            else
                bounceAmount--;

        }


    }
}
