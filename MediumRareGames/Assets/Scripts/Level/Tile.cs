/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   Tile
       - The script attached to every tile in the arena allowing per tile
         information

   Details:
       - Contains its position based on tile size
       - Can be set to be a player spawn point or weapon drop point
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] [ReadOnly] private Vector3 m_position; //The tile position in accordance to other tiles

        [SerializeField] private bool m_playerSpawnPoint = false; //Can a player spawn here
        [SerializeField] private bool m_weaponDropPoint = false; //Can a weapon drop here
        [SerializeField] private bool m_kothSpawnPoint = false; //Can a KotH point spawn here?

        public Vector3 Position { get { return m_position; } }
        public bool PlayerSpawnPoint { get { return m_playerSpawnPoint; } }
        public bool WeaponDropPoint { get { return m_weaponDropPoint; } }
        public bool KothSpawnPoint { get { return m_kothSpawnPoint; } }

        /// <summary>Initialization</summary>
        private void Start()
        {
            //Get the tile position using the world position and tilesize
            Vector3 tileSize = Settings.Level.Builder.Get.TileSize;
            m_position.x = Mathf.Round(transform.position.x / tileSize.x);
            m_position.y = Mathf.Round(transform.position.y / tileSize.y);
            m_position.z = Mathf.Round(transform.position.z / tileSize.z);
        }
    }
}
