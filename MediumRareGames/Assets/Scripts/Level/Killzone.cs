using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour {


    private float damage = 300f;

    //private void OnCollisionEnter(Collision collision)
    //{
    //            Health.Health health = GetComponent<Health.Health>();
    //    if(health != null)
    //    {
    //        health.Damage(new Health.DamageSource(damage, gameObject));
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        Health.Health health = other.GetComponent<Health.Health>();
        if(health != null)
        {
            health.Damage(new Health.DamageSource(damage, gameObject));
            MatchHandler.FellOff();
        }
    }
}
