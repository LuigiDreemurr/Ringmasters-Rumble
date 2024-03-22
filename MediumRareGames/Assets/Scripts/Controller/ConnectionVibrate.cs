using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionVibrate : MonoBehaviour
{
    [SerializeField] private Vector2 m_motorPower = new Vector2(0.2f, 0.2f);
    [SerializeField] private float m_duration = 0.4f;

	// Use this for initialization
	void Start ()
    {
        XInput.ControllerManager.Instance.OnConnect += OnConnect;
        XInput.ControllerManager.Instance.OnDisconnect += OnDisconnect;
	}
	
    private void OnConnect(XInput.Controller _Controller)
    {
        _Controller.Vibrate(m_motorPower, m_duration);
    }

    private void OnDisconnect(XInput.Controller _Controller)
    {
        _Controller.StopVibration();
    }

    // Update is called once per frame
    void Update () {
		
	}
}
