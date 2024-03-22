using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Weapon;

public class SharkAI : MonoBehaviour {

    //[HideInInspector]
    public Carrier owner;
    public NavMeshAgent agent;

    public Carrier target;
    public float loseDistance = 30f;
    public float chompSpeed = 15f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(target != null)
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);

            //lose the target if they become too far from the shark
            if(dist > loseDistance) { agent.ResetPath(); target = null; }

            //otherwise, chase the player down
            else
            {
                agent.SetDestination(target.transform.position);
                //transform.position = Vector3.MoveTowards(transform.position, target.transform.position, chompSpeed);
                transform.LookAt(target.transform);
                Vector3 rot = transform.eulerAngles;
                rot.z = 0f;
                agent.transform.eulerAngles = rot;
            }
        }
	}

    private void OnTriggerEnter(Collider c)
    {
        Carrier p = c.GetComponent<Carrier>();
        if (p != null) //collided with player
        {

            if(p != owner && target == null)
            {
                target = p;
            }

        }
    }
}
