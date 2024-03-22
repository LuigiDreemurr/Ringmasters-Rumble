using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class Pickup : MonoBehaviour
    {
        #region Data Members
        private Collider m_collider;
        [SerializeField] private Type m_type;
        [SerializeField] private Color m_particleColor = Color.white;
        [Space]
        [SerializeField] private GameObject m_underGlow;
        [SerializeField] private GameObject m_groundCrack;

        /// <summary>The weapon this pickup represents</summary>
        public Type Type { get { return m_type; } }
        #endregion

        #region Unity Messages
        /// <summary>Called when a collider enteres this trigger</summary>
        private void OnTriggerEnter(Collider _Other)
        {
            Carrier carrier = _Other.GetComponent<Carrier>();

            //If a carrier collides have them pick up this weapon type
            if(carrier != null)
            {
                carrier.PickupWeapon(m_type);
                GameObject.FindGameObjectWithTag("Announcer")?.GetComponent<AnnouncerChatter>()?.Play_Pickup(m_type);
                //Destroy(gameObject); //Destroy pickup
                Destroy(gameObject.transform.parent.gameObject);
            }
        }

        #endregion

        #region Public Methods
        /// <summary>Toggles the pickup through enabling/disabling its collider</summary>
        public void AllowPickup(bool _CanPickup)
        {
            GetComponent<Collider>().enabled = _CanPickup;

            m_underGlow.SetActive(_CanPickup);
            m_underGlow.GetComponent<UnderGlowColor>().Color = m_particleColor;

            m_groundCrack.SetActive(_CanPickup);
        }
        #endregion
    }
}
