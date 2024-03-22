using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempController : MonoBehaviour
{
    Animator animator;

    public float damp = 0.15f;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;

        animator.SetFloat("Horizontal", input.x, damp, Time.deltaTime);
        animator.SetFloat("Vertical", input.y, damp, Time.deltaTime);
		
	}
}
