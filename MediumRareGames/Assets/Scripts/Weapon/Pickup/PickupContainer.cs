using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class PickupContainer : MonoBehaviour
    {
        [SerializeField] private Pickup m_pickup;
        [SerializeField] private Animator m_animator;

        private Vector3 m_dropPoint;

        public Vector3 DropPoint
        {
            get { return m_dropPoint; }
            set { m_dropPoint = value; }
        }

        #region Unity Messages
        /// <summary>Initialization</summary>
        private void Start()
        {
            m_pickup.AllowPickup(false);
        }

        private void OnDestroy()
        {
            PickupManager.Instance.AddDropPoint(m_dropPoint);
        }

        /// <summary>When colliding</summary>
        private void OnCollisionEnter(Collision _Coll)
        {  
            m_pickup.AllowPickup(true);
            m_animator.SetTrigger("Open");
            GetComponent<BoxCollider>().enabled = false;
            Destroy(GetComponent<Rigidbody>());
        }
        #endregion
    }
}
