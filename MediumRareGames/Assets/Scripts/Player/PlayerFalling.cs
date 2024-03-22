/*
-----------------------------------------------------------------------------
       Created By Zachary Pilon
-----------------------------------------------------------------------------
   PlayerFalling
       - Simple class that tracks whether or not a character is falling, then will tell the PlayerController to slow movement for the character.
          
   Details:
       - 
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFalling : MonoBehaviour {

    [SerializeField] private float stunTime;


    private Rigidbody m_rigidbody;
    public bool isFalling;

    
    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        isFalling = false;
    }

    private void Update()
    {
        CheckFalling();
    }


    private void CheckFalling()
    {
        if (Mathf.Abs(m_rigidbody.velocity.y) >= 15)
        {
            isFalling = true;
        } 
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isFalling)
        {
            GetComponent<PlayerController>().Input.Vibrate(new Vector2(0.2f,0.2f), .1f);
            
            StartCoroutine(StunRoutine());
        }
    }

    private IEnumerator StunRoutine ()
    {

        yield return new WaitForSeconds(stunTime);

        isFalling = false;
        
    }
}
