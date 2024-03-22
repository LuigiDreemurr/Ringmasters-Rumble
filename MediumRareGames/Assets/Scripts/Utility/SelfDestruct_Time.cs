/*
-----------------------------------------------------------------------------
       Created By Wesley Ducharme
-----------------------------------------------------------------------------
   SelfDestruct_Time
       - A simple script that destroys its own game object after a certain amount of time

   Details:
       - 
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct_Time : MonoBehaviour {

    [SerializeField] private float m_time; //Time till destruction

    /// <summary>Initialization</summary>
	void Start ()
    {
        //Destroy gameObject after m_time
        Destroy(gameObject, m_time);	
	}
	
}
