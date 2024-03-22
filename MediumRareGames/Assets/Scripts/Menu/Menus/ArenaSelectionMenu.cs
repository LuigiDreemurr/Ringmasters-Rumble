using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSelectionMenu : Menu<ArenaSelectionMenu> 
{

    [SerializeField] private List<string> m_arenaScenes = new List<string>(3)
    {
        "Arena_1",
        "Arena_2",
        "Arena_3"
    };

    public void LoadArena(int _ArenaIndex)
    {
        MenuUtility.Instance.LoadScene(m_arenaScenes[_ArenaIndex]);
    }

    public void LoadRandomArena()
    {
        LoadArena(Random.Range(0, m_arenaScenes.Count));
    }
}
