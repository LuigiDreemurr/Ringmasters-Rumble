/*
-----------------------------------------------------------------------------
       Created By Brandon Vout
-----------------------------------------------------------------------------
   Settings.DefaultSettings
       - The settings asset that stores the default values for KotH, LMS, & Gameplay Settings

   Details:
       - 
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "DefaultSettings", menuName = Directory + "DefaultSettings")]
    public class DefaultSettings : Asset
    {
        #region Match

        [SerializeField] private int m_roundsToWin = 3;

        /// <summary>How many rounds a player must win to win the match</summary>
        public int RoundsToWin { get { return m_roundsToWin; } }

        #endregion  // Match

        #region Weapon

        [SerializeField] private Weapon.Type m_weapon = Weapon.Type.GumballLauncher;
        
        public Weapon.Type DefaultWeapon { get { return m_weapon; } }

        #endregion  // Weapon

        #region LMS

        [SerializeField] private int m_lives = 1;
        [SerializeField] private float m_lmsRespawnTime = 2.0f;
        [SerializeField] private float m_timeLimit = 45.0f;

        /// <summary>How many lives each player has</summary>
        public int Lives { get { return m_lives; } }

        /// <summary>How long does it take for a player to respawn</summary>
        public float LMSRespawnTime { get { return m_lmsRespawnTime; } }

        /// <summary> How long rounds last </summary>
        public float TimeLimit { get { return m_timeLimit; } }

        #endregion  // LMS

        #region KOTH

        [SerializeField] private int m_pointsToCapture = 1; // Points needed to capture to win
        [SerializeField] private int m_possiblePoints = 1;  // How many different points are spawned in map
        [SerializeField] private float m_kothRespawnTime = 2.0f;
        [SerializeField] private float m_countdownTime = 30.0f;
        
        public int PointsToCapture { get { return m_pointsToCapture; } }

        /// <summary> How many points to cycle through </summary>
        public int PossiblePoints { get { return m_possiblePoints; } }

        /// <summary>How long does it take for a player to respawn</summary>
        public float KotHRespawnTime { get { return m_kothRespawnTime; } }

        /// <summary> How long players have to stand on the point </summary>
        public float CountdownTime { get { return m_countdownTime; } }

        #endregion  // KotH
    }

}
