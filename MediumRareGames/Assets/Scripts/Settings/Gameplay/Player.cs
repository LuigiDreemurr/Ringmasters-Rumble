/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   Settings.Player
       - The settings asset for relevant information applied to each player

   Details:
       - Currently only contains information involing player movement
       - Moving, aiming, dashing
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = Directory + "Player")]
    public class Player : Settings.Asset
    {
        [SerializeField] private float m_moveSpeed; //How fast the player moves
        [SerializeField] private float m_aimSpeed; //How fast the player aims
        //[SerializeField] private float m_dashForce; //How much force for the dash
        //[SerializeField] private float m_dashCooldown; //How long till the player can dash again
        //[SerializeField] private float m_dashTime; //How long till the player can move after dashing

        /// <summary>Player movement speed (properly scaled with tile size)</summary>
        public float MoveSpeed { get { return m_moveSpeed /* * tileScale */; } }

        /// <summary>Player aiming speed</summary>
        public float AimSpeed { get { return m_aimSpeed; } }

        ///// <summary>Player dash force</summary>
        //public float DashForce { get { return m_dashForce; } }

        ///// <summary>Player dash cooldown</summary>
        //public float DashCooldown { get { return m_dashCooldown; } }

        ///// <summary>Player movement cooldown after dashing</summary>
        //public float DashTime { get { return m_dashTime; } }
    }
}
