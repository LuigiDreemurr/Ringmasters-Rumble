using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XInput;

public class PlayerManager : MonoBehaviour
{
    private static List<PlayerContainer> s_players;
    public static List<PlayerContainer> Players { get { return s_players; } }

    [SerializeField] private GameObject m_playerPrefab;
    [SerializeField] private GamemodeController m_gamemodeController;

    void Start ()
    {
        //Simple check for making sure rounds start off running
        if(Time.timeScale != 1)
            Time.timeScale = 1;

        s_players = new List<PlayerContainer>();
        List<Level.Tile> spawnPoints = DeathHandler.GetSpawnPoints();

        foreach (PlayerInfo info in LobbyMenu.Players)
        {
            PlayerContainer container = new PlayerContainer(Instantiate(m_playerPrefab));
            
            //Set the right controller for playercontroller
            container.Player.GetComponent<PlayerController>().Input = ControllerManager.Instance.GetController(info.Index);
            container.Player.GetComponent<PlayerController>().Container = container;

            //Set the name
            container.Container.name = info.Name;

            //Set the color
            PlayerIdentifier identifier = container.Container.GetComponent<PlayerIdentifier>();
            identifier.Info = info;

            //Spawn the player
            Level.Tile spawnTile = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
            container.Container.GetComponent<DeathHandler>().Spawn(spawnTile.transform.position);
            spawnPoints.Remove(spawnTile); //Remove tile from spawn list so it doesnt use the same tile twice

            //Add the player to static list
            s_players.Add(container);
        }

        //Set the first player in control of the menus
        MenuManager.Instance.MenuController = ControllerManager.Instance.GetController(LobbyMenu.Players[0].Index);

        m_gamemodeController.Initialize(s_players.Select((p)=> { return p.Container; }).ToArray() );
    }
}

public class PlayerContainer
{
    private GameObject m_container;
    private GameObject m_player;

    public GameObject Container { get { return m_container; } }
    public GameObject Player { get { return m_player; } }

    public PlayerContainer(GameObject _Container)
    {
        m_container = _Container;
        m_player = _Container.transform.GetChild(0).gameObject;
    }
}
