using System;
using System.Collections;
using System.Collections.Generic;
using Health;
using UnityEngine;

public class TestDestructible : Destructible
{
    protected override void OnDeath(Health.Health _Health, DamageSource _DamageSource)
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            GetComponent<Health.Health>().Damage(new DamageSource(10, gameObject));
    }
}
