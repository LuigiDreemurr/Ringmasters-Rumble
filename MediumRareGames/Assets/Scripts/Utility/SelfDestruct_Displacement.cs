/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   SelfDestruct_Displacement
       - A simple script that destroys its own game object after traveling a certain
         distance

   Details:
       - 
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct_Displacement : MonoBehaviour
{
    [SerializeField] private float m_displacement; //How much displacement until destruction

    private Vector3 m_previousPosition;
    private float m_currDisp; //The current distance traveled/displacement

    /// <summary>Initialization</summary>
	private void Start()
    {
        //Cache starting position
        m_previousPosition = transform.position;
    }

    /// <summary>Called once per frame</summary>
    private void Update()
    {
        m_currDisp += Vector3.Distance(transform.position, m_previousPosition); //Calculate the displacement this frame
        m_previousPosition = transform.position; //Update the previous position

        //Destroy the game object when the current displacement exceeds m_displacement
        if (m_currDisp >= m_displacement)
            Destroy(gameObject);
    }


}
