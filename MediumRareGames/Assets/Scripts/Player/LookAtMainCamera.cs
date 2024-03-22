using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LookAtMainCamera : MonoBehaviour
{
    //[SerializeField] private bool m_xAxis = false;
    //[SerializeField] private bool m_yAxis = false;
    //[SerializeField] private bool m_zAxis = false;

    private Transform m_mainCamera;

    // Use this for initialization
    void Start ()
    {
        m_mainCamera = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update ()
    {

	}
}
