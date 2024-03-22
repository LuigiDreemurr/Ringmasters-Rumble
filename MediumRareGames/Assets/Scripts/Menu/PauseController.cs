using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour {

    [SerializeField] private Menu m_pauseMenu;
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
            MenuManager.Instance.ShowMenu(m_pauseMenu);
	}
}
