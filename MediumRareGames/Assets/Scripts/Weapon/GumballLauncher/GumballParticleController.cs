using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GumballParticleController : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] m_particleSystems;

    public void SetColor(Color _Color)
    {
        foreach(ParticleSystem system in m_particleSystems)
        {
            var main = system.main;
            main.startColor = _Color;
        }
    }

    public void Play()
    {
        foreach (ParticleSystem system in m_particleSystems)
        {
            system.Play();
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
