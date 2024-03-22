using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Weapon
{
    public class PickupManager : MonoBehaviour
    {
        #region Singleton Setup
        static private PickupManager s_instance;
        static public PickupManager Instance { get { return s_instance; } }

        private void Awake()
        {
            if (s_instance == null)
                s_instance = this;
            else
                ConsoleLogging.Log.Warning(this, "There are two pickup managers in the scene");
        }
        #endregion

        #region Data Members
        private Settings.WeaponPickup m_settings;

        //The available weapons that are able to 'drop'
        private List<Settings.WeaponPickup.WeaponAvailability> m_availableWeapons;

        private List<Vector3> m_dropPoints;

        #endregion

        #region Unity Messages
        /// <summary>Initialization</summary>
        private void Start()
        {
            m_settings = GlobalSettings.Get.Weapon.Pickup;
            m_availableWeapons = m_settings.AvailableWeapons;
            m_dropPoints = GetDropPoints();

            StartCoroutine(SpawnRoutine());
        }
        #endregion

        #region Public Methods

        public void AddDropPoint(Vector3 _DropPoint)
        {
            m_dropPoints.Add(_DropPoint);
        }

        #endregion

        #region Helper Methods
        /// <summary>The simple coroutine that will constantly spawn weapon pickups</summary>
        private IEnumerator SpawnRoutine()
        {
            //Wait initial time
            yield return new WaitForSeconds(m_settings.InitialSpawnDelay);

            while(true)
            {
                //Spawn
                SpawnPickup();

                //Wait the spawn rate
                yield return new WaitForSeconds(m_settings.SpawnRate);
            }
        }

        /// <summary>Will spawn a pickup for a random weapon at a random position</summary>
        /// <returns>The pickup container game object that was instansiated</returns>
        private void SpawnPickup()
        {
            if (m_dropPoints.Count == 0)
                return;

            int randomIndex = Random.Range(0, m_dropPoints.Count);
            Vector3 randomDropPoint = m_dropPoints[randomIndex];
            m_dropPoints.RemoveAt(randomIndex);

            //Choose a random weapon
            var weapon = m_availableWeapons[Random.Range(0, m_availableWeapons.Count)];

            //Get a random rotation
            float randomY = Random.Range(m_settings.MinChestRot, m_settings.MaxChestRot);
            Quaternion randomRot = Quaternion.Euler(0, randomY, 0);

            //Spawn that prefab
            PickupContainer pickupContainer = Instantiate(weapon.Prefab, randomDropPoint + m_settings.PickupSpawnOffset, randomRot, transform).GetComponent<PickupContainer>();
            pickupContainer.DropPoint = randomDropPoint;
            GameObject.FindGameObjectWithTag("Announcer")?.GetComponent<AnnouncerChatter>()?.Play_WeaponDrop();
        }

        private List<Vector3> GetDropPoints()
        {
            return Level.Utility.FindFilteredTiles((Level.Tile _Tile) =>
            {
                return _Tile.WeaponDropPoint;
            }).Select((Level.Tile _Tile)=> { return _Tile.transform.position; }).ToList();
        }
        #endregion
    }
}
