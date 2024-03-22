/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   GumballPool
       - An implementation of ObjectPool for the gumballs

   Details:
       - The pool size is determined by the maximum amount of gumballs possible
         during gameplay
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class GumballPool : ObjectPool<GumballPool, Gumball>
    {
        [SerializeField] [ReadOnly] private int m_total = 0; //The size of the gumball pool (for inspector)

        /// <summary>Size is based on how many possible gumballs there can be at once.
        /// It is determined by the player count, the gumball firerate and the gumball alive time</summary>
        /// <returns>The maximum amount of gumballs available</returns>
        protected override int Size() 
        {
            //Calculate how many bullets could possibly be on screen
            int playerCount = LobbyMenu.Players.Count;
            float perSecond = 1f / GlobalSettings.Get.Weapon.GumballLauncher.FireRate;
            float aliveTime = 3;

            m_total = playerCount * (int)(perSecond * aliveTime);

            return m_total;
        }
    }
}
