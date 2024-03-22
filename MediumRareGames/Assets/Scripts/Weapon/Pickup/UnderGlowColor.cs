using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderGlowColor : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_underGlow;
    [SerializeField] private ParticleSystem m_rays;
    [SerializeField] private ParticleSystem m_chestGlow;
    [SerializeField] private Light m_spotLight;
    private Color m_color;

    public Color Color
    {
        get { return m_color; }
        set
        {
            m_color = value;

            var underGlowMain = m_underGlow.main;
            underGlowMain.startColor = new ParticleSystem.MinMaxGradient(m_color);

            var raysMain = m_rays.main;
            raysMain.startColor = new ParticleSystem.MinMaxGradient(m_color);

            var chestGlowMain = m_chestGlow.main;
            chestGlowMain.startColor = new ParticleSystem.MinMaxGradient(m_color);

            m_spotLight.color = m_color;
        }
    }
        
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
