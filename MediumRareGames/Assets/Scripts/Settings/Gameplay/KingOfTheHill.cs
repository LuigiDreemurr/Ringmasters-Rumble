/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   Settings.KingOfTheHill
       - The settings asset that the KotH gamemode uses

   Details:
       - 
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "KingOfTheHillSettings", menuName = Directory + "Gamemode/KingOfTheHill")]
    public class KingOfTheHill : Asset
    {
        [SerializeField] private int m_pointsToCapture = 1; // Points needed to capture to win
        [SerializeField] private int m_possiblePoints = 1;  // How many different points are spawned in map
        [SerializeField] private float m_respawnTime = 2.0f;
        [SerializeField] private float m_countdownTime = 30.0f;

        /// <summary>How many lives each player has</summary>
        public int PointsToCapture { get { return m_pointsToCapture; } }

        /// <summary> How many points to cycle through </summary>
        public int PossiblePoints { get { return m_possiblePoints; } }

        /// <summary>How long does it take for a player to respawn</summary>
        public float RespawnTime { get { return m_respawnTime; } }

        /// <summary> How long players have to stand on the point </summary>
        public float CountdownTime { get { return m_countdownTime; } }

        public void SetPointsCapture(int _Count) { m_pointsToCapture = _Count; }
        public void SetPossiblePoints(int _Count) { m_possiblePoints = _Count; }
        public void SetRespawn(float _Time) { m_respawnTime = _Time; }
        public void SetCountdown(float _Time) { m_countdownTime = _Time; }
    }
}