/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   SelfDestruct_Distance
       - A simple script that destroys its own game object after a being a certain 
         distance away from its start point

   Details:
       - This is not 'distance traveled' but merely the distance between the 
         current position and starting position
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct_Distance : MonoBehaviour
{
    [SerializeField] private float m_distance; //How far from the starting pos until destruction

    private Vector3 m_startingPosition;

    /// <summary>Initialization</summary>
	private void Start()
    {
        //Cache starting position
        m_startingPosition = transform.position;
    }

    /// <summary>Called once per frame</summary>
    private void Update()
    {
        //Destroy gameobject when the distance from the starting pos exceeds m_distance
        if (Vector3.Distance(m_startingPosition, transform.position) >= m_distance)
            Destroy(gameObject);
    }
}
