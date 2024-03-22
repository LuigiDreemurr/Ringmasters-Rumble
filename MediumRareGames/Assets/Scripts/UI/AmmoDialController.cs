/*
-----------------------------------------------------------------------------
       Created By MATT Swanson
-----------------------------------------------------------------------------
   AmmoDialController
       - controlls dial size based on ammo
          
   Details:
       - sets the image to the corisponding value via %
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoDialController : MonoBehaviour
{

    public GameObject player; //ref to the player to get their respected health/ammo script... could do parent though...
    private Image theDial;//holder for the image component    
    private Weapon.Carrier weaponCarrier; //holder for the Ammo component

    // Use this for initialization
    void Start()
    {
        theDial = this.GetComponent<Image>();
        weaponCarrier = GetComponent<Weapon.Carrier>();
    }

    // Update is called once per frame
    void Update()
    {

        //theDial.fillAmount = weaponCarrier.Weapon.AmmoPercent / 2; //since we're working with a half dial the % needs to be halfed

    }

}