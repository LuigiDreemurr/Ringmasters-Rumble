/*
-----------------------------------------------------------------------------
       Created By Brandon Vout
-----------------------------------------------------------------------------
   Settings.GameplaySettings
       - The settings asset that stores the Gameplay Settings

   Details:
       - Stores default weapon
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "GameplaySettings", menuName = Directory + "GameplaySettings")]
    public class GameplaySettings : Asset
    {
        [SerializeField] private Weapon.Type m_startingWeapon = Weapon.Type.GumballLauncher;
        
        public Weapon.Type StartingWeapon { get { return m_startingWeapon; } }

        public void SetWeapon(Weapon.Type _Weapon) { m_startingWeapon = _Weapon; }
    }
}