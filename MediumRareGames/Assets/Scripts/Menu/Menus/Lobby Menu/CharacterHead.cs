using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Character")]
public class CharacterHead : ScriptableObject
{
    public enum KidType { Tiki, Bandit, Space, Blocky, KentuckyFried }

    [SerializeField] private KidType m_type;
    [SerializeField] private Sprite m_sprite;
    [SerializeField] private GameObject m_prefab;

    public KidType Type { get { return m_type; } }
    public Sprite Sprite { get { return m_sprite; } }
    public GameObject Prefab { get { return m_prefab; } }
}
