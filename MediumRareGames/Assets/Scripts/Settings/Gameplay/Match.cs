/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   Settings.Match
       - The settings asset for relevant information that the Match system can use

   Details:
       - Currently only contains the int representing how many rounds a player must
         win to fully win the match
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "MatchSettings", menuName = Directory + "Match")]
    public class Match : Asset
    {
        [SerializeField] private int m_roundsToWin = 3;

        /// <summary>How many rounds a player must win to win the match</summary>
        public int RoundsToWin { get { return m_roundsToWin; } }
        public void SetRounds(int _Rounds) { m_roundsToWin = _Rounds; }
	}
}