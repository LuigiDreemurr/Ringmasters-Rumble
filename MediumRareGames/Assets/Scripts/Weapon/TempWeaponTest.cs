using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempWeaponTest : MonoBehaviour {

	
	// Update is called once per frame
	void Update ()
    {
        Weapon.Carrier carrier = GetComponent<Weapon.Carrier>();
        
        //Discard/pickup testing
        if (Input.GetKeyDown(KeyCode.D))
            carrier.DiscardWeapon();
        else if (Input.GetKeyDown(KeyCode.G))
            carrier.PickupWeapon(Weapon.Type.GumballLauncher);
        else if (Input.GetKeyDown(KeyCode.S))
            carrier.PickupWeapon(Weapon.Type.SharkZooka);

        //Fire test
        if (Input.GetMouseButtonDown(0))
            carrier.Weapon.Fire();
	}
}
