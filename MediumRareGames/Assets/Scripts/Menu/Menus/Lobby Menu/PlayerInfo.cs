using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInput;
using XInputDotNetPure;

[System.Serializable]
public class PlayerInfo
{
    [SerializeField] private PlayerIndex m_index;
    [SerializeField] private Color m_color;
    [SerializeField] private CharacterHead m_character;
    [SerializeField] private string m_name;

    public PlayerIndex Index { get { return m_index; } }
    public Color Color { get { return m_color; } }

    public CharacterHead Character
    {
        get { return m_character; }
        set { m_character = value; }
    }

    public string Name { get { return m_name; } }
}
