using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInput;
using XInputDotNetPure;

public class VibrationTest : MonoBehaviour
{
    [SerializeField] private PlayerIndex m_index;
    [SerializeField] private Vector2 m_motor = Vector2.one;
    [SerializeField] private float m_duration = 3;

	// Use this for initialization
	void Start ()
    {
        ControllerManager.Instance.GetController(m_index).Subscribe(Button.A, OnA);
	}
	
    void OnA(XInput.Controller _Controller, ButtonArgs _Args)
    {
        if(_Args.Action == ButtonAction.Down)
            _Controller.Vibrate(m_motor, m_duration);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
