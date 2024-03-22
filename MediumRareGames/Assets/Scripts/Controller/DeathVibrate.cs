using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathVibrate : MonoBehaviour
{
    private PlayerController m_playerController;
    [SerializeField] private Vector2 m_vibrate = new Vector2(0.3f, 0.3f);
    [SerializeField] private float m_duration = 0.4f;
	// Use this for initialization
	void Start ()
    {
        m_playerController = GetComponent<PlayerController>();
        GetComponent<Health.Health>().OnDeath += OnDeath;	
	}
	
    private void OnDeath(Health.Health _Health, Health.DamageSource _Source)
    {
        m_playerController.Input.Vibrate(m_vibrate, m_duration);
    }
}
