using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShake : MonoBehaviour {

    public CameraShake camShake;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.R)) { camShake.Shake(ShakeType.Random, 3f); }
        if (Input.GetKeyDown(KeyCode.U)) { camShake.Shake(ShakeType.UpDown, 3f); }
        if (Input.GetKeyDown(KeyCode.B)) { camShake.Shake(ShakeType.BackForth, 3f); }
        if (Input.GetKeyDown(KeyCode.L)) { camShake.Shake(ShakeType.LeftRight, 3f); }
        if (Input.GetKeyDown(KeyCode.D)) { camShake.Shake(ShakeType.Diagnal, 3f); }
        if (Input.GetKeyDown(KeyCode.Escape)) { camShake.ResetShake(); }
        if (Input.GetKeyDown(KeyCode.Backspace)) { camShake.ResetShakeImmediately(); }
    }
}
