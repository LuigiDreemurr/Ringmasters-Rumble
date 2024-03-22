/*
-----------------------------------------------------------------------------
       Created By MATT Swanson
-----------------------------------------------------------------------------
   HealthDialController
       - controlls the health dial size based on health
          
   Details:
       - sets the image to the corisponding value via %
-----------------------------------------------------------------------------
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDialController : MonoBehaviour
{

    public GameObject player; //ref to the player to get their respected health/ammo script... could do parent though...
    private Image theDial; //holder for the image component
    private Health.Health healthCom; //holder for the health component

    // Use this for initialization
    void Start()
    {
        theDial = this.GetComponent<Image>();
        healthCom = player.GetComponent<Health.Health>();

        //Subscribe to health change event
        healthCom.OnChange += HealthChange;
    }

    /// <summary>Update the dial fill amount when the health is changed</summary>
    /// <param name="health">The health that was changed</param>
    private void HealthChange(Health.Health health)
    {
        theDial.fillAmount = health.Percent;
    }
}
