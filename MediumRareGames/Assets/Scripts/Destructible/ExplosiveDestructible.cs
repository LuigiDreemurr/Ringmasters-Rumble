using System;
using System.Collections;
using System.Collections.Generic;
using Health;
using UnityEngine;

public class ExplosiveDestructible : Destructible {

    [SerializeField] private GameObject range;
    [SerializeField] private GameObject explosion;

    protected override void OnDeath(Health.Health _Health, DamageSource _DamageSource)
    {
        range.SetActive(true);
        explosion.SetActive(true);


        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            GetComponent<Health.Health>().Damage(new DamageSource(10, gameObject));
    }
}
