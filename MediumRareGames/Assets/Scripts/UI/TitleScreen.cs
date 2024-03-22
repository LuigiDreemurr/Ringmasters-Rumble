using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controller;
using System;

public class TitleScreen : MonoBehaviour {

    [SerializeField] private Controller.Controller[] Players = new Controller.Controller[1];
    [SerializeField] private MenuController Menu;
    [SerializeField] private GameObject Focus;
    [SerializeField] private GameObject MenuObj;
	// Use this for initialization
	void Start () {
		

        for(int i = 0; i < Players.Length; i++)
        {
            foreach(Controller.Button button in Enum.GetValues(typeof(Button)))
                Players[i].Subscribe(button, AnyButton);
        }
	}
	


    private void AnyButton(object _Sender, ButtonEventArgs _Args)
    {
        
        

        if (_Args.State == ButtonInputState.Up)
        {
            Menu.Btn_ChangeMenu(MenuObj);
            Menu.Btn_SelectObject(Focus);


            for (int i = 0; i < Players.Length; i++)
            {
                foreach (Controller.Button button in Enum.GetValues(typeof(Button)))
                    Players[i].UnSubscribe(button, AnyButton);
            }
        }

        
    }
}
