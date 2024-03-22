/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   DeathHandler
       - Simple class with functions allowing easy spawning/respawning for a player
          
   Details:
       - 
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    [SerializeField] private float m_yOffset = 3;

    [SerializeField] private Transform m_player;
    [SerializeField] private Health.Health m_health;

    public Health.Health Health { get { return m_health; } }
    public Transform Player { get { return m_player; } }

    /// <summary>Coroutine to handle respawning the player. Will find a random spawn point and reset the player to that position</summary>
    private IEnumerator RespawnRoutine(float _RespawnTime)
    {
        m_player.gameObject.SetActive(false); //Disable
        m_player.GetComponent<Weapon.Carrier>().DiscardWeapon(); //Discard weapon
        MatchHandler.PlayerDeath();

        //Wait for a certain amount of time 
        yield return new WaitForSeconds(_RespawnTime);

        //Find spawn points
        List<Level.Tile> spawnPoints = GetSpawnPoints();

        //Spawn player at given position
        Spawn(spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position);  // TODO: Players can still move on Respawn, we could add a timer and invincibility frames where the player cannot move to fix this
        GameObject.FindGameObjectWithTag("Announcer")?.GetComponent<AnnouncerChatter>()?.Play_Player_Respawn();
    }

    /// <summary>Will respawn the player after the respawn time at a random spawn point</summary>
    /// <param name="_RespawnTime">How long till the player respawns</param>
    public void Respawn(float _RespawnTime)
    {
        StartCoroutine(RespawnRoutine(_RespawnTime));
    }

    /// <summary>Will spawn the player (enable) at a given spawn point</summary>
    /// <param name="_SpawnPoint">The spawn point position</param>
    public void Spawn(Vector3 _SpawnPoint)
    {
        //Make sure player spawns above the tile
        _SpawnPoint.y += m_yOffset;

        //Setup transform
        m_player.GetComponent<Health.Health>().Reset(); //Reset health
        m_player.transform.position = _SpawnPoint;
        m_player.transform.rotation = Quaternion.identity; //Default rotation

        //Enable player
        m_player.gameObject.SetActive(true);
    }

    /// <summary>Finds the available spawn points</summary>
    /// <returns>A list of tile spawn points</returns>
    static public List<Level.Tile> GetSpawnPoints()
    {
        return Level.Utility.FindFilteredTiles((Level.Tile _Tile) => { return _Tile.PlayerSpawnPoint; });
    }
}
