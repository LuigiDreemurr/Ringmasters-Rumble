/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   Desctructible
       - A simple abstract class that hooks into a Health's OnDeath event allowing
         for easy destructibles to be made
         

   Details:
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Health;

[RequireComponent(typeof(Health.Health))]
public abstract class Destructible : MonoBehaviour
{
    /// <summary>Initialization</summary>
    protected void Start()
    {
        GetComponent<Health.Health>().OnDeath += OnDeath;        
    }

    /// <summary>What to do when the destructible dies</summary>
    /// <param name="_Health">The destructible's Health</param>
    /// <param name="_DamageSource">The damage information</param>
    protected abstract void OnDeath(Health.Health _Health, DamageSource _DamageSource);
}
