/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   Settings.LastManStanding
       - The settings asset that the LMS gamemode uses

   Details:
       - 
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "LastManStandingSettings", menuName = Directory + "Gamemode/LastManStanding")]
    public class LastManStanding : Asset
    {
        [SerializeField] private int m_lives = 1;
        [SerializeField] private float m_respawnTime = 2.0f;
        [SerializeField] private float m_timeLimit = 45.0f;

        /// <summary>How many lives each player has</summary>
        public int Lives { get { return m_lives; } }	

        /// <summary>How long does it take for a player to respawn</summary>
        public float RespawnTime { get { return m_respawnTime; } }

        /// <summary> How long rounds last </summary>
        public float TimeLimit { get { return m_timeLimit; } }

        public void SetLives(int _Lives) { m_lives = _Lives; }
        public void SetRespawnTime(float _Time) { m_respawnTime = _Time; }
        public void SetTimeLimit(float _Time) { m_timeLimit = _Time; }
    }
}