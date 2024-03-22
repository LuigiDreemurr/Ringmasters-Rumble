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
    public class WallPool : ObjectPool<WallPool, Wall>
    {
        [SerializeField] [ReadOnly] private int m_total = 0; //The size of the gumball pool (for inspector)

        /// <summary>Size is based on how many possible gumballs there can be at once.
        /// It is determined by the player count, the gumball firerate and the gumball alive time</summary>
        /// <returns>The maximum amount of gumballs available</returns>
        protected override int Size() 
        {
            m_total = 20;

            return m_total;
        }
    }
}
